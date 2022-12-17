using UnityEngine;
using System.Collections.Generic;


namespace GameLibrary
{
    public static class UnityExtensions
    {
        public static T[] GetComponentsOnlyInChildren<T>(this MonoBehaviour script, bool includeInactive = false) where T : class
        {
            List<T> group = new List<T>();

            //collect only if its an interface or a Component
            if (typeof(T).IsInterface ||
                typeof(T).IsSubclassOf(typeof(Component)) ||
                typeof(T) == typeof(Component))
            {
                foreach (Transform child in script.transform)
                {
                    group.AddRange(child.GetComponentsInChildren<T>(includeInactive));
                }
            }

            return group.ToArray();
        }
    }
}