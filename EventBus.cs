using System;

namespace ECS
{
    public sealed class EventBus
    {
        private ECSEngine engine;
        private EntityPool entityPool;
        private Components components;
        private SystemPool systemPool;

        public EventBus(ECSEngine engine, EntityPool entityPool, Components componentPool, SystemPool systemPool) 
        {
            this.engine = engine;
            this.entityPool = entityPool;
            this.components = componentPool;
            this.systemPool = systemPool;

            this.components.EventBus = this;
            this.systemPool.EventBus = this;
        }

        public T OnComponentAdded<T>(uint entityId) where T : IComponent, new() 
        {
            var component = components.AddComponent<T>(entityId);
            engine.OnEntityUpdated(entityId);
            return component;
        }

        public void OnComponentRemoved<T>(uint entityId, T component) where T : IComponent , new()
        {
            components.RemoveComponent<T>(entityId, component);
            engine.OnEntityUpdated(entityId);
        }

        /**
         * TODO: callback for components that points to other entityIds
         */
        public void OnEntityRemoved(uint entityId)
        {
            components.RemoveComponentsFor(entityId);
            entityPool.RemoveEntity(entityId);
            engine.OnEntityUpdated(entityId);
        }
    }
}