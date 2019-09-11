using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov
{
    static class ReflectionMisc
    {
        public static string GetTypeName(Type type)
        {
            string name;
            if (Attribute.IsDefined(type, typeof(NameAttribute)))
            {
                name = ((NameAttribute)Attribute.GetCustomAttribute(type, typeof(NameAttribute))).Name;
            }
            else
            {
                name = type.Name;
            }
            return name;
        }
        public static string GetPropertyName(PropertyInfo info)
        {
            string name;
            if (Attribute.IsDefined(info, typeof(NameAttribute)))
            {
                name = ((NameAttribute)info.GetCustomAttribute(typeof(NameAttribute))).Name;
            }
            else
            {
                name = info.Name;
            }
            return name;
        }

        public static int GetPropertyDepth(PropertyInfo info, Type type)
        {
            bool declared = type.Equals(info.DeclaringType);
            if (!declared)
            {
                return GetPropertyDepth(info, type.BaseType) + 1;
            }
            else
            {
                return 0;
            }
        }
       
    }
}
