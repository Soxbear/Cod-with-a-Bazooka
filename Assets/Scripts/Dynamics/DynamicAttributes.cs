using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class DynamicOut : Attribute {
    /*
    public string Name;
    public DynamicOut (string _Name) {
        Name = _Name;
    }
    */
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DynamicIn : Attribute {
    public string Name;
    public DynamicIn (string _Name) {
        Name = _Name;
    }
}

public static class GetDynamics {
    /*
    public static DynamicInInfo<T>[] GetInputs<T>() {
        
        List<DynamicInInfo<T>> Classes = new List<DynamicInInfo<T>>();

        foreach (MonoBehaviour In in MonoBehaviour.FindObjectsOfType<MonoBehaviour>().OfType<Dynamic<T>>()) {
            List<Action<T>> Functions = new List<Action<T>>();
            List<string> Names = new List<string>();

            foreach (MethodInfo Method in In.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public)) {
                DynamicIn Input = (DynamicIn) Attribute.GetCustomAttribute(Method, typeof(DynamicIn));

                if (Input != null && Method.GetParameters().Length == 1 && Method.GetParameters()[0].ParameterType == typeof(T)) {
                    Functions.Add((Action<T>) Method.CreateDelegate(typeof(Action<T>)));
                    Names.Add(Input.Name);
                }
            }

            if (Functions.Count > 0) {
                Classes.Add(new DynamicInInfo<T>(Functions, Names, In.gameObject));
            }
        }

        return Classes.ToArray();
    }
    */


}

/*
public class DynamicInInfo<T> {
    public List<Action<T>> Functions;
    public List<string> Names;

    public GameObject Parent;

    public DynamicInInfo(IEnumerable<Action<T>> _Functions, IEnumerable<string> _Names, GameObject _Parent) {
        Functions = new List<Action<T>>();
        Functions.AddRange(_Functions);
        Names = new List<string>();
        Names.AddRange(_Names);
        Parent = _Parent;
    }
}
*/

/*
public delegate void DynamicOutput<T>(T In);


#if UNITY_EDITOR
[CustomEditor(typeof(DynamicOutput<>))]
public class DynamicAttributes : Editor {
    bool Searching;
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        Type OutputType =  target.GetType().GenericTypeArguments[0];

        if (Searching) {
            if (GUILayout.Button("Cancel Connection")) {
                Searching = false;
                return;
            }
            //foreach (DynamicInInfo<OutputType> InInfo in GetDynamics.GetInputs<OutputType>()) {
                
            //}
        }
    }
}
#endif
*/