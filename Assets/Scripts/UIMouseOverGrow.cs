using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseOverGrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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

        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
