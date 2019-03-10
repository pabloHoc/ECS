using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ECS
{
    public sealed class SystemPool
    {
        public EventBus EventBus { private get; set; }
        private IList<ISystem> Systems { get; set; }

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

        public void ExecuteSystems(float delta)
        {
            // List<Task> tasks = new List<Task>();
            // Task task = new Task(() => system.Update(delta));
            // tasks.Add(task);
            // task.Start();
            // Task.WaitAll(tasks.ToArray());
            foreach(var system in Systems) {
                system.Execute(delta);
            }
        }
    }    
}