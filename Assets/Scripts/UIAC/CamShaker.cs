using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShaker : MonoBehaviour
{
    public static float shakeStrength;

    public static void Shake(float strength, float duration) {
        CamShakeController.singleton.Shake(strength, duration);
    }

    CinemachineBasicMultiChannelPerlin noise;

    void Start()
    {
        noise = GetComponent<Cinemachine.CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        noise.m_AmplitudeGain = shakeStrength;
    }
}
