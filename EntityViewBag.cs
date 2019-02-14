using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ECS {
    public sealed class EntityViewBag<T> : IEntityViewBag where T : IEntityView
    {
        public List<T> EntityViews { get; private set; }

        public EntityViewBag()
        {
            EntityViews = new List<T>(); 
        }
        public void AddEntityView(IEntityView component) {
            EntityViews.Add((T)component);
        }

        public void RemoveEntityView(long entityId) {
            for (int i = EntityViews.Count - 1; i >= 0; i--)
            {
                if (EntityViews[i].EntityId == entityId)
                    EntityViews.RemoveAt(i);
            }
        }

        public IEntityView GetEntityView(long entityId)
        {
            foreach(var entityView in EntityViews) {
                if (entityView.EntityId == entityId) {
                    return entityView;
                }
            }
            return null;
        }
    }
}
