using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS
{
    public partial class Filter 
    {
        public Type Type { get; set;}
        public List<Type> AllOfComponentTypes = new List<Type>();
        public List<uint> allOfEntities = new List<uint>();
        private HashSet<uint> distinctCache = new HashSet<uint>();    
        private HashSet<uint> duplicatesCache = new HashSet<uint>();
        private static List<Filter> filterCache = new List<Filter>();
        private static List<Type> typeCache = new List<Type>();

        public static Filter For<T>() where T : IComponent
        {
            foreach(var filter in filterCache)
                if (filter.AllOfComponentTypes.Count == 1 && filter.AllOfComponentTypes.Contains(typeof(T)))
                    return filter;

            var newFilter = new Filter();
            newFilter.AllOfComponentTypes.Add(typeof(T));
            filterCache.Add(newFilter);

            return newFilter;
        } 

        public static Filter AllOf(params Filter[] filters)
        {
            foreach (var filter in filters)        
                typeCache.AddRange(filter.AllOfComponentTypes);

            foreach(var filter in filterCache)
            {
                var found = true;

                foreach (var type in typeCache)
                    if (!filter.AllOfComponentTypes.Contains(type))
                        found = false;

                if (found)
                    return filter;
            }

            var newFilter = new Filter();
            newFilter.AllOfComponentTypes.AddRange(typeCache);
            filterCache.Add(newFilter);                    
            
            typeCache.Clear();

            return newFilter;
        }

        public List<uint> GetEntities(Dictionary<Type, IComponentPool> componentGroups)
        {
            UpdateAllOfEntities(componentGroups);
            return allOfEntities;
        }

        private void UpdateAllOfEntities(Dictionary<Type, IComponentPool> componentGroups)
        {
            foreach (var keyValuePair in componentGroups)
                if (AllOfComponentTypes.Contains(keyValuePair.Key))
                    foreach (var item in keyValuePair.Value.EntityIdsCache)
                        if (!distinctCache.Add(item))
                            duplicatesCache.Add(item);

            allOfEntities.AddRange(duplicatesCache);

            distinctCache.Clear();
            duplicatesCache.Clear();
        }

        public bool Matches(List<Type> componentsType)
        {
            foreach (var componentType in componentsType)
                if (!AllOfComponentTypes.Contains(componentType))
                    return false;
                
            return true;
        }
    }
}