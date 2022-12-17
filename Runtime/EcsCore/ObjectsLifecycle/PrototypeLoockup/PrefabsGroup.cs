using System;

namespace Library
{
    [Serializable]
    public struct PrefabsGroup
    {
        public string Name;
        public PrefabPair[] Pairs;


        public PrefabsGroup(string name)
        {
            Name = name;
            Pairs = Array.Empty<PrefabPair>();
        }
    }
}