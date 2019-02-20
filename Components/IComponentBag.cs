using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IComponentBag
    {
        void AddComponent(IComponent component);
        void RemoveComponent(uint EntityId);
        IComponent GetComponent(uint EntityId);
    }
}