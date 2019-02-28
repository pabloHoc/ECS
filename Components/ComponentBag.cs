using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECS {
    public sealed class ComponentBag<T> : IComponentBag where T : IComponent
    {
        private DynamicArray<T> components;

        public Type ComponentType
        {
            get 
            {
                return typeof(T);
            }
        }

        public ComponentBag(int initialSize)
        {
            components = new DynamicArray<T>(initialSize); 
        }
        public void AddComponent(IComponent component) {
            components.AddAt((T)component, component.EntityId);
        }

        public void RemoveComponents(uint entityId) {
            components.RemoveAllAt(entityId);
        }

        public void RemoveComponent(uint entityId, T item) {
            components.RemoveAt(entityId, item);
        }

        public DynamicArray<T> GetComponents() {
            return components;                                
        }

        public IComponent[] GetComponentsAt(uint entityId)
        {
            var componentsForEntity = components.GetAt(entityId);
            if (componentsForEntity == null)
                return new IComponent[0];

            var array = new IComponent[componentsForEntity.Length];

            for (int i = 0; i < array.Length; i++)
                array[i] = componentsForEntity[i];

            return array;
        }
    }
}
