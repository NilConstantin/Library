using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    [CustomPropertyDrawer(typeof(PrefabPair))]
    public class PrefabPairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var idRect = new Rect(position.x, position.y, 120, position.height);
            var assetPathRect = new Rect(position.x + 125, position.y, position.width - 125, position.height);

            EditorGUI.PropertyField(idRect, property.FindPropertyRelative(nameof(PrefabPair.PrefabId)), GUIContent.none);
            EditorGUI.PropertyField(assetPathRect, property.FindPropertyRelative(nameof(PrefabPair.AssetPath)), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}