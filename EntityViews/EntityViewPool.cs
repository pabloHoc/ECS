using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class EntityViewPool
    {
        public EventBus EventBus { private get; set; }
        private readonly Dictionary<Type, IEntityViewBag> entityViewBags;

        public EntityViewPool() 
        {
            entityViewBags = new Dictionary<Type, IEntityViewBag>();
        }

        public void AddEntityView<T>(T entityView) where T : IEntityView
        {
            var entityViewBag = GetEntityViewBag<T>();
            entityViewBag.AddEntityView(entityView);
        }
        public IEntityViewBag AddEntityViewType<T>() where T : IEntityView
        {
            IEntityViewBag entityViewBag;
            entityViewBags.TryGetValue(typeof(T), out entityViewBag);
            
            if (entityViewBag == null) {
                entityViewBag = new EntityViewBag<T>();
                entityViewBags.Add(typeof(T), entityViewBag);
                return entityViewBag;
            }

            return null;
        }

        public void CheckAndAddEntityView(Guid entityGuid, IEnumerable<IComponent> entityComponents)
        {
            var entityViewTypes = GetEntityViewTypesForComponents(entityComponents);

            foreach(var type in entityViewTypes) {
                var entityView = BuildEntityViewFromComponents(entityGuid, type, entityComponents);
                entityViewBags[type].AddEntityView(entityView);
            }
        }

        internal void RemoveEntityViewsWithComponent<T>(Guid entityGuid) where T : IComponent
        {
            var entityViewTypes = GetEntityViewTypesForComponent<T>();

            foreach (var type in entityViewTypes)
            {
                entityViewBags[type].RemoveEntityView(entityGuid);
            }
        }

        private IEntityView BuildEntityViewFromComponents(Guid entityGuid, Type entityViewBlueprint, IEnumerable<IComponent> entityComponents)
        {
            var entityView = (IEntityView)Activator.CreateInstance(entityViewBlueprint);

            foreach (var component in entityComponents) {
                var prop = entityView.GetType().GetProperty(component.GetType().ToString());
                prop.SetValue(entityView, component);
            }

            var entityIdProp = entityView.GetType().GetProperty("EntityGuid");
            entityIdProp.SetValue(entityView, entityGuid);

            return entityView;
        }

        
        private IEnumerable<Type> GetEntityViewTypesForComponent<T>() where T : IComponent 
        {
            var entityViewTypes = new List<Type>();

            foreach (var key in entityViewBags.Keys) 
            {
                foreach (var property in key.GetProperties()) {
                    if (property.Name == typeof(T).Name) 
                        entityViewTypes.Add(key);
                }
            }

            return entityViewTypes;
        }

        private IEnumerable<Type> GetEntityViewTypesForComponents(IEnumerable<IComponent> entityComponents) 
        {
            var entityViewTypes = new List<Type>();

            foreach (var key in entityViewBags.Keys) 
            {
                entityViewTypes.Add(key);

                foreach (var property in key.GetProperties()) {
                    if (property.Name != "EntityGuid") 
                    {
                        var match = false;
                        foreach (var component in entityComponents) {
                            if (property.Name == component.GetType().Name)
                                match = true;
                        }
                        if (!match) 
                            entityViewTypes.Remove(key);
                    }
                }
            }

            return entityViewTypes;
        }

        public IEnumerable<IEntityView> GetEntityViewsFor(Guid entityGuid)
        {
            var entityViews = new List<IEntityView>();

            foreach (var entityViewBag in entityViewBags.Values)
            {
                var entityView = entityViewBag.GetEntityView(entityGuid);
                if (entityView != null) {
                    entityViews.Add(entityView);
                }
            }

            return entityViews;
        }

        private IEntityViewBag GetEntityViewBag<T>() where T : IEntityView
        {
            IEntityViewBag entityViewBag;
            if (!entityViewBags.TryGetValue(typeof(T), out entityViewBag))
            {
                entityViewBag = new EntityViewBag<T>();
                entityViewBags.Add(typeof(T), entityViewBag);
            }
            return entityViewBag;
        }

        public IEnumerable<IEntityViewBag> GetEntityViewBags() {
            var entityViewBagCollection = new List<IEntityViewBag>();

            foreach (var key in entityViewBags.Keys)
            {
                IEntityViewBag entityViewBag;
                entityViewBags.TryGetValue(key, out entityViewBag);
                if (entityViewBag != null)
                    entityViewBagCollection.Add(entityViewBag);
            }

            return entityViewBagCollection;
        }
  
    }
}