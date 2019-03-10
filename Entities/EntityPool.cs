using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECS
{
    public sealed class EntityPool
    {
        uint lastEntityId;
        private List<uint> entities;
        private Queue<uint> deadEntities;

        public EntityPool() 
        {
            entities = new List<uint>();
            deadEntities = new Queue<uint>();
            lastEntityId = 0;
        }

        internal uint AddEntity() 
        {
            if (deadEntities.Count > 0)
                return deadEntities.Dequeue();

            var entity = lastEntityId;
            entities.Add(entity);
            lastEntityId++;
            return entity;
        }

        internal void RemoveEntity(uint entityId) 
        {
            if (entities.Contains(entityId) && !deadEntities.Contains(entityId))
                deadEntities.Enqueue(entityId);
        }
    }
}