using System;

namespace ECS
{
    public struct Entity
    {
        public Guid Guid { get; private set; }

        public Entity(Guid guid) {
            Guid = guid;
        }
    }
}