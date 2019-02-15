using System;

namespace ECS {
    public interface IComponent
    {
        Guid EntityGuid { get; } 
    }
}
