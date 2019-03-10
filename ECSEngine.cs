using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECSTest;

namespace ECS
{
    public partial class ECSEngine {
        private EntityPool entityPool;
        private SystemPool systemPool;
        private Components components;
        private EventBus eventBus;
        private Dictionary<Filter, List<uint>> groupsCache;

        public ECSEngine() 
        {
            entityPool = new EntityPool();
            components = new Components();
            systemPool = new SystemPool();
            groupsCache = new Dictionary<Filter, List<uint>>();
            
            eventBus = new EventBus(this, entityPool, components, systemPool);
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
            system.Engine = this;
            system.EventBus = eventBus;
            system.Components = components;
            systemPool.AddSystem(system);
        }

        public uint AddEntity()
        {
            return entityPool.AddEntity();
        }

        public void RemoveEntity(uint entityId)
        {
            eventBus.OnEntityRemoved(entityId);
        }

        public T AddComponentToEntity<T>(uint entityId) where T : IComponent, new() 
        {
            return eventBus.OnComponentAdded<T>(entityId);
        }

        public void OnEntityUpdated(uint entityId)
        {
            var componentTypes = components.GetComponentTypesFor(entityId);

            foreach (var keyValuePair in groupsCache) 
            {
                if (keyValuePair.Key.Matches(componentTypes)) 
                {
                    if (!keyValuePair.Value.Contains(entityId))   
                        keyValuePair.Value.Add(entityId);
                } else
                {
                    keyValuePair.Value.Remove(entityId);
                }
            }
        }

        public List<uint> GetEntitiesFor(Filter filter) 
        {
            List<uint> entities;

            if(!groupsCache.TryGetValue(filter, out entities)) 
            {
                entities = components.GetEntitiesFor(filter);
                groupsCache.Add(filter, entities);
            }

            return entities;
        }
    }    
}