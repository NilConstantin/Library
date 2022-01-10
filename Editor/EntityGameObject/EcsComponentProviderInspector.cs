using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EcsComponentProvider<>), true)]
    public class EcsComponentProviderInspector : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            DrawScriptFieldFromMonoBehaviour();

            serializedObject.Update();

            var componentProperty = serializedObject.FindProperty("Component");
            var componentFields = componentProperty
                .serializedObject
                .targetObject
                .GetType()
                .GetField("Component", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .FieldType
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            var isAllFieldsHidden = true;
            foreach (var componentField in componentFields)
            {
                var hasHideInInspectorAttribute = Attribute.IsDefined(componentField, typeof(HideInInspector));
                if (!hasHideInInspectorAttribute)
                {
                    isAllFieldsHidden = false;
                    break;
                }
            }

            if (!isAllFieldsHidden)
            {
                EditorGUILayout.PropertyField(componentProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}