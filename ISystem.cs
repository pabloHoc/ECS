using System.Collections.Generic;

namespace ECS {
    public interface ISystem
    {
        void Update(float delta);
        void CheckEntityViewAndSubscribe<T>(IEntityViewBag entityViewBag) where T : IEntityView;
    }
} 
