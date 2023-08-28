using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseOverGrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 startScale;

    public float ScaleChange;

    public float Time;

    float Target;
    float Actual;

    public void OnPointerEnter(PointerEventData e) {
        Target = Time;
    }

    public void OnPointerExit(PointerEventData e) {
        Target = 0;
    }

    void Update() {
        if (Actual == Target)
            return;

        Actual = Mathf.MoveTowards(Actual, Target, UnityEngine.Time.unscaledDeltaTime);

        float Scale = 1 + ScaleChange * DynamicUtil.EvaluateCurve(DynamicUtil.EvalCurve.Sine, Actual / Time);

        transform.localScale = startScale * Scale;
    }

    void Start() {
        startScale = transform.localScale;
    }
}
