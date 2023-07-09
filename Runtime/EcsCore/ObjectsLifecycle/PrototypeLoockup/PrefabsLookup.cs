using System;
using System.Collections.Generic;
using UnityEngine;


namespace Library
{
    [CreateAssetMenu(fileName = "PrefabsLookup", menuName = "Ecs/Prefabs Loockup")]
    public class PrefabsLookup : ScriptableObject
    {
        public string PathToPrefabsIdsFile;
        public string PrefabsIdsFileNamespace;
        
        [SerializeField] private PrefabsGroup[] groups = Array.Empty<PrefabsGroup>();

        private readonly Dictionary<int, string> assetPaths = new Dictionary<int, string>();


        public void Init()
        {
            assetPaths.Clear();
            foreach (var group in groups)
            {
                foreach (var pair in group.Pairs)
                {
                    assetPaths.Add(pair.PrefabId, pair.AssetPath);
                }
            }
        }


        public bool Contains(int prefabId)
        {
            return assetPaths.ContainsKey(prefabId);
        }


        public string this[int key] => assetPaths[key];
    }
}
