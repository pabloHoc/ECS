using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IComponentBag
    {
        Type ComponentType { get; }
        void AddComponent(IComponent component);
        void RemoveComponents(uint entityId);
        IComponent[] GetComponentsAt(uint entityId);
    }
}