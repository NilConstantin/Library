using System;

namespace Library
{
    [Serializable]
    public struct PrefabPair
    {
        public int PrefabId;

        [AssetPath(typeof(UnityEngine.Object))]
        public string AssetPath;

        public PrefabPair(int prefabId, string assetPath)
        {
            PrefabId = prefabId;
            AssetPath = assetPath;
        }
    }
}