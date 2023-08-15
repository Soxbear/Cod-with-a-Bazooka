using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManager
{
    public static HealthUIController healthUI;

    public static ResourceUIController resourceUI;
}

// public abstract class UIController : MonoBehaviour {
//     public bool show = true;

//     CanvasGroup group;

//     float currentAlpha;

//     float targetAlpha;

//     float rateAlpha;

//     public void Enable(float fadeTime = 0.25f) {
//         group.blocksRaycasts = true;
//         targetAlpha = 1;
//         rateAlpha = 1 / fadeTime;
//         show = true;
//     }

//     public void Disable(float fadeTime = 0.25f) {
//         group.blocksRaycasts = false;
//         targetAlpha = 0;
//         rateAlpha = 1 / fadeTime;
//         show = false;
//     }

//     void Update() {
//         currentAlpha += Mathf.Clamp(targetAlpha - currentAlpha, -rateAlpha * Time.deltaTime, rateAlpha * Time.deltaTime);
//         group.alpha = currentAlpha;
//     }

//     void OnEnable() {
//         if (GetComponent<CanvasGroup>() == null)
//             group = gameObject.AddComponent<CanvasGroup>();
//         else
//             group = GetComponent<CanvasGroup>();

//         if (show) {
//             targetAlpha = 1;
//             currentAlpha = 1;
//             rateAlpha = 4;
//             group.alpha = 1;
//         }
//         else {
//             targetAlpha = 0;
//             currentAlpha = 0;
//             rateAlpha = 4;
//             group.alpha = 0;
//         }
//     }
// }