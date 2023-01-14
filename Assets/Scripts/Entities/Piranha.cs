using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piranha : Enemy, ExternalTriggerStay2DUser
{
    [Header("Settings")]
    public int Damage;

    [Header("References")]
    Animator BodyAnimator;
    Animator FaceAnimator;

    public void ExternalTriggerStay2D(Collider2D Col, int Identifier) {
        if (RequestAttack()) {
            Hittable Hittable = Col.GetComponentInParent<Hittable>();
            if (Hittable!= null)
                Hittable.Hit(Damage, (Vector2) (transform.position - Col.transform.position), 0.25f);
        }
    }

    void Start()
    {
        BodyAnimator = transform.GetChild(1).GetComponent<Animator>();
        FaceAnimator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect((Vector2) FindObjectOfType<Player>().GetComponent<Transform>().position);
        BodyAnimator.SetFloat("Speed", Body.velocity.magnitude);
    }
}
