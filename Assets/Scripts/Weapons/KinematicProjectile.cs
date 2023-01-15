using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicProjectile : MonoBehaviour
{
    public int Damage;

    public float Knockback;

    public float InitialSpeed;

    public bool Pierce;

    public float DestroyDelay;

    public float MaxLifetime;

    [Header("Layer Masks are only needed for pierce mode")]
    public LayerMask HitMask;
    public LayerMask LevelMask;


    Rigidbody2D Body;

    void Start() {
        Body = GetComponent<Rigidbody2D>();
        Body.AddRelativeForce(new Vector2(InitialSpeed, 0f), ForceMode2D.Impulse);
        Destroy(gameObject, MaxLifetime);
    }

    void OnTriggerEnter2D(Collider2D Hit) {
        if (Pierce) {
            if (HitMask == (HitMask | (1 << (int) Hit.gameObject.layer)))
                Hit.GetComponent<Hittable>().Hit(Damage, Body.velocity.normalized, Knockback, HitType.Bullet);

            if (LevelMask == (LevelMask | (1 << (int) Hit.gameObject.layer)))
                Destroy(gameObject, DestroyDelay);
        }
        else {
            Hittable Hittable;

            if (Hit.attachedRigidbody != null) {
                if (Hit.attachedRigidbody.TryGetComponent<Hittable>(out Hittable)) {
                    Hittable.Hit(Damage, Body.velocity.normalized /*Hit.transform.position - transform.position*/, Knockback, HitType.Bullet);
                }
            }

            Destroy(gameObject, DestroyDelay);
        }
    }
}
