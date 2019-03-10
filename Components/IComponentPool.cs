using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IComponentPool
    {
        Type ComponentType { get; }
        IComponent AddComponent(uint entityId);
        void RemoveComponents(uint entityId);
        List<uint> EntityIdsCache { get; }
        bool EntityHasComponent(uint entityId);
    }
}