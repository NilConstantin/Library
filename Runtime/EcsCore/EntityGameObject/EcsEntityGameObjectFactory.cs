using Leopotam.Ecs;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Library
{
    public class EcsEntityGameObjectFactory
    {
        private const int DefaultCapacity = 0;

        private static readonly Vector3 HiddenPosition = new Vector3(-10000.0f, -10000.0f, -10000.0f);

        private PrefabsLookup prefabsLookup;
        private PoolCapacity[] poolCapacities;
        private EcsWorld world;
        private GameObject root;
        private GameObject poolsRoot;

        private Dictionary<int, GameObjectPool> pools;
        private Dictionary<int, GameObject> cachedPrefabs;


        public void Init(PrefabsLookup prefabsLookup, PoolCapacity[] poolCapacities, EcsWorld world, GameObject root, GameObject poolsRoot)
        {
            this.prefabsLookup = prefabsLookup;
            this.poolCapacities = poolCapacities;
            this.world = world;
            this.root = root;
            this.poolsRoot = poolsRoot;

            pools = new Dictionary<int, GameObjectPool>(this.poolCapacities.Length);
            cachedPrefabs = new Dictionary<int, GameObject>(this.poolCapacities.Length);

            for (var i = 0; i < this.poolCapacities.Length; i++)
            {
                var poolCapacity = poolCapacities[i];
                CreatePool(poolCapacity.PrefabId);
            }
        }


        public EcsEntityGameObject Produce(int prefabId, Vector3 position)
        {
            return Produce(prefabId, position, Quaternion.identity, Vector3.one);
        }
        

        public EcsEntityGameObject Produce(int prefabId, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            //Debug.Log($"Create entity. PrefabId: {prefabId}. Frame: {Time.frameCount}");
            
            if (!pools.ContainsKey(prefabId))
            {
                Debug.LogWarning($"Create: Not contains pool for {prefabId}. Check pool capacities.");

                CreatePool(prefabId);
            }

            var pool = pools[prefabId];
            var requestItemResult = pool.RequestItem();

            var entityGameObject = pool.Items[requestItemResult.IndexInPool];
            entityGameObject.IndexInPool = requestItemResult.IndexInPool;

            var transform = entityGameObject.transform;
            transform.SetParent(root.transform);
            
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = scale;

            SetupEntityInEntityGameObject(entityGameObject);
            
            entityGameObject.EnableInPool();

            return entityGameObject;
        }


        public void CreateEntityForEntityGameObject(EcsEntityGameObject entityGameObject)
        {
            var entity = world.NewEntity();
            entity.Get<EcsEntityGameObjectOwner>().EntityGameObject = entityGameObject;
            entityGameObject.Entity = entity;
        }


        public void RegisterEntity(EcsEntityGameObject entityGameObject)
        {
            if (entityGameObject.IndexInPool != -1)
            {
                return;
            }
            
            var prefabId = entityGameObject.PrefabId;
            var gameObject = entityGameObject.gameObject;

            PrepareProvidersEntityGameObject(entityGameObject);

            if (!prefabsLookup.Contains(entityGameObject.PrefabId))
            {
                //Debug.LogWarning($"Entity Game Object \"{entityGameObject}\" not in prefabs lookup");

                SetupEntityInEntityGameObject(entityGameObject);
                return;
            }

#if UNITY_EDITOR
            var assetPath = prefabsLookup[prefabId];
            var originalGameObject = Resources.Load<GameObject>(AssetPath.ConvertToResourcesPath(assetPath));

            var components = gameObject.GetComponents<MonoBehaviour>();
            var originalComponents = originalGameObject.GetComponents<MonoBehaviour>();

            if (components.Length != originalComponents.Length)
            {
                throw new UnityException($"Game object {gameObject} not match to original. Prefab id: {prefabId}");
            }

            for (var i = 0; i < originalComponents.Length; i++)
            {
                if (components[i].GetType() != originalComponents[i].GetType())
                {
                    throw new UnityException($"Game object {gameObject} not match to original. Prefab id: {prefabId}");
                }
            }

            originalGameObject = null;
            Resources.UnloadUnusedAssets();
#endif

            var pool = pools.ContainsKey(prefabId) ? GetPool(prefabId) : CreatePool(prefabId);
            var itemIndex = pool.LinkItem(gameObject);

            entityGameObject.IndexInPool = itemIndex;

            SetupEntityInEntityGameObject(entityGameObject);
        }


        public void Recycle(EcsEntityGameObject entityGameObject)
        {
            //Debug.Log($"Destroy entity: {entityGameObject.Entity}. " +
            //          $"PrototypeId: {entityGameObject.PrefabId}. " +
            //          $"IndexInPool: {entityGameObject.IndexInPool}. " +
            //          $"Frame: {Time.frameCount}");

            if (!prefabsLookup.Contains(entityGameObject.PrefabId))
            {
                //Debug.LogWarning($"Entity Game Object \"{entityGameObject}\" not in prefabs lookup");

                entityGameObject.Entity.Destroy();
                Object.Destroy(entityGameObject.gameObject);
                return;
            }

            GameObjectPool pool = GetPool(entityGameObject.PrefabId);
            if (entityGameObject.IndexInPool < 0 || entityGameObject.IndexInPool >= pool.Items.Length)
            {
                throw new UnityException($"Index in pool of out range. " +
                    $"Index: {entityGameObject.IndexInPool}. " +
                    $"Pool length: {pool.Items.Length}. " +
                    $"Frame: {Time.frameCount}");
            }

            entityGameObject.Entity.Destroy();

            var poolItem = pool.Items[entityGameObject.IndexInPool];

            poolItem.DisableInPool();

            poolItem.transform.SetParent(pool.RootTransform);
            poolItem.transform.localPosition = HiddenPosition;

            pool.RecycleItemByIndex(entityGameObject.IndexInPool);
        }


        private void PrepareProvidersEntityGameObject(EcsEntityGameObject entityGameObject)
        {
            entityGameObject.ComponentsProviders = entityGameObject.GetComponents<BaseEcsComponentProvider>();
            
            foreach (var componentProvider in entityGameObject.ComponentsProviders)
            {
                componentProvider.Prepare(entityGameObject);
            }
        }
        
        
        private void SetupEntityInEntityGameObject(EcsEntityGameObject entityGameObject)
        {
            if (entityGameObject.Entity.IsNull() || !entityGameObject.Entity.IsAlive())
            {
                CreateEntityForEntityGameObject(entityGameObject);
            }

            foreach (var componentProvider in entityGameObject.ComponentsProviders)
            {
                componentProvider.AddComponentToEntity(entityGameObject);
            }
        }


        private GameObjectPool CreatePool(int prefabId)
        {
            var poolCreationSettings = new GameObjectPoolCreationSettings
            {
                PrefabId = prefabId,
                PoolsRoot = poolsRoot,
                NamePostfix = GetPoolNamePostfix(prefabId),
                ConstructMethod = ConstructEntity,
                Capacity = GetCapacity(prefabId)
            };

            var pool = new GameObjectPool(poolCreationSettings);
            pools.Add(prefabId, pool);
            return pool;
        }


        private GameObjectPool GetPool(int prefabId)
        {
            pools.TryGetValue(prefabId, out var pool);

#if UNITY_EDITOR
            if (pool == null)
            {
                throw new UnityException($"Pool not found for prefab id {prefabId}");
            }
#endif

            return pool;
        }


        private EcsEntityGameObject ConstructEntity(int prefabId, int indexInPool, Transform parent)
        {
#if UNITY_EDITOR
            if (!prefabsLookup.Contains(prefabId))
            {
                throw new UnityException($"Prefab lookup not contains pool for {prefabId}. Check pool capacities.");
            }
#endif

            if (!cachedPrefabs.ContainsKey(prefabId))
            {
                var assetPath = prefabsLookup[prefabId];
                cachedPrefabs[prefabId] = Resources.Load<GameObject>(AssetPath.ConvertToResourcesPath(assetPath));
            }

            var prefab = cachedPrefabs[prefabId];
            var gameObject = Object.Instantiate(prefab, HiddenPosition, Quaternion.identity, parent);
            gameObject.name = prefab.name;

            var entityGameObject = gameObject.GetComponent<EcsEntityGameObject>();

#if UNITY_EDITOR
            if (entityGameObject == null)
            {
                throw new UnityException($"No {nameof(EcsEntityGameObject)} component found");
            }

            if (entityGameObject.PrefabId != prefabId)
            {
                throw new UnityException($"Prefab ids not match. Parameter: {prefabId}, game object: {entityGameObject.PrefabId}");
            }
#endif

            PrepareProvidersEntityGameObject(entityGameObject);

            entityGameObject.IndexInPool = indexInPool;

            entityGameObject.DisableInPool();

            return entityGameObject;
        }
        

        private int GetCapacity(int prefabId)
        {
            foreach (var poolCapacity in poolCapacities)
            {
                if (poolCapacity.PrefabId == prefabId)
                {
                    return poolCapacity.Capacity;
                }
            }

            return DefaultCapacity;
        }


        private string GetPoolNamePostfix(int prefabId)
        {
#if UNITY_EDITOR
            if (!prefabsLookup.Contains(prefabId))
            {
                throw new UnityException($"Prefabs lookup not contains pool for {prefabId}. Check pool capacities.");
            }
#endif

            return Path.GetFileNameWithoutExtension(prefabsLookup[prefabId]);
        }
    }
}