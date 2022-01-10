#if UNITY_EDITOR

namespace Library.Editor
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    public enum TableFoldoutMode
    {
        None,
        Foldout,
        NotFoldout
    }

    public enum TableRowDragableMode
    {
        None,
        Dragable,
        NotDragable
    }

    public enum TableRemoveMode
    {
        None,
        WithRemove,
        WithoutRemove
    }

    public class TableColumn
    {
        public string Header;
        public float WidthFactor;
        public string FieldName;
        public System.Action<Rect, SerializedProperty> CustomDrawCallback;
    }

    public class TableView
    {
        private const float HeaderLeftOffset = 14.0f;
        private const float ColumnOffset = 8.0f;

        private ReorderableList list = null;

        private SerializedObject serializedObject = null;
        private SerializedProperty enumarableProperty = null;
        private string title = string.Empty;
        private TableColumn[] columns = null;
        private bool isVisible = false;
        private TableFoldoutMode foldoutMode = TableFoldoutMode.None;
        private TableRowDragableMode dragableMode = TableRowDragableMode.None;
        private TableRemoveMode removeMode = TableRemoveMode.None;
        private System.Action<SerializedProperty> afterAddedCallback = null;
        private System.Action afterTableRemoveCallback = null;

        public TableView(SerializedObject serializedObject, SerializedProperty enumarableProperty,
                         string title, TableColumn[] columns,
                         TableFoldoutMode foldoutMode, TableRowDragableMode dragableMode,
                         TableRemoveMode removeMode,
                         System.Action<SerializedProperty> afterAddedCallback,
                         System.Action afterTableRemoveCallback)
        {
            if (removeMode == TableRemoveMode.WithRemove && afterTableRemoveCallback == null)
            {
                throw new UnityException("Wrong configuration. afterTableRemoveCallback must be set is removeMode == TableRemoveMode.WithRemove");
            }

            this.serializedObject = serializedObject;
            this.enumarableProperty = enumarableProperty;
            this.title = title;
            this.columns = columns;
            this.foldoutMode = foldoutMode;
            this.dragableMode = dragableMode;
            this.removeMode = removeMode;
            this.afterAddedCallback = afterAddedCallback;
            this.afterTableRemoveCallback = afterTableRemoveCallback;

            list = new ReorderableList(this.serializedObject, this.enumarableProperty, IsDragable(), true, true, true);

            InitializeDrawHeaderCallback();
            InitializeDrawElementCallback();
            InitializeAddElementCallback();
        }

        public void Draw()
        {
            EditorGUILayout.Space();
            DrawTitle();

            if (isVisible)
            {
                list.DoLayoutList();
            }
        }

        void InitializeDrawHeaderCallback()
        {
            list.drawHeaderCallback = (Rect rect) =>
            {
                float leftOffset = dragableMode == TableRowDragableMode.Dragable ? HeaderLeftOffset : 0.0f;
                float currentOffsetX = leftOffset;
                float availableWidth = rect.width - leftOffset;
                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];

                    var x = rect.x + currentOffsetX;
                    var width = availableWidth * columns[i].WidthFactor;
                    var columnHeaderRect = new Rect(x, rect.y, width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.LabelField(columnHeaderRect, column.Header);

                    currentOffsetX += width;
                }
            };
        }

        private void InitializeDrawElementCallback()
        {
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                float currentOffsetX = 0.0f;
                float availableWidth = rect.width - ColumnOffset * columns.Length;
                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];

                    var x = rect.x + currentOffsetX;
                    var width = availableWidth * columns[i].WidthFactor;
                    var propertyRect = new Rect(x, rect.y, width, EditorGUIUtility.singleLineHeight);
                    if (column.CustomDrawCallback == null)
                    {
                        var property = element.FindPropertyRelative(column.FieldName);
                        EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
                    }
                    else
                    {
                        column.CustomDrawCallback(propertyRect, element);
                    }

                    currentOffsetX += width + ColumnOffset;
                }
            };
        }

        private void InitializeAddElementCallback()
        {
            list.onAddCallback = (ReorderableList list) =>
            {
                enumarableProperty.InsertArrayElementAtIndex(enumarableProperty.arraySize);
                if (afterAddedCallback != null)
                {
                    var lastElementProperty = enumarableProperty.GetArrayElementAtIndex(enumarableProperty.arraySize - 1);
                    afterAddedCallback(lastElementProperty);
                }
            };
        }


        private void DrawTitle()
        {
            GUILayout.BeginHorizontal();
            {
                if (foldoutMode == TableFoldoutMode.Foldout)
                {
                    isVisible = EditorGUILayout.Foldout(isVisible, title);
                }
                else
                {
                    isVisible = true;
                    EditorGUILayout.LabelField(title);
                }

                if (removeMode == TableRemoveMode.WithRemove)
                {
                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        afterTableRemoveCallback();
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        private bool IsDragable()
        {
            return dragableMode == TableRowDragableMode.Dragable;
        }
    }
}

#endif
