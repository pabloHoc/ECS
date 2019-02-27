using System.Collections.Generic;

namespace ECS {
    public interface ISystem
    {
        EventBus EventBus { set; }
        ComponentPool ComponentPool { set; }
        void Execute(float delta);
        void CheckEntityAndSubscribe(uint entityId, IEnumerable<IComponent> components);
        void UnsubscribeEntityWithComponent<T>(uint entityId) where T : IComponent;
        void UnsubscribeEntityWithId(uint entityId);
    }
} 
