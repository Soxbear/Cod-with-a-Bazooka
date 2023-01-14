using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DynamicUtil
{
    public static IEnumerator ObjectMove(Transform Object, Vector3 RelativeMotion, float MoveTime, EvalCurve Curve = EvalCurve.Sine) {
        if (MoveTime < 0) {
            Debug.LogError("Time cannot be negative. Recieved value: " + MoveTime);
            MoveTime = 0;
        }

        Vector3 StartPosition = Object.position;

        float TimeElapsed = 0f;

        bool Finished = false;

        while (!Finished) {
            yield return null;

            TimeElapsed = Mathf.Clamp(TimeElapsed + Time.deltaTime, 0, MoveTime);

            Object.position = StartPosition + (RelativeMotion * EvaluateCurve(Curve, TimeElapsed / MoveTime));

            if (TimeElapsed == MoveTime)
                Finished = true;
        }
    }


    public enum EvalCurve {
        None,
        Sine,
        Tanget
    }

    public static float EvaluateCurve(EvalCurve Curve, float x) {
        if (x < 0 || x > 1)
            Debug.LogWarning("x should be between 0 and 1. Recieved value: " + x);

        x = Mathf.Clamp01(x);

        switch (Curve) {
            default:
                return x;

            case EvalCurve.Sine:
                return Mathf.Sin(x * Mathf.PI * 0.5f);

            case EvalCurve.Tanget:
                return Mathf.Tan((x * 0.999f) * Mathf.PI) / Mathf.Tan(0.001f * Mathf.PI);
        }
    }
}
