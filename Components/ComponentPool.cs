using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECS
{
    public sealed class ComponentPool<T> : IComponentPool where T : IComponent, new()
    {
        public Type ComponentType { get { return typeof(T); } }
        public List<uint> EntityIdsCache { get; private set; } 
        private List<T>[] components;
        private Queue<T> reusableComponents;

        public ComponentPool(int initialSize)
        {
            EntityIdsCache = new List<uint>();
            components = new List<T>[initialSize]; 
            reusableComponents = new Queue<T>(); 
        }

        private IComponent GetComponent(uint entityId)
        {
            T component;
            if (reusableComponents.Count > 0)
                component = reusableComponents.Dequeue();
            else    
                component = new T();

            component.EntityId = entityId;
            
            return component;
        }

        public IComponent AddComponent(uint entityId) {
            var component = GetComponent(entityId);

            if (component.EntityId > components.Length)
                ResizeArray(component.EntityId);

            if (components[component.EntityId] == null) 
                components[component.EntityId] = new List<T>();
            
            if (components[component.EntityId].Count == 0)
                EntityIdsCache.Add(component.EntityId);
            
            components[component.EntityId].Add((T)component);

            return component;
        }

        public void RemoveComponents(uint entityId) {
            if (components[entityId] != null)
            {
                foreach (var component in components[entityId])
                    reusableComponents.Enqueue(component);

                components[entityId].Clear();
                EntityIdsCache.Remove(entityId);
            }
        }
        
        public void RemoveComponent(uint entityId, T item) {
            if (components[entityId] != null) 
            {
                reusableComponents.Enqueue(item);
                components[entityId].Remove(item);
                if (components[entityId].Count == 0)
                    EntityIdsCache.Remove(entityId);
            }
        }

        public List<T>[] GetComponents() {
            return components;                                
        }

        public List<T> GetComponentsFor(uint entityId)
        {
            if (entityId >= components.Length)
                throw new IndexOutOfRangeException();

            if (components[entityId] == null)
                components[entityId] = new List<T>();

            return components[entityId];
        }

        private void ResizeArray(uint newMax)
        {
            var newArray = new List<T>[(int)(newMax / 0.8)];
            components.CopyTo(newArray, 0);
            components = newArray;
        }

        public T GetUnique(uint index)
        {
            if (index >= components.Length)
                throw new IndexOutOfRangeException();

            return components[index][0];
        }

        public bool EntityHasComponent(uint entityId)
        {
            if (components[entityId] != null && components[entityId].Count > 0)
                return true;
                
            return false;
        }
    }
}