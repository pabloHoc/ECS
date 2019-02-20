using System;

namespace ECS
{
    public struct Entity
    {
        public uint Id { get; private set; }

        public Entity(uint id) {
            Id = id;
        }
    }
}