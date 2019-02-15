using System;

namespace ECS
{
    public interface IEntityViewBag
    {
        void AddEntityView(IEntityView entityView);
        IEntityView GetEntityView(Guid entityGuid);
        void RemoveEntityView(Guid entityGuid);
    }
}