using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Transform GetParent(this Transform Child, int Generation) {
        if (Generation < 0)
            Debug.LogError("Generation must be 0 or greater. Requested generation: " + Generation);

        Transform Recent = Child;

        for (int i = 0; i < Generation + 1; i++) {
            if (Recent.parent != null)
                Recent = Recent.parent;
            else
                return null;
        }

        return Recent;
    }

    public static T GetComponentInParent<T>(this Transform Child, int Generation) {
        if (Generation < 0)
            Debug.LogError("Generation must be 0 or greater. Requested generation: " + Generation);
            
        return Child.GetParent(Generation).GetComponent<T>();
    }

    public static T[] GetComponentsInParent<T>(this Transform Child, int Generation) {
        if (Generation < 0)
            Debug.LogError("Generation must be 0 or greater. Requested generation: " + Generation);

        return Child.GetParent(Generation).GetComponents<T>();
    }

    public static T GetComponentInParents<T>(this Transform Child) where T : class {
        int i = 0;
        while (true) {
            if (Child.GetParent(i) == null)
                return null;

            if (Child.GetParent(i).GetComponent<T>() != null)
                return Child.GetParent(i).GetComponent<T>();
        }
    }

    public static T[] GetComponentsInParents<T>(this Transform Child) where T : class {
        List<T> ToReturn = new List<T>();


        int i = 0;
        while (true) {
            if (Child.GetParent(i) == null)
                return ToReturn.ToArray();

            if (Child.GetParent(i).GetComponent<T>() != null)
                ToReturn.AddRange(Child.GetParent(i).GetComponents<T>());

            i++;
        }
    }




    public static T Random<T>(this List<T> List) {
        return List[UnityEngine.Random.Range(0, List.Count - 1)];
    }
}
