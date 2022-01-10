#if UNITY_EDITOR

namespace Library.Editor
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class ScriptableObjectUtility
    {
        //	This makes it easy to create, name and place unique new ScriptableObject asset files.
        /// <summary>
        /// </summary>
        public static ScriptableObject CreateAsset<T>(string name) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            SetupAsset(asset, name);
            return asset;
        }

        public static ScriptableObject CreateAsset(string className)
        {
            var asset = ScriptableObject.CreateInstance(className);
            SetupAsset(asset, className);
            return asset;
        }

        private static void SetupAsset(ScriptableObject asset, string className)
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, className));

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        public static string GetNameForUi(string fullName)
        {
            var penultimatePointIndex = fullName.LastIndexOf(".");
            return penultimatePointIndex == -1 ? fullName : fullName.Substring(penultimatePointIndex + 1, fullName.Length - penultimatePointIndex - 1);
        }
    }
}

#endif