using System;
using System.Collections.Generic;

public static class ContainerTools {
    public static T At<T>(this T[] array, int index, T ifNotInRange = default) {
        if(array == null || index >= array.Length || index < 0) return ifNotInRange;
        return array[index];
    }

    public static T At<T>(this List<T> list, int index, T ifNotInRange = default) {
        if(list == null || index >= list.Count || index < 0) return ifNotInRange;
        return list[index];
    }

    public static V At<K, V>(this Dictionary<K, V> dict, K key, V ifAbsent = default) {
        if(dict == null || !dict.ContainsKey(key)) return ifAbsent;
        return dict[key];
    }

    public static T AtRandom<T>(this T[] array, T ifEmpty = default) {
        if(array == null || array.Length == 0) return ifEmpty;
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static T AtRandom<T>(this List<T> list, T ifEmpty = default) {
        if(list == null || list.Count == 0) return ifEmpty;
        return list[UnityEngine.Random.Range(0, list.Count)];
    }


    public static List<T> Filter<T>(this T[] array, Func<T, bool> func) {
        var result = new List<T>(array.Length);
        for(var i = 0; i < array.Length; i++) {
            if(func.Invoke(array[i])) {
                result.Add(array[i]);
            }
        }
        return result;
    }

    public static List<T> Filter<T>(this List<T> list, Func<T, bool> func) {
        var result = new List<T>(list.Count);
        for(var i = 0; i < list.Count; i++) {
            if(func.Invoke(list[i])) {
                result.Add(list[i]);
            }
        }
        return result;
    }

    public static List<R> Map<T, R>(this List<T> list, Func<T, R> func) {
        var result = new List<R>(list.Count);
        for(var i = 0; i < list.Count; i++) {
            result.Add( func.Invoke(list[i]) );
        }
        return result;
    }

    public static List<R> Map<T, R>(this List<T> list, Func<T, int, R> func) {
        var result = new List<R>(list.Count);
        for(var i = 0; i < list.Count; i++) {
            result.Add( func.Invoke(list[i], i) );
        }
        return result;
    }

    public static List<R> Map<T, R>(this T[] array, Func<T, R> func) {
        var result = new List<R>(array.Length);
        for(var i = 0; i < array.Length; i++) {
            result.Add(func.Invoke(array[i]));
        }
        return result;
    }

    public static List<R> Map<T, R>(this T[] array, Func<T, int, R> func) {
        var result = new List<R>(array.Length);
        for(var i = 0; i < array.Length; i++) {
            result.Add(func.Invoke(array[i], i));
        }
        return result;
    }

    public static List<R> ToList<T, R>(this Dictionary<T, R> dictionary) {
        var result = new List<R>(dictionary.Count);

        foreach (var value in dictionary)
            result.Add(value.Value);

        return result;
    }

    public static R Fold<T, R>(this List<T> list, Func<R, T, R> func, R acc) {
        for(var i = 0; i < list.Count; i++) {
            acc = func.Invoke(acc, list[i]);
        }
        return acc;
    }

    public static T Fold<T>(this List<T> list, Func<T, T, T> func) {
        var acc = list.At(0);
        for(var i = 0; i < list.Count; i++) {
            acc = func.Invoke(acc, list[i]);
        }
        return acc;
    }

    public static R Fold<T, R>(this T[] array, Func<R, T, R> func, R acc) {
        for(var i = 0; i < array.Length; i++) {
            acc = func.Invoke(acc, array[i]);
        }
        return acc;
    }

    public static T Fold<T>(this T[] array, Func<T, T, T> func) {
        var acc = array.At(0);
        for(var i = 0; i < array.Length; i++) {
            acc = func.Invoke(acc, array[i]);
        }
        return acc;
    }

    public static int Sum<T>(this List<T> list, Func<T, int> func, int acc = 0) {
        for(var i = 0; i < list.Count; i++) {
            acc += func.Invoke(list[i]);
        }
        return acc;
    }

    public static T MinValue<T>(this List<T> list, Func<T, float> func, T ifEmpty = default) {
        if(list.IsNullOrEmpty()) return ifEmpty;

        var minValue = float.MaxValue;
        var idx = -1;
        for(var i = 0; i < list.Count; i++) {
            var v = func.Invoke(list[i]);
            if(v <= minValue) {
                minValue = v;
                idx = i;
            }
        }
        return list[idx];
    }

    public static T MinValue<T>(this T[] array, Func<T, float> func, T ifEmpty = default) {
        if(array.IsNullOrEmpty()) return ifEmpty;

        var minValue = float.MaxValue;
        var idx = -1;
        for(var i = 0; i < array.Length; i++) {
            var v = func.Invoke(array[i]);
            if(v <= minValue) {
                minValue = v;
                idx = i;
            }
        }
        return array[idx];
    }

    public static T MaxValue<T>(this List<T> list, Func<T, float> func, T ifEmpty = default) {
        if(list.IsNullOrEmpty()) return ifEmpty;

        var maxValue = float.MinValue;
        var idx = -1;
        for(var i = 0; i < list.Count; i++) {
            var v = func.Invoke(list[i]);
            if(v >= maxValue) {
                maxValue = v;
                idx = i;
            }
        }
        return list[idx];
    }

    public static T MaxValue<T>(this T[] array, Func<T, float> func, T ifEmpty = default) {
        if(array.IsNullOrEmpty()) return ifEmpty;

        var maxValue = float.MinValue;
        var idx = -1;
        for(var i = 0; i < array.Length; i++) {
            var v = func.Invoke(array[i]);
            if(v >= maxValue) {
                maxValue = v;
                idx = i;
            }
        }
        return array[idx];
    }

    public static int Count<T>(this T[] array, Func<T, bool> func) {
        var count = 0;
        for(var i = 0; i < array.Length; i++) {
            count += func.Invoke(array[i]) ? 1 : 0;
        }
        return count;
    }

    public static int Count<T>(this List<T> list, Func<T, bool> func) {
        var count = 0;
        for(var i = 0; i < list.Count; i++) {
            count += func.Invoke(list[i]) ? 1 : 0;
        }
        return count;
    }

    public static bool Any<T>(this T[] array, Func<T, bool> func) {
        for(var i = 0; i < array.Length; i++) {
            if(func.Invoke(array[i])) return true;
        }
        return false;
    }

    public static bool Any<T>(this List<T> list, Func<T, bool> func) {
        for(var i = 0; i < list.Count; i++) {
            if(func.Invoke(list[i])) return true;
        }
        return false;
    }

    public static bool All<T>(this List<T> list, Func<T, bool> func) {
        for(var i = 0; i < list.Count; i++) {
            if(!func.Invoke(list[i])) return false;
        }
        return true;
    }

    public static bool All<T>(this T[] array, Func<T, bool> func) {
        for(var i = 0; i < array.Length; i++) {
            if(!func.Invoke(array[i])) return false;
        }
        return true;
    }

    public static int IndexOf<T>(this List<T> list, Func<T, bool> func, int ifAbsent = -1) {
        for(var i = 0; i < list.Count; i++) {
            if(func.Invoke(list[i])) return i;
        }
        return ifAbsent;
    }

    public static int IndexOf<T>(this T[] array, Func<T, bool> func, int ifAbsent = -1) {
        for(var i = 0; i < array.Length; i++) {
            if(func.Invoke(array[i])) return i;
        }
        return ifAbsent;
    }

    public static T Last<T>(this List<T> list, T ifEmpty = default) {
        if(list.Count == 0) return ifEmpty;
        return list[list.Count - 1];
    }
    
    public static bool IsNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dic) {
        return dic == null || dic.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list) {
        return list == null || list.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this T[] list) {
        return list == null || list.Length == 0;
    }

    public static bool Contains<T>(this T[] array, T item) {
        for(var i = 0; i < array.Length; i++) {
            if(array[i].Equals(item)) return true;
        }
        return false;
    }

    public static T Find<T>(this T[] array, Func<T, bool> func, T ifAbsent = default) {
        for(var i = 0; i < array.Length; i++) {
            if(func.Invoke(array[i])) return array[i];
        }
        return ifAbsent;
    }
    
    public static List<R> FindAll<T,R>(this Dictionary<T,R> dictionary, Func<KeyValuePair<T,R>, bool> func) {
        var result = new List<R>();
        foreach (var value in dictionary)
        {
            if (func(value))
            {
                result.Add(value.Value);
            }
        }
        return result;
    }

    public static void Remove<T>(this List<T> list, Predicate<T> func)
    {
        list.Remove(list.Find(func));
    }

    public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
        TKey fromKey, TKey toKey)
    {
        TValue value = dic[fromKey];
        dic.Remove(fromKey);
        dic[toKey] = value;
    }

    public static void ForEach<T>(this T[] array, Action<T> action) {
        for(var i = 0; i < array.Length; i++) {
            action.Invoke(array[i]);
        }
    }

    public static void ForEach<T>(this T[] array, Action<T, int> action) {
        for(var i = 0; i < array.Length; i++) {
            action.Invoke(array[i], i);
        }
    }

    public static void ForEach<T>(this List<T> list, Action<T> action) {
        for(var i = 0; i < list.Count; i++) {
            action.Invoke(list[i]);
        }
    }

    public static void ForEach<T>(this List<T> list, Action<T, int> action) {
        for(var i = 0; i < list.Count; i++) {
            action.Invoke(list[i], i);
        }
    }

    public static T Pop<T>(this List<T> list, T ifEmpty = default) {
        if(list.Count == 0) return ifEmpty;
        var result = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return result;
    }

    public static T AddNotNull<T>(this List<T> list, T value) where T : class {
        if(value == null) return null;
        list.Add(value);
        return value;
    }

    public static T FindInRange<T>(this List<T> list, int startIndex, int endIndex, Predicate<T> predicate, T ifAbsent = default)
    {
        if (startIndex <= endIndex)
        {
            for (var i = startIndex; i <= endIndex; i++)
            {
                if (predicate.Invoke(list[i])) return list[i];
            }
        }
        else
        {
            for (var i = startIndex; i >= endIndex; i--)
            {
                if (predicate.Invoke(list[i])) return list[i];
            }
        }

        return ifAbsent;
    }
}
