using System.Collections.Generic;
using UnityEngine;


namespace Library
{
    [CreateAssetMenu(fileName = "PrefabsLookup", menuName = "Ecs/Prefabs Loockup")]
    public class PrefabsLookup : ScriptableObject
    {
        [SerializeField] private PrefabsGroup[] groups = new PrefabsGroup[0];

        private Dictionary<int, string> assetPathes = new Dictionary<int, string>();


        public void Init()
        {
            for (var i = 0; i < groups.Length; i++)
            {
                var group = groups[i];
                for (var j = 0; j < group.Pairs.Length; j++)
                {
                    var pair = group.Pairs[j];
                    assetPathes.Add(pair.PrefabId, pair.AssetPath);
                }
            }
        }


        public bool Contains(int prefabId)
        {
            return assetPathes.ContainsKey(prefabId);
        }


        public string this[int key] 
        {
            get
            {
                return assetPathes[key];
            }
        }
    }
}
