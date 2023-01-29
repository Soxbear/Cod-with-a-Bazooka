using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IntertalReferenceUser
{
    List<Vector2> InertialReferences {
        get;
        set;
    }
}

public static class InertialReference {
    public static Vector2 GetTotalInertialReference(this IntertalReferenceUser User) {
        Vector2 Total = Vector2.zero;
        foreach (Vector2 IntertialReference in User.InertialReferences) {
            Total += IntertialReference;
        }
        return Total;
    }
}