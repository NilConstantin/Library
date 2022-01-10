#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;


namespace Library.Editor
{
    //ToDo: Fix Delete
    [CustomEditor(typeof(PrefabsLookup), true)]
    public class PrefabsLoockupEditor : CustomInspectorEditor
    {
        private const float ButtonWidth = 100;

        private const string Groups = "groups";

        private const string Name = "Name";
        private const string Pairs = "Pairs";
        private const string PrefabId = "PrefabId";
        private const string AssetPath = "AssetPath";

        private TableView[] groupTableViews;
        string newGroupName;
        PrefabLoockupEditorState state;

        private void OnEnable()
        {
            state = PrefabLoockupEditorState.Editing;

            var groupsProperty = serializedObject.FindProperty(Groups);

            groupTableViews = new TableView[groupsProperty.arraySize];

            for (var i = 0; i < groupTableViews.Length; i++)
            {
                var groupProperty = groupsProperty.GetArrayElementAtIndex(i);
                var groupName = groupProperty.FindPropertyRelative(Name).stringValue;
                var pairsProperty = groupProperty.FindPropertyRelative(Pairs);
                groupTableViews[i] = CreateGroupTableView(i, groupName, pairsProperty);
            }

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


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptFieldFromScriptableObject();

            DrawPrefabsGroups();

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


        protected void DrawPrefabsGroups()
        {
            serializedObject.Update();

            var groupsProperty = serializedObject.FindProperty(Groups);

            for (var i = 0; i < groupsProperty.arraySize; i++)
            {
                groupTableViews[i].Draw();
            }

            DrawAddGroup(groupsProperty);

            serializedObject.ApplyModifiedProperties();
        }


        private void DrawAddGroup(SerializedProperty groupsProperty)
        {
            GUILayout.Space(2.5f);

            if (state == PrefabLoockupEditorState.AddingNewGroup)
            {
                GUILayout.Space(1.5f);
                GUILayout.BeginHorizontal();
                {
                    newGroupName = EditorGUILayout.TextField("New Group Name: ", newGroupName);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(1.5f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Group", GUILayout.ExpandWidth(true), GUILayout.Width(ButtonWidth)))
                {
                    if (state == PrefabLoockupEditorState.AddingNewGroup)
                    {
                        groupsProperty.InsertArrayElementAtIndex(groupsProperty.arraySize);
                        var lastGroupProperty = groupsProperty.GetArrayElementAtIndex(groupsProperty.arraySize - 1);
                        lastGroupProperty.FindPropertyRelative(Name).stringValue = newGroupName;

                        var pairsProperty = lastGroupProperty.FindPropertyRelative(Pairs);
                        pairsProperty.ClearArray();

                        var newGroupTableView = CreateGroupTableView(groupsProperty.arraySize - 1, newGroupName, pairsProperty);
                        ArrayUtility.Add(ref groupTableViews, newGroupTableView);
                    }

                    if (state == PrefabLoockupEditorState.AddingNewGroup)
                    {
                        state = PrefabLoockupEditorState.Editing;
                    }
                    else if (state == PrefabLoockupEditorState.Editing)
                    {
                        state = PrefabLoockupEditorState.AddingNewGroup;
                    }
                }

                if (state == PrefabLoockupEditorState.AddingNewGroup)
                {
                    if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(true), GUILayout.Width(ButtonWidth)))
                    {
                        state = PrefabLoockupEditorState.Editing;
                    }
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
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


        private TableView CreateGroupTableView(int index, string groupName, SerializedProperty pairsProperty)
        {
            System.Action<SerializedProperty> afterAddedCallback = (SerializedProperty property) =>
            {
                var id = index == 0 ? 1 : index * 1000;

                if (pairsProperty.arraySize > 1)
                {
                    for (var i = 0; i < pairsProperty.arraySize; i++)
                    {
                        int currentId = pairsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(PrefabId).intValue;
                        if (currentId >= id)
                        {
                            id = currentId + 1;
                        }
                    }
                }

                property.FindPropertyRelative(PrefabId).intValue = id;
                property.FindPropertyRelative(AssetPath).stringValue = string.Empty;
            };

            System.Action afterTableRemoveCallback = () =>
            {
                var groupsProperty = serializedObject.FindProperty(Groups);
                groupsProperty.DeleteArrayElementAtIndex(index);
                ArrayUtility.RemoveAt(ref groupTableViews, index);
            };

            return new TableView(serializedObject, pairsProperty, groupName,
                new TableColumn[]
                {
                    new TableColumn
                    {
                        Header = PrefabId.ToCamelCaseWithSpaces(),
                        WidthFactor = 0.5f,
                        FieldName = PrefabId
                    },
                    new TableColumn
                    {
                        Header = AssetPath.ToCamelCaseWithSpaces(),
                        WidthFactor = 0.5f,
                        FieldName = AssetPath
                    }
                },
                TableFoldoutMode.Foldout,
                TableRowDragableMode.Dragable,
                TableRemoveMode.WithRemove,
                afterAddedCallback,
                afterTableRemoveCallback);
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