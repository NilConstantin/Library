using System;

namespace Library
{
    [Serializable]
    public struct PrefabPair
    {
        public int PrefabId;

        [AssetPath(typeof(UnityEngine.Object))]
        public string AssetPath;
    }
}