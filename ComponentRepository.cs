using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class ComponentRepository
    {
        public EventBus EventBus { private get; set; }
        private readonly Dictionary<Type, IComponentBag> componentManagers;

        public ComponentRepository() 
        {
            componentManagers = new Dictionary<Type, IComponentBag>();
        }

        public void AddComponent<T>(T component) where T : IComponent
        {
            var componentBag = GetComponentBag<T>();
            componentBag.AddComponent(component);
        }

        public IEnumerable<IComponent> GetComponentsFor(long entityId)
        {
            var components = new List<IComponent>();

            foreach (var componentManager in componentManagers.Values)
            {
                var component = componentManager.GetComponent(entityId);
                if (component != null) {
                    components.Add(component);
                }
            }

            return components;
        }

        private IComponentBag GetComponentBag<T>() where T : IComponent
        {
            IComponentBag componentManager;
            if (!componentManagers.TryGetValue(typeof(T), out componentManager))
            {
                componentManager = new ComponentBag<T>();
                componentManagers.Add(typeof(T), componentManager);
            }
            return componentManager;
        }

        public void RemoveComponent<T>(long entityId) where T : IComponent
        {
            var componentBag = GetComponentBag<T>();
            componentBag.RemoveComponent(entityId);
        }

        public IEnumerable<IComponentBag> GetComponentBags() {
            var componentManagerCollection = new List<IComponentBag>();

            foreach (var key in componentManagers.Keys)
            {
                IComponentBag componentManager;
                componentManagers.TryGetValue(key, out componentManager);
                if (componentManager != null)
                    componentManagerCollection.Add(componentManager);
            }

            return componentManagerCollection;
        }
    }
}