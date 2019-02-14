namespace ECS
{
    public sealed class EventBus
    {
        private SystemRepository systemRepository;
        private ComponentRepository componentRepository;
        private EntityViewRepository entityViewRepository;

        public EventBus(SystemRepository systemRepository, ComponentRepository componentRepository, EntityViewRepository entityViewRepository) 
        {
            this.systemRepository = systemRepository;
            this.componentRepository = componentRepository;
            this.entityViewRepository = entityViewRepository;

            this.systemRepository.EventBus = this;
            this.componentRepository.EventBus = this;
            this.entityViewRepository.EventBus = this;
        }

        public void OnComponentAdded<T>(T component) where T : IComponent 
        {
            componentRepository.AddComponent(component);

            var entityComponents = componentRepository.GetComponentsFor(component.EntityId);
            entityViewRepository.CheckAndAddEntityView(component.EntityId, entityComponents);
        }

        public void OnComponentRemoved<T>(long entityId) where T : IComponent 
        {
            componentRepository.RemoveComponent<T>(entityId);

            entityViewRepository.RemoveEntityViewsWithComponent<T>(entityId);
        }

        public void OnEntityRemoved(long entityId)
        {

        }

        public void OnEntityViewTypeAdded<T>() where T : IEntityView 
        {
            var entityViewBag = entityViewRepository.AddEntityViewType<T>();

            if (entityViewBag != null) {
                systemRepository.SubscribeSystemsToEntityView<T>(entityViewBag);
            }
        }
    }
}