using UnityEngine;


namespace Library
{
    [CreateAssetMenu(fileName = "PoolCapacityData", menuName = "Game Data/Pool Capacity Data")]
    public class PoolCapacityData : ScriptableObject
    {
        [SerializeField] private PoolCapacity[] poolCapacities;


        public PoolCapacity[] PoolCapacities => poolCapacities;
    }
}