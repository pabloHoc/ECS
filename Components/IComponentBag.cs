using System;

namespace ECS
{
    public interface IComponentBag
    {
        void AddComponent(IComponent component);
        void RemoveComponent(Guid EntityGuid);
        IComponent GetComponent(Guid EntityGuid);
    }
}