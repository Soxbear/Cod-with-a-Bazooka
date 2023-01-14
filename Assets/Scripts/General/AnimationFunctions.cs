using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    Animator Animator;

    public void Start() {
        Animator = GetComponent<Animator>();
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    public void DisableAnim() {
        Animator.enabled = false;
    }
}
