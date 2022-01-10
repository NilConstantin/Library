using System;
using UnityEngine;


namespace Library
{
    public sealed class GameObjectPool
    {
        private const int MinSize = 8;

        private readonly int prefabId = default;

        private readonly Transform poolRootTransform = default;

        private readonly Func<int, int, Transform, EcsEntityGameObject> constructMethod = default;

        public EcsEntityGameObject[] Items = new EcsEntityGameObject[MinSize];

        private int itemsCount;

        private int[] requestedIndices = new int[MinSize];
        private int requestedIndicesCount;

        public Transform RootTransform => poolRootTransform;


        public GameObjectPool(GameObjectPoolCreationSettings creationSettings)
        {
            prefabId = creationSettings.PrefabId;
            var poolRoot = new GameObject($"Pool-{creationSettings.NamePostfix}");
            poolRoot.transform.SetParent(creationSettings.PoolsRoot.transform);
            poolRootTransform = poolRoot.transform;

            constructMethod = creationSettings.ConstructMethod;

            Create(creationSettings.Capacity);
        }


        public GameObjectPoolRequestItemResult RequestItem()
        {
            GameObjectPoolRequestItemResult result;

            if (requestedIndicesCount > 0)
            {
                requestedIndicesCount--;
                result = new GameObjectPoolRequestItemResult
                {
                    IndexInPool = requestedIndices[requestedIndicesCount],
                    IsRecycled = true
                };
            }
            else
            {
                result = new GameObjectPoolRequestItemResult
                {
                    IndexInPool = itemsCount,
                    IsRecycled = false
                };
                InitItemAtEnd(constructMethod(prefabId, result.IndexInPool, poolRootTransform));
                Debug.LogWarning($"Heavy creation happened. Prefab id: {prefabId}");
            }

            return result;
        }


        public void RecycleItemByIndex(int index)
        {
            if (requestedIndicesCount == requestedIndices.Length)
            {
                var newSize = requestedIndicesCount > 0 ? requestedIndicesCount << 1 : 1;
                Array.Resize(ref requestedIndices, newSize);
            }
            requestedIndices[requestedIndicesCount] = index;
            requestedIndicesCount++;
        }


        public int LinkItem(GameObject gameObject)
        {
            var item = gameObject.GetComponent<EcsEntityGameObject>();

#if UNITY_EDITOR
            if (item == null)
            {
                throw new UnityException($"Game object {gameObject.name} has no  {nameof(EcsEntityGameObject)} component");
            }
#endif

            var index = itemsCount;
            InitItemAtEnd(item);
            return index;
        }


        private void Create(int capacity)
        {
            Array.Resize(ref Items, capacity);
            for (var i = 0; i < Items.Length; i++)
            {
                Items[i] = constructMethod(prefabId, i, poolRootTransform);
            }
            itemsCount = Items.Length;

            Array.Resize(ref requestedIndices, capacity);
            for (var i = 0; i < requestedIndices.Length; i++)
            {
                requestedIndices[i] = i;
            }
            requestedIndicesCount = requestedIndices.Length;
        }


        private void InitItemAtEnd(EcsEntityGameObject item)
        {
            if (itemsCount == Items.Length)
            {
                var newSize = itemsCount > 0 ? itemsCount << 1 : 1;
                Array.Resize(ref Items, newSize);
                Debug.LogWarning($"Pool increased from {itemsCount} to {itemsCount << 1}. Prefab id: {prefabId}.");
            }
            Items[itemsCount] = item;
            itemsCount++;
        }
    }
}