using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable : MonoBehaviour
{
    private bool Tgl;

    public bool Toggled {
        get {
            return Tgl;
        }
    }    

    public bool Toggle() {
        Tgl = !Toggled;

        if (Tgl) {
            BroadcastMessage("OnToggleTrue", SendMessageOptions.DontRequireReceiver);
            BroadcastMessage("OnTrue", SendMessageOptions.DontRequireReceiver);
        }
        else {
            BroadcastMessage("OnToggleFalse", SendMessageOptions.DontRequireReceiver);
            BroadcastMessage("OnFalse", SendMessageOptions.DontRequireReceiver);
        }

        return Toggled;
    }

    public void Toggle(bool State) {
        bool Prev = Tgl;

        Tgl = State;

        if (Tgl) {
            BroadcastMessage("OnTrue", SendMessageOptions.DontRequireReceiver);
            if (Prev != Tgl)
                BroadcastMessage("OnToggleTrue", SendMessageOptions.DontRequireReceiver);
        } else {
            BroadcastMessage("OnFalse", SendMessageOptions.DontRequireReceiver);
            if (Prev != Tgl)
                BroadcastMessage("OnToggleFalse", SendMessageOptions.DontRequireReceiver);
        }
    }
}
