using System.Collections.Generic;
using UnityEngine;


namespace Library
{
    public class PoolCapacitiesProvider : MonoBehaviour
    {
        [SerializeField] private PoolCapacityData[] poolCapacitiesData;


        public PoolCapacity[] GetPoolCapacities()
        {
            var poolCapacities = new List<PoolCapacity>();
            
            foreach (var poolCapacityData in poolCapacitiesData)
            {
                poolCapacities.AddRange(poolCapacityData.PoolCapacities);
            }

            return poolCapacities.ToArray();
        }
    }
}