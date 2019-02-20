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
            var componentBag = GetComponentBag<T>(true);
            componentBag.AddComponent(component);
        }

        public IEnumerable<IComponent> GetComponentsFor(uint entityId)
        {
            var components = new List<IComponent>();

            foreach (var componentBag in componentBags.Values)
            {
                var component = componentBag.GetComponent(entityId);
                if (component != null) {
                    components.Add(component);
                }
            }

            return components;
        }

        public IComponentBag GetComponentBag<T>(bool createIfNotExists = false) where T : IComponent
        {
            IComponentBag componentBag;
            if (!componentBags.TryGetValue(typeof(T), out componentBag))
            {
                if (createIfNotExists) {
                    componentBag = new ComponentBag<T>(100000);
                    componentBags.Add(typeof(T), componentBag);
                }
            }
            return componentBag;
        }

        internal void RemoveComponentsFor(uint entityId)
        {
            foreach (var T in componentBags.Keys)
            {
                IComponentBag componentBag;
                if (componentBags.TryGetValue(T, out componentBag))
                {
                    componentBag.RemoveComponent(entityId);
                }
            }
        }

        public DynamicArray<T> GetComponents<T>() where T : IComponent 
        {
            return ((ComponentBag<T>)GetComponentBag<T>()).GetComponents();
        }

        public void RemoveComponent<T>(uint entityId) where T : IComponent
        {
            var componentBag = GetComponentBag<T>();
            componentBag.RemoveComponent(entityId);
        }
    }
}   