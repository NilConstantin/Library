using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    public class PsdUiBuilderWindow : EditorWindow
    {
        private FontAssetLookup fontAssetLookup;
        private Texture2D sourcePsdTexture;
        private GameObject rootGameObject;
        

        [MenuItem("Window/Psd Ui Builder")]
        public static void OpenWindow()
        {
            var window = GetWindow<PsdUiBuilderWindow>();
            window.titleContent = new GUIContent("Psd Ui Builder");
        }
        

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                fontAssetLookup = (FontAssetLookup)EditorGUILayout.ObjectField("Root Game Object", fontAssetLookup, typeof (FontAssetLookup), false);   
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                sourcePsdTexture = (Texture2D)EditorGUILayout.ObjectField("Source Psd", sourcePsdTexture, typeof (Texture2D), false);   
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                rootGameObject = (GameObject)EditorGUILayout.ObjectField("Root Game Object", rootGameObject, typeof (GameObject), true);   
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Go"))
            {
                // var v = "98.34175";
                // object vv = v;
                // int fontSize = (int)Convert.ToSingle(vv, CultureInfo.InvariantCulture);
                // Debug.Log(fontSize);
                new PsdUiBuilder().BuildUi(fontAssetLookup, rootGameObject, sourcePsdTexture);
            }
        }
    }
}