using System;

namespace ECS
{
    public sealed class EventBus
    {
        private EntityPool entityPool;
        private ComponentPool componentPool;
        private SystemPool systemPool;

        public EventBus(EntityPool entityPool, ComponentPool componentPool, SystemPool systemPool) 
        {
            this.entityPool = entityPool;
            this.componentPool = componentPool;
            this.systemPool = systemPool;

            this.componentPool.EventBus = this;
            this.systemPool.EventBus = this;
        }

        public void OnComponentAdded<T>(T component) where T : IComponent 
        {
            componentPool.AddComponent(component);

            var entityComponents = componentPool.GetComponentsFor(component.EntityId);
            systemPool.CheckEntityAndSubscribe(component.EntityId, entityComponents);
        }

        public void OnComponentRemoved<T>(uint entityId, T item) where T : IComponent 
        {
            componentPool.RemoveComponent<T>(entityId, item);
            systemPool.UnsubscribeEntityWithComponent<T>(entityId);
        }

        public void OnEntityRemoved(uint entityId)
        {
            componentPool.RemoveComponentsFor(entityId);
            systemPool.UnsubscribeEntityWithId(entityId);
            entityPool.RemoveEntity(entityId);
        }
    }
}