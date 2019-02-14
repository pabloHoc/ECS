using System;

namespace ECS
{
    public interface IComponentBag
    {
        void AddComponent(IComponent component);
        void RemoveComponent(long entityId);
        IComponent GetComponent(long entityId);
    }
}