using System;
using System.Reflection;
namespace GoodsHandbookMalchikovPavlov
{
    internal static class Misc
    {
        private static void Merge(int[] arr, int p, int q, int r, Array satelliteData)
        {
            int n1 = q - p + 1;
            int n2 = r - q;
            int[] lArr = new int[n1 + 1];
            int[] rArr = new int[n2 + 1];
            Array lSatArr = Array.CreateInstance(satelliteData.GetValue(0).GetType(), n1);
            Array rSatArr = Array.CreateInstance(satelliteData.GetValue(0).GetType(), n2);
            int i = 0, j = 0;
            for (; i < n1; i++)
            {
                lArr[i] = arr[p + i];
                lSatArr.SetValue(satelliteData.GetValue(p + i), i);
            }
            for (; j < n2; j++)
            {
                rArr[j] = arr[q + 1 + j];
                rSatArr.SetValue(satelliteData.GetValue(q + 1 + j), j);


            }
            lArr[n1] = int.MaxValue;
            rArr[n2] = int.MaxValue;
            i = 0;
            j = 0;
            for (int k = p; k <= r; k++)
            {
                if (lArr[i] <= rArr[j])
                {
                    arr[k] = lArr[i];
                    satelliteData.SetValue(lSatArr.GetValue(i), k);
                    i++;
                }
                else
                {
                    arr[k] = rArr[j];
                    satelliteData.SetValue(rSatArr.GetValue(j), k);
                    j++;
                }
            }
        }
        public static void MergeSort(int[] arr, int p, int r, Array satelliteData)
        {
            if (p < r)
            {
                int q = (p + r) / 2;
                MergeSort(arr, p, q, satelliteData);
                MergeSort(arr, q + 1, r, satelliteData);
                Merge(arr, p, q, r, satelliteData);
            }
        }
        public static int FindString(string str, string[] strings)
        {
            for (int i = 0; i < strings.Length; i++)
            {
                if (strings[i].Equals(str))
                {
                    return i;
                }
            }
            return -1;
        }
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
        public static void SortProperties(PropertyInfo[] properties, Type type)
        {
            int[] depths = new int[properties.Length];
            for (int i = 0; i < depths.Length; i++)
            {
                depths[i] = -GetPropertyDepth(properties[i], type);
            }
            Misc.MergeSort(depths, 0, properties.Length - 1, properties);
            return;
        }
    }
}
