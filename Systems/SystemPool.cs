using System;
using System.Collections.Generic;

namespace ECS
{
    public sealed class SystemPool
    {
        public EventBus EventBus { private get; set; }
        public IList<ISystem> Systems { get; private set; }

        public SystemPool()
        {
            Systems = new List<ISystem>();
        }

        public void AddSystem(ISystem system) 
        {
            Systems.Add(system);
        }

        public void RemoveSystem(ISystem system) {
            Systems.Remove(system);
        }
        
        public void SubscribeSystemsToEntityView<T>(IEntityViewBag entityViewBag) where T : IEntityView
        {
            foreach(var system in Systems) {
                system.CheckEntityViewAndSubscribe<T>(entityViewBag);                    
            }
        }
    }    
}
