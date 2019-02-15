using System;

namespace ECS
{
    public interface IEntityView
    {
         Guid EntityGuid { get; }
    }
}