using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    public class MixamoAnimationRenamer
    {
        [MenuItem("Assets/Skeleton/Cut Mixamo Prefix")]
        public static void CutMixamoPrefixInAnimations()
        {
            var animationClips = CollectAssetsFromSelection<AnimationClip>();

            foreach (var animationClip in animationClips)
            {
                var pathToFile = AssetDatabase.GetAssetPath(animationClip);
                var fullPathToFile = Path.Combine(Application.dataPath.Replace("Assets", string.Empty), pathToFile);
                var fileData = File.ReadAllText(fullPathToFile);

                fileData = ConvertNaming(fileData);
                File.WriteAllText(fullPathToFile, fileData);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        [MenuItem("GameObject/Skeleton/Cut Mixamo Prefix", false, 0)]
        public static void CutMixamoPrefixInGameObject()
        {
            var selectedGameObjects = CollectAssetsFromSelection<GameObject>();

            foreach (var selectedGameObject in selectedGameObjects)
            {
                foreach (var gameObject in selectedGameObject.GetComponentsInChildren<Transform>())
                {
                    gameObject.name = ConvertNaming(gameObject.name);
                }
            }
        }


        private static string ConvertNaming(string sourceData)
        {
            return sourceData.Replace("Armature.001/", string.Empty).Replace("mixamorig:", string.Empty);
        }


        private static List<T> CollectAssetsFromSelection<T>() where T : Object
        {
            var animationClips = new List<T>();

            if (Selection.objects.Length > 1)
            {
                animationClips = Selection.objects
                    .Select(x => x as T)
                    .Where(x => x != null)
                    .ToList();
            }
            else if (Selection.activeObject is T clip)
            {
                animationClips.Add(clip);
            }

            return animationClips;
        }
    }
}