using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class ComponentPool
    {
        public EventBus EventBus { private get; set; }
        private readonly Dictionary<Type, IComponentBag> componentBags;

        public ComponentPool() 
        {
            componentBags = new Dictionary<Type, IComponentBag>();
        }

        public void AddComponent<T>(T component) where T : IComponent
        {
            var componentBag = GetComponentBag<T>();
            componentBag.AddComponent(component);
        }

        public IEnumerable<IComponent> GetComponentsFor(Guid entityGuid)
        {
            var components = new List<IComponent>();

            foreach (var componentBag in componentBags.Values)
            {
                var component = componentBag.GetComponent(entityGuid);
                if (component != null) {
                    components.Add(component);
                }
            }

            return components;
        }

        private IComponentBag GetComponentBag<T>() where T : IComponent
        {
            IComponentBag componentBag;
            if (!componentBags.TryGetValue(typeof(T), out componentBag))
            {
                componentBag = new ComponentBag<T>();
                componentBags.Add(typeof(T), componentBag);
            }
            return componentBag;
        }

        public void RemoveComponent<T>(Guid entityGuid) where T : IComponent
        {
            var componentBag = GetComponentBag<T>();
            componentBag.RemoveComponent(entityGuid);
        }

        public IEnumerable<IComponentBag> GetComponentBags() {
            var componentBagCollection = new List<IComponentBag>();

            foreach (var key in componentBags.Keys)
            {
                IComponentBag componentManager;
                componentBags.TryGetValue(key, out componentManager);
                if (componentManager != null)
                    componentBagCollection.Add(componentManager);
            }

            return componentBagCollection;
        }
    }
}