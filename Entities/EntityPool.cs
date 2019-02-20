using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECS {
    public sealed class EntityPool
    {
        uint lastEntityId;
        private List<Entity> entities;
        private Queue<Entity> deadEntities;

        public EntityPool() 
        {
            entities = new List<Entity>();
            deadEntities = new Queue<Entity>();
            lastEntityId = 0;
        }

        internal Entity AddEntity() 
        {
            if (deadEntities.Count > 0)
                return deadEntities.Dequeue();

            var entity = new Entity(lastEntityId);
            entities.Add(entity);
            lastEntityId++;
            return entity;
        }

        internal void RemoveEntity(uint entityId) 
        {
            var entity = entities.Find(e => e.Id == entityId);
            if (!entity.Equals(default(Entity)) && !deadEntities.Contains(entity))
                deadEntities.Enqueue(entity);
        }
    }
}
