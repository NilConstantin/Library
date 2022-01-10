using UnityEngine;


namespace Library
{
    public static class GameObjectExtensions
    {
        public static void SetLayerRecursively(this GameObject gameObject, int layer) {
            
            gameObject.layer = layer;
            
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetLayerRecursively(layer);
            }
        }
    }
}