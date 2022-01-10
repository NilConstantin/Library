using System;
using UnityEngine;


namespace Library
{
    public sealed class GameObjectPoolCreationSettings
    {
        public int PrefabId;
        public GameObject PoolsRoot;
        public string NamePostfix;
        public Func<int, int, Transform, EcsEntityGameObject> ConstructMethod;
        public int Capacity;
    }
}