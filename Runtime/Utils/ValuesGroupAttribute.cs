using System;
using UnityEngine;


namespace Library
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValuesGroupAttribute : PropertyAttribute
    {
        public readonly string GroupName;


        public ValuesGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}