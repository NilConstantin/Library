using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;


namespace Library.Editor
{
    [CustomPropertyDrawer(typeof(EnumAttribute))]
    public class EnumAttributeDrawer : PropertyDrawer
    {
        private EnumAttribute enumAttribute => ((EnumAttribute)attribute);

        private int index;
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var list = enumAttribute.ListForEnum;

            if (list != null)
            {
                EditorGUI.BeginChangeCheck();

                index = -1;

                var contents = new List<string>(list.Length);
                foreach (var item in list)
                {
                    if (GetValue(property) == item.Value)
                    {
                        index = contents.Count;
                    }

                    var prefix = string.IsNullOrEmpty(item.GroupName) ? string.Empty : $"{item.GroupName} / ";

                    contents.Add($"{prefix}{item.Key} - {item.Value}\t");
                }

                index = EditorGUI.Popup(position, label.text, index, contents.ToArray());

                if (EditorGUI.EndChangeCheck() && index != -1)
                {
                    SetValue(property, list[index].Value);
                }
            }
        }


        private static string GetValue(SerializedProperty property)
        {
            if (property.type == "int")
            {
                return property.intValue.ToString();
            }
            else if (property.type == "float")
            {
                return property.floatValue.ToString(CultureInfo.InvariantCulture);
            }
            return property.stringValue;
        }


        private static void SetValue(SerializedProperty property, string value)
        {
            if (property.type == "int")
            {
                property.intValue = int.Parse(value);
            }
            else if (property.type == "float")
            {
                property.floatValue = float.Parse(value);
            }
            else if (property.type == "string")
            {
                property.stringValue = value;
            }
        }
    }
}