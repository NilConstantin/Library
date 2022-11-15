using System.Collections.Generic;
using UnityEngine;


namespace Library
{
    public static class GameObjectExtensions
    {
        public static T[] GetComponentsOnlyInChildren<T>(this MonoBehaviour script) where T:class
        {
            var group = new List<T>();
 
            //collect only if its an interface or a Component
            if (typeof(T).IsInterface 
                || typeof(T).IsSubclassOf(typeof(Component)) 
                || typeof(T) == typeof(Component)) 
            {
                foreach (Transform child in script.transform) 
                {
                    group.AddRange (child.GetComponentsInChildren<T> ());
                }
            }
         
            return group.ToArray ();
        }
        
        
        public static void SetLayerRecursively(this GameObject gameObject, int layer) {
            
            gameObject.layer = layer;
            
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
            }
        }
    }
}