#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


namespace Library.Editor
{
    [CustomEditor(typeof(PrefabsLookup), true)]
    public class PrefabsLoockupEditor : CustomInspectorEditor
    {
        private const string Groups = "groups";
        
        private const string Pairs = "Pairs";
        private const string PrefabId = "PrefabId";
        private const string AssetPath = "AssetPath";
        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(2.0f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Force Update", GUILayout.ExpandWidth(true), GUILayout.Width(250)))
                {
                    UpdatePrefabsIds();
                }
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();   
        }

        
        private void OnEnable()
        {
            UpdatePrefabsIds();
        }

        
        private void OnDisable()
        {
            UpdatePrefabsIds();
        }
        
        
        private void OnLostFocus()
        {
            UpdatePrefabsIds();
        }
      
        
        private void UpdatePrefabsIds()
        {
            //ToDo: Better flow for get assets needed
            var existingEcsEntityGameObjects = Resources.LoadAll<EcsEntityGameObject>(string.Empty);
        
            foreach (var existingEcsEntityGameObject in existingEcsEntityGameObjects)
            {
                ResetPrefabId(existingEcsEntityGameObject.gameObject, -1);
            }

            var groupsProperty = serializedObject.FindProperty(Groups);
            for (var i = 0; i < groupsProperty.arraySize; i++)
            {
                var pairsProperty = groupsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(Pairs);
        
                for (int j = 0; j < pairsProperty.arraySize; j++)
                {
                    var assetPath = pairsProperty.GetArrayElementAtIndex(j).FindPropertyRelative(AssetPath).stringValue;
                    var prefabId = pairsProperty.GetArrayElementAtIndex(j).FindPropertyRelative(PrefabId).intValue;
        
                    if (!string.IsNullOrEmpty(assetPath))
                    {
                        var gameObject = Resources.Load<GameObject>(Library.AssetPath.ConvertToResourcesPath(assetPath));
                        ResetPrefabId(gameObject, prefabId);
                    }
                }
            }
            SaveAndRefreshAssets();
        }
        
        
        private void ResetPrefabId(GameObject gameObject, int prefabId)
        {
            if (gameObject != null)
            {
                EcsEntityGameObject hybridEntityData = gameObject.GetComponent<EcsEntityGameObject>();
                if (hybridEntityData != null)
                {
                    hybridEntityData.PrefabId = prefabId;
                    EditorUtility.SetDirty(hybridEntityData);
                }
                else
                {
                    Debug.LogError($"Entity has no {nameof(EcsEntityGameObject)} component. Entity name: {gameObject.name}");
                }
            }
        }
    }
}

#endif