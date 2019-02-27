using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECS
{
    public sealed class ECSEngine {
        private EntityPool entityPool;
        private SystemPool systemPool;
        private ComponentPool componentPool;
        private EventBus eventBus;

        public ECSEngine() 
        {
            entityPool = new EntityPool();
            componentPool = new ComponentPool();
            systemPool = new SystemPool();
            
            eventBus = new EventBus(entityPool, componentPool, systemPool);
        }

        public void Update(float delta) 
        {
            systemPool.ExecuteSystems(delta);
            // List<Task> tasks = new List<Task>();
                // Task task = new Task(() => system.Update(delta));
                // tasks.Add(task);
                // task.Start();
            // Task.WaitAll(tasks.ToArray());

        }

        public void AddSystem<T>() where T : ISystem, new() {
            var system = new T();
            system.EventBus = eventBus;
            system.ComponentPool = componentPool;
            systemPool.AddSystem(system);
        }

        public Entity AddEntity()
        {
            return entityPool.AddEntity();
        }

        public void AddComponentToEntity<T>(T component) where T  : IComponent 
        {
            eventBus.OnComponentAdded<T>(component);
        }

        public void RemoveEntitiesTest()
        {
            for (uint i = 0; i < 11; i++)
            {
                eventBus.OnEntityRemoved(i);
            }
        }
    }    
}
