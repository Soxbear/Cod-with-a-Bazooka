using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static int AddReturnIndex<T>(this List<T> list, T obj) {
        list.Add(obj);
        return list.Count - 1;
    }
}
