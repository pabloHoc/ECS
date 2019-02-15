using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECS
{
    public sealed class ECSEngine {
        private EntityPool entityPool;
        private SystemPool systemPool;
        private ComponentPool componentPool;
        private EntityViewPool entityViewPool;
        private EventBus eventBus;

        public ECSEngine() 
        {
            entityPool = new EntityPool();
            systemPool = new SystemPool();
            componentPool = new ComponentPool();
            entityViewPool = new EntityViewPool();
            
            eventBus = new EventBus(systemPool, componentPool, entityViewPool);
        }

        public void Update(float delta) 
        {
            // List<Task> tasks = new List<Task>();
            foreach (var system in systemPool.Systems)
            {
                system.Update(delta);
                // Task task = new Task(() => system.Update(delta));
                // tasks.Add(task);
                // task.Start();
            }
            // Task.WaitAll(tasks.ToArray());

        }

        public void AddSystem(ISystem system)
        {
            systemPool.AddSystem(system);
        }

        public Entity AddEntity()
        {
            return entityPool.AddEntity();
        }

        public void AddComponentToEntity<T>(T component) where T : IComponent 
        {
            eventBus.OnComponentAdded<T>(component);
        }

        public void LoadEntityView<T>() where T : IEntityView 
        {
            eventBus.OnEntityViewTypeAdded<T>();
        }
    }    
}
