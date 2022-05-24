using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PdUtils
{
    public static class CollectionExtensions
    {
        public static bool ContainsName<T>(this List<T> list, string name) where T : Component
        {
            return list.GetByName(name) != null;
        }

        public static T GetByName<T>(this List<T> list, string name) where T : Component
        {
            foreach (var item in list)
                if (item.name == name)
                    return item;

            return null;
        }

        public static T GetWithNameContaining<T>(this T[] array, string name) where T : Object
        {
            foreach (var element in array)
                if (element.name.Contains(name))
                    return element;

            return null;
        }

        public static bool AddIfNotPresent<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
                return true;
            }

            return false;
        }

        public static bool Contains<T>(this List<T> list, string name) where T : Object
        {
            foreach (var element in list)
                if (element.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        public static T GetWithName<T>(this List<T> list, string name) where T : Object
        {
            foreach (var element in list)
                if (element.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return element;

            return null;
        }

        public static T GetWithName<T>(this T[] list, string name) where T : Object
        {
            foreach (var element in list)
                if (element.name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return element;

            return null;
        }

        public static T GetWithCaseInsensitiveNameOrNull<T>(this Dictionary<string, T> map, string name) where T : class
        {
            var invariantName = name.ToLowerInvariant();
            if (!map.ContainsKey(invariantName)) return null;

            return map[invariantName];
        }

        public static T GetFirstInactiveOrNull<T>(this List<T> list) where T : Component
        {
            foreach (var element in list)
                if (!element.gameObject.activeSelf)
                    return element;

            return null;
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static bool Contains(this string[] array, string element)
        {
            foreach (var arrayElement in array)
                if (string.Equals(arrayElement, element, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        public static bool ContainsIgnoreCase(this List<string> list, string element)
        {
            foreach (var arrayElement in list)
                if (string.Equals(arrayElement, element, StringComparison.InvariantCultureIgnoreCase))
                    return true;

            return false;
        }

        public static T[] AddAndReturn<T>(this T[] array, T[] addition)
        {
            var newArray = new T[array.Length + addition.Length];
            array.CopyTo(newArray, 0);
            addition.CopyTo(newArray, array.Length);
            return newArray;
        }
    }
}