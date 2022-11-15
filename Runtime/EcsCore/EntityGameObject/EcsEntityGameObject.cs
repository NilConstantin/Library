using System;
using Leopotam.Ecs;
using UnityEngine;


namespace Library
{
    public class EcsEntityGameObject : MonoBehaviour
    {
        public BaseEcsComponentProvider[] ComponentsProviders = Array.Empty<BaseEcsComponentProvider>();
        public EcsEntityGameObject[] NestedEcsEntityGameObjects = Array.Empty<EcsEntityGameObject>();
        public int PrefabId = -1;
        public EcsEntity Entity = default;
        public int IndexInPool = -1;


        public virtual void EnableInPool()
        {
            gameObject.SetActive(true);
        }
        
        
        public virtual void DisableInPool()
        {
            gameObject.SetActive(false);
        }
    }
}