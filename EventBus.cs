using System;

namespace ECS
{
    public sealed class EventBus
    {
        private SystemPool systemPool;
        private ComponentPool componentPool;
        private EntityViewPool entityViewPool;

        public EventBus(SystemPool systemPool, ComponentPool componentPool, EntityViewPool entityViewPool) 
        {
            this.systemPool = systemPool;
            this.componentPool = componentPool;
            this.entityViewPool = entityViewPool;

            this.systemPool.EventBus = this;
            this.componentPool.EventBus = this;
            this.entityViewPool.EventBus = this;
        }

        public void OnComponentAdded<T>(T component) where T : IComponent 
        {
            componentPool.AddComponent(component);

            var entityComponents = componentPool.GetComponentsFor(component.EntityGuid);
            entityViewPool.CheckAndAddEntityView(component.EntityGuid, entityComponents);
        }

        public void OnComponentRemoved<T>(Guid entityGuid) where T : IComponent 
        {
            componentPool.RemoveComponent<T>(entityGuid);

            entityViewPool.RemoveEntityViewsWithComponent<T>(entityGuid);
        }

        public void OnEntityRemoved(Guid entityGuid)
        {

        }

        public void OnEntityViewTypeAdded<T>() where T : IEntityView 
        {
            var entityViewBag = entityViewPool.AddEntityViewType<T>();

            if (entityViewBag != null) {
                systemPool.SubscribeSystemsToEntityView<T>(entityViewBag);
            }
        }
    }
}