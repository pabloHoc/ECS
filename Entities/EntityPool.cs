using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECS {
    sealed class EntityPool
    {
        private List<Entity> entities;

        public EntityPool() {
            entities = new List<Entity>();
        }

        internal Entity AddEntity() {
            var entity = new Entity(Guid.NewGuid());
            entities.Add(entity);
            return entity;
        }

        internal void RemoveEntity(Entity entity) {
            entities.Remove(entity);
        }
    }
}
