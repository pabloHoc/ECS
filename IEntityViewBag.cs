using System;

namespace ECS
{
    public interface IEntityViewBag
    {
        void AddEntityView(IEntityView entityView);
        IEntityView GetEntityView(long entityId);
        void RemoveEntityView(long entityId);
    }
}