using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlastDoor : MonoBehaviour, Dynamic<bool>
{
    Transform UpperDoor;
    Transform LowerDoor;
    SpriteRenderer UpperIndicator;
    SpriteRenderer LowerIndicator;

    public bool IsOpen {
        get {
            return Open;
        }
    }

    bool Open;

    public float MoveTime;

    void Start() {
        UpperDoor = transform.GetChild(0);
        LowerDoor = transform.GetChild(1);
        UpperIndicator = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        LowerIndicator = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void SetOpenState(bool State) {
        if (State && !Open) {
            StartCoroutine(DynamicUtil.ObjectMove(UpperDoor, new Vector3(0f, 2.5f, 0f), MoveTime));
            StartCoroutine(DynamicUtil.ObjectMove(LowerDoor, new Vector3(0f, -2.5f, 0f), MoveTime));
            UpperIndicator.color = Color.green;
            LowerIndicator.color = Color.green;
            Open = true;
        } else if (!State && Open) {            
            StartCoroutine(DynamicUtil.ObjectMove(UpperDoor, new Vector3(0f, -2.5f, 0f), MoveTime));
            StartCoroutine(DynamicUtil.ObjectMove(LowerDoor, new Vector3(0f, 2.5f, 0f), MoveTime));
            UpperIndicator.color = Color.red;
            LowerIndicator.color = Color.red;
            Open = false;
        }
    }

    public void ToggleState() {
        SetOpenState(!Open);
    }
}
