using System;
using System.Collections.Generic;

namespace ECS
{
    public interface IFilter
    {
         List<uint> GetEntities(Dictionary<Type, IComponentPool> componentGroups);
         bool Matches(List<Type> components);
    }
}