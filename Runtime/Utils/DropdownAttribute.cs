using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;


namespace Library
{
    public class DropdownAttribute : PropertyAttribute
    {
        private DropdownItem[] listForEnum;

        private readonly Type listSourceClass;
        private readonly string listSourceMethod;


        public DropdownItem[] ListForEnum
        {
            get
            {
                if (listSourceClass != null)
                {
                    if (!string.IsNullOrEmpty(listSourceMethod))
                    {
                        UpdateMethod();
                    }
                    else
                    {
                        UpdateFields();
                    }
                }

                return listForEnum;
            }
        }


        DropdownAttribute()
        {
        }


        public DropdownAttribute(Type sourceClassConstFields)
        {
            if (sourceClassConstFields != null)
            {
                listSourceClass = sourceClassConstFields;
                listSourceMethod = null;

                UpdateFields();
            }
        }


        public DropdownAttribute(Type sourceClassList, string sourceMethodList)
        {
            if (sourceClassList != null)
            {
                listSourceClass = sourceClassList;

                if (!string.IsNullOrEmpty(sourceMethodList))
                {
                    listSourceMethod = sourceMethodList;
                    UpdateMethod();
                }
                else
                {
                    listSourceMethod = null;
                }
            }
        }


        // public EnumAttribute(params object[] list)
        // {
        //     if (list.Length > 0)
        //     {
        //         listForEnum = new string[list.Length];
        //         for (int i = 0; i < list.Length; i++)
        //         {
        //             listForEnum[i] = list[i].ToString();
        //         }
        //     }
        // }
        //
        //
        // public EnumAttribute(string[] list)
        // {
        //     if (list.Length > 0)
        //     {
        //         listForEnum = new string[list.Length];
        //         for (int i = 0; i < list.Length; i++)
        //         {
        //             listForEnum[i] = list[i];
        //         }
        //     }
        // }


        void UpdateMethod()
        {
            if (listSourceClass.GetMethod(listSourceMethod)?.Invoke(null, null) is List<DropdownItem> list)
            {
                listForEnum = list.ToArray();
            }
        }


        void UpdateFields()
        {
            var list = new List<DropdownItem>();
            foreach (FieldInfo field in listSourceClass.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    var fieldValue = field.GetRawConstantValue();
                    var valuesGroup = field.GetCustomAttribute<ValuesGroupAttribute>();
                    list.Add(new DropdownItem(field.Name, fieldValue.ToString(), valuesGroup?.GroupName ?? string.Empty));
                }
            }
            listForEnum = list.ToArray();
        }
    }
}