using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ECS {
    sealed class EntityManager
    {
        private long nextEntityId; 
        private HashSet<long> entities;

        public EntityManager() {
            nextEntityId = 0;
            entities = new HashSet<long>();
        }

        internal long AddEntity() {
            entities.Add(nextEntityId);
            return ++nextEntityId;
        }

        internal void RemoveEntity(long entityId) {
            entities.Remove(entityId);
        }
    }
}
