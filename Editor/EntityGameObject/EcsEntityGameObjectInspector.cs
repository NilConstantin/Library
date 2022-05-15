using UnityEditor;


namespace Library.Editor
{
    [CustomEditor(typeof(EcsEntityGameObject), true)]
    public class EcsEntityGameObjectInspector : CustomInspectorEditor
    {
        public override void OnInspectorGUI()
        {
            DrawScriptFieldFromMonoBehaviour();

            // var componentsProvidersProperty = serializedObject.FindProperty(nameof(EcsEntityGameObject.ComponentsProviders));
            // EditorGUI.BeginDisabledGroup(true);
            // EditorGUILayout.PropertyField(componentsProvidersProperty);
            // EditorGUI.EndDisabledGroup();

            var prefabIdProperty = serializedObject.FindProperty(nameof(EcsEntityGameObject.PrefabId));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(prefabIdProperty);
            EditorGUI.EndDisabledGroup();

            var ecsEntityGameObject = serializedObject.targetObject as EcsEntityGameObject;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Entity", ecsEntityGameObject.Entity.ToString());
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Index In Pool", ecsEntityGameObject.IndexInPool.ToString());
            EditorGUI.EndDisabledGroup();
        }
    }
}