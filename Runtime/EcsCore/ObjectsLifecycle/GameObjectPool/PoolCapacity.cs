using System;

namespace Library
{
    [Serializable]
    public struct PoolCapacity
    {
        public int PrefabId;
        public int Capacity;


        public PoolCapacity(int prefabId, int capacity)
        {
            PrefabId = prefabId;
            Capacity = capacity;
        }
    }
}