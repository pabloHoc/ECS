using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECS
{
    public sealed class ECSEngine {
        private EntityManager entityManager;
        private SystemRepository systemRepository;
        private ComponentRepository componentRepository;
        private EntityViewRepository entityViewRepository;
        private EventBus eventBus;

        public ECSEngine() 
        {
            entityManager = new EntityManager();
            systemRepository = new SystemRepository();
            componentRepository = new ComponentRepository();
            entityViewRepository = new EntityViewRepository();
            
            eventBus = new EventBus(systemRepository, componentRepository, entityViewRepository);
        }

        public void Update(float delta) 
        {
            List<Task> tasks = new List<Task>();
            foreach (var system in systemRepository.Systems)
            {
                // system.Update(delta);
                Task task = new Task(() => system.Update(delta));
                tasks.Add(task);
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());

        }

        public void AddSystem(ISystem system)
        {
            systemRepository.AddSystem(system);
        }

        public long AddEntity()
        {
            return entityManager.AddEntity();
        }

        public void AddComponentToEntity<T>(long entityId, T component) where T : IComponent 
        {
            eventBus.OnComponentAdded<T>(component);
        }

        public void LoadEntityView<T>() where T : IEntityView 
        {
            eventBus.OnEntityViewTypeAdded<T>();
        }
    }    
}
