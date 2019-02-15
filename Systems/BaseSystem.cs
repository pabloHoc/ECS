using System.Collections.Generic;

namespace ECS
{
    public abstract class BaseSystem<T> : ISystem where T : IEntityView
    {
        public EntityViewBag<T> EntityViewBag { get; set; }

        public BaseSystem() 
        {
            EntityViewBag = new EntityViewBag<T>();
        }

        public virtual void Update(float delta) 
        {
        }
        
        public virtual void CheckEntityViewAndSubscribe<U>(IEntityViewBag entityViewBag) where U : IEntityView
        {
            if (typeof(T).Equals(typeof(U)))
                EntityViewBag = (EntityViewBag<T>)entityViewBag;
        }
    }
}