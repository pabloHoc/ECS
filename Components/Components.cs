using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class Components
    {
        public EventBus EventBus { private get; set; }
        private readonly Dictionary<Type, IComponentPool> componentPools;
        private List<Type> componentTypesCache;

        public Components() 
        {
            componentPools = new Dictionary<Type, IComponentPool>();
            componentTypesCache = new List<Type>();
        }

        public T AddComponent<T>(uint entityId) where T : IComponent, new()
        {
            return (T)GetComponentPool<T>(true).AddComponent(entityId);
        }

        public ComponentPool<T> GetComponentPool<T>(bool createIfNotExists = false) where T : IComponent, new()
        {
            IComponentPool componentPool;
            if (!componentPools.TryGetValue(typeof(T), out componentPool))
            {
                if (createIfNotExists) {
                    componentPool = new ComponentPool<T>(100000);
                    componentPools.Add(typeof(T), componentPool);
                }
            }
            return (ComponentPool<T>)componentPool;
        }

        internal void RemoveComponentsFor(uint entityId)
        {
            foreach (var T in componentPools.Keys)
            {
                IComponentPool componentGroup;
                if (componentPools.TryGetValue(T, out componentGroup))
                {
                    componentGroup.RemoveComponents(entityId);
                }
            }
        }

        public List<T>[] GetComponents<T>() where T : IComponent, new() 
        {
            return ((ComponentPool<T>)GetComponentPool<T>()).GetComponents();
        }

        public void RemoveComponent<T>(uint entityId, T item) where T : IComponent, new()
        {
            GetComponentPool<T>().RemoveComponent(entityId, item);
        }

        public List<uint> GetEntitiesFor(Filter filter)
        {
            return filter.GetEntities(componentPools);
        }

        public List<Type> GetComponentTypesFor(uint entityId)
        {
            componentTypesCache.Clear();

            foreach (var keyValuePair in componentPools)
                if (keyValuePair.Value.EntityHasComponent(entityId))
                    componentTypesCache.Add(keyValuePair.Key);

            return componentTypesCache;
        }
    }
}