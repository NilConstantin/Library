using System;
using UnityEngine;
using UnityEditor;


namespace Library.Editor
{
    public class ReplacerWindow : EditorWindow
    {
        public GameObject[] ReplacementTargets;
        public GameObject ReplacementPrefab = default;


        [MenuItem("Window/Replacer Window")]
        public static void OpenWindow()
        {
            var window = GetWindow<ReplacerWindow>();
            window.titleContent = new GUIContent("Replacer");
        }


        private void OnGUI()
        {
            ScriptableObject target = this;
            SerializedObject so = new SerializedObject(target);

            SerializedProperty replacementTargetsProperty = so.FindProperty("ReplacementTargets");
            EditorGUILayout.PropertyField(replacementTargetsProperty, true); // True means show children
            
            if (GUILayout.Button("Clear"))
            {
                ReplacementTargets = Array.Empty<GameObject>();
            }
            
            SerializedProperty replacementPrefabProperty = so.FindProperty("ReplacementPrefab");
            EditorGUILayout.PropertyField(replacementPrefabProperty); // True means show children

            so.ApplyModifiedProperties(); // Remember to apply modified properties

            if (GUILayout.Button("Replace"))
            {
                for (var i = 0; i < ReplacementTargets.Length; i++)
                {
                    var replacementTarget = ReplacementTargets[i];
                    Replace(replacementTarget, ReplacementPrefab, i);
                }
            }
        }

        private void Replace(GameObject replacementTarget, GameObject replacementPrefab, int index)
        {
            var gameObject = PrefabUtility.InstantiatePrefab(replacementPrefab) as GameObject;
            gameObject.transform.SetParent(replacementTarget.transform.parent); 
            gameObject.transform.position = replacementTarget.transform.position;
            gameObject.transform.rotation = replacementTarget.transform.rotation;
            gameObject.transform.localScale = replacementTarget.transform.localScale;

            gameObject.name = $"{replacementPrefab.name}";
            if (index > 0)
            {
                gameObject.name += $" ({index})";
            }
        }
    }
}