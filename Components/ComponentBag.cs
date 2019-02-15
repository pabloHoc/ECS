using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECS {
    internal sealed class ComponentBag<T> : IComponentBag where T : IComponent
    {
        private List<T> components;

        public ComponentBag()
        {
            components = new List<T>(); 
        }
        public void AddComponent(IComponent component) {
            components.Add((T)component);
        }

        public void RemoveComponent(Guid entityGuid) {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (components[i].EntityGuid == entityGuid)
                    components.RemoveAt(i);
            }
        }

        public IComponent GetComponent(Guid entityGuid)
        {
            foreach(var component in components) {
                if (component.EntityGuid == entityGuid) {
                    return component;
                }
            }
            return null;
        }
    }
}
