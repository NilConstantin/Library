using System.Collections.Generic;
using UnityEngine;


namespace GameLibrary
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> array)
        {
            var count = array.Count;
            for (var i = 0; i < count; i++)
            {
                var randomIndex = i + Random.Range(0, count - i);
                (array[randomIndex], array[i]) = (array[i], array[randomIndex]);
            }
        }
    }
}