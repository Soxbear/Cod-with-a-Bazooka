using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif

public class MouseCentralizer : InputProcessor<Vector2>
{
    Transform Player;

    public override Vector2 Process(Vector2 MousePosition, InputControl Controls)
    {
        return MousePosition - (new Vector2(Screen.width, Screen.height) / 2);
    }

    #if UNITY_EDITOR
    static MouseCentralizer() {
        Initialize();
    }
    #endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() {
        InputSystem.RegisterProcessor<MouseCentralizer>();
        //Player = MonoBehaviour.FindObjectOfType<Player>().transform;
    }
}
