using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECS {
    internal sealed class ComponentBag<T> : IComponentBag where T : IComponent
    {
        private DynamicArray<T> components;

        public ComponentBag(int initialSize)
        {
            components = new DynamicArray<T>(initialSize); 
        }
        public void AddComponent(IComponent component) {
            components.AddAt((T)component, component.EntityId);
        }

        public void RemoveComponent(uint entityId) {
            components.RemoveAt(entityId);
        }

        public DynamicArray<T> GetComponents() {
            return components;                                
        }

        public IComponent GetComponent(uint entityId)
        {
            return components.GetAt(entityId);
        }
    }
}
