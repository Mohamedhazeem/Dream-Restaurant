using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper 
{
    // For Array Assignment
    //public static T[] FromJson<T>(string json)
    //{
    //    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
    //    return wrapper.Items;
    //}
    public static List<T> ListFromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.ListItems;
    }

    //public static string ToJson<T>(T[] array)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.Items = array;
    //    return JsonUtility.ToJson(wrapper);
    //}

    //public static string ToJson<T>(T[] array, bool prettyPrint)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.Items = array;
    //    return JsonUtility.ToJson(wrapper, prettyPrint);
    //}
    public static string ToJson<T>(List<T> array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.ListItems = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    [System.Serializable]
    private class Wrapper<T>
    {
        //public T[] Items;
        public List<T> ListItems;
    }
}
