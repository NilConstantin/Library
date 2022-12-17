using UnityEngine;
using System;


namespace Library
{
    public class DropdownArrayAttribute : PropertyAttribute
    {
        public string PropertyName;
        public Type EnumType;

        public DropdownArrayAttribute(string propertyName, Type enumType)
        {
            PropertyName = propertyName;
            EnumType = enumType;
        }
    }
}