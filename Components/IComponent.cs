using System;

namespace ECS {
    public interface IComponent
    {
        uint EntityId { get; }
    }
}
