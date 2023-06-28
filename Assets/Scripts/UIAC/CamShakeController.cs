using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShakeController : MonoBehaviour
{
    
    public static CamShakeController singleton;

    void Start()
    {
        singleton = this;
    }

    public void Shake(float strength, float duration) {
        StartCoroutine(ShakeTimer(strength, duration));
    }    

    IEnumerator ShakeTimer(float strength, float duration) {
        CamShaker.shakeStrength += strength;
        yield return new WaitForSeconds(duration);
        CamShaker.shakeStrength -= strength;
    }
}
