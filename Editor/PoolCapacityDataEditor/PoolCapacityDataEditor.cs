#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;


namespace Library.Editor
{
    [CustomEditor(typeof(PoolCapacityData), true)]
    public class PoolCapacityDataEditor : CustomInspectorEditor
    {
        private const string PoolCapacities = "poolCapacities";
        private const string PrefabId = "PrefabId";
        private const string Prefab = "Prefab";
        private const string Capacity = "Capacity";

        private const string Title = "Pool Capacities";

        private EcsEntityGameObject[] existingEcsEntityGameObjects = default;

        private TableView tableView = default;

        private void OnEnable()
        {
            //ToDo: Better flow for get assets needed
            existingEcsEntityGameObjects = Resources.LoadAll<EcsEntityGameObject>(string.Empty);

            var poolCapacitiesProperty = serializedObject.FindProperty(PoolCapacities);
            tableView = new TableView(serializedObject, poolCapacitiesProperty, Title,
                new TableColumn[]
                {
                    new TableColumn
                    {
                        Header = PrefabId.ToCamelCaseWithSpaces(),
                        WidthFactor = 0.3333f,
                        FieldName = PrefabId
                    },
                    new TableColumn
                    {
                        Header = Prefab,
                        WidthFactor = 0.3333f,
                        CustomDrawCallback = (Rect rect, SerializedProperty element) =>
                        {
                            var prefabId = element.FindPropertyRelative(PrefabId).intValue;
                            var prefabName = "Not found";
                            var hybridEntityData = existingEcsEntityGameObjects.FirstOrDefault(x => x.PrefabId == prefabId);
                            if (hybridEntityData != null)
                            {
                                prefabName = hybridEntityData.name;
                            }
                            EditorGUI.LabelField(rect, prefabName);
                        }
                    },
                    new TableColumn
                    {
                        Header = Capacity.ToCamelCaseWithSpaces(),
                        WidthFactor = 0.3333f,
                        FieldName = Capacity
                    }
                },
                TableFoldoutMode.NotFoldout,
                TableRowDragableMode.Dragable,
                TableRemoveMode.WithoutRemove,
                null,
                null);
        }

        private void OnDisable()
        {
            existingEcsEntityGameObjects = null;
            Resources.UnloadUnusedAssets();
        }

        public override void OnInspectorGUI()
        {
            DrawScriptFieldFromScriptableObject();

            serializedObject.Update();

            tableView.Draw();

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif