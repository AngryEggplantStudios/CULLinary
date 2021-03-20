using UnityEngine;
using System;
public static class JsonArrayParser
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper;
        try
        {
            wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        }
        catch
        {
            Debug.Log("Unable to parse");
            wrapper = new Wrapper<T>();
        }
        if (wrapper == null)
        {
            return Array.Empty<T>();
        }
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    
    [Serializable] private class Wrapper<T>
    {
        public T[] Items;
    }
}