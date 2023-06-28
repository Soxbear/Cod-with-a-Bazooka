using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using Soxbear.Channels;

public class BlastDoor : MonoBehaviour
{
    public bool IsOpen {
        get {
            return Open;
        }
    }

    bool Open;


    Transform UpperDoor;
    Transform LowerDoor;
    SpriteRenderer UpperIndicator;
    SpriteRenderer LowerIndicator;
    Light2D IndicatorLight1;
    Light2D IndicatorLight2;
    Light2D IndicatorLight3;
    Light2D IndicatorLight4;    

    bool IsMoving;

    public float MoveTime;

    public ChannelReadWrite<bool> setState;
    public ChannelReadWrite<bool> toggleState;

    void Start() {
        UpperDoor = transform.GetChild(0);
        LowerDoor = transform.GetChild(1);
        UpperIndicator = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        LowerIndicator = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        IndicatorLight1 = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Light2D>();
        IndicatorLight2 = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Light2D>();
        IndicatorLight3 = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Light2D>();
        IndicatorLight4 = transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Light2D>();

        setState.onUpdate += (value) => {
            Move(value);
        };
        toggleState.onUpdate += (value) => {
            if (value)
                Move(!Open);
        };
    }

    void Move(bool State) {
        if (IsMoving)
            return;

        if (State && !Open) {
            StartCoroutine(DynamicUtil.ObjectMove(UpperDoor, transform.up * 2.5f, MoveTime));
            StartCoroutine(DynamicUtil.ObjectMove(LowerDoor, transform.up * -2.5f, MoveTime));
            UpperIndicator.color = Color.green;
            LowerIndicator.color = Color.green;
            IndicatorLight1.color = Color.green;
            IndicatorLight2.color = Color.green;
            IndicatorLight3.color = Color.green;
            IndicatorLight4.color = Color.green;
            Open = true;
        } else if (!State && Open) {            
            StartCoroutine(DynamicUtil.ObjectMove(UpperDoor, transform.up * -2.5f, MoveTime));
            StartCoroutine(DynamicUtil.ObjectMove(LowerDoor, transform.up * 2.5f, MoveTime));
            UpperIndicator.color = Color.red;
            LowerIndicator.color = Color.red;
            IndicatorLight1.color = Color.red;
            IndicatorLight2.color = Color.red;
            IndicatorLight3.color = Color.red;
            IndicatorLight4.color = Color.red;
            IndicatorLight1.intensity = 1;
            IndicatorLight2.intensity = 1;
            IndicatorLight3.intensity = 1;
            IndicatorLight4.intensity = 1;
            Open = false;
        }
        StartCoroutine(IsMovingChange());
    }

    IEnumerator IsMovingChange() {
        IsMoving = true;
        yield return new WaitForSeconds(MoveTime);
        IsMoving = false;
        if (Open) {
            IndicatorLight1.intensity = 0;
            IndicatorLight2.intensity = 0;
            IndicatorLight3.intensity = 0;
            IndicatorLight4.intensity = 0;
        }
    }
}