using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Distance;
    public float Force;
    public int Damage;

    public LayerMask KnockbackMask;

    public LayerMask ProtectionMask;

    public LayerMask DamageMask;

    void Start()
    {
        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, Distance, KnockbackMask);

        foreach (Collider2D Hit in Hits) {
            Vector2 Relative = Hit.transform.position - transform.position;
            RaycastHit2D SightHit = Physics2D.Raycast((Vector2)transform.position + Relative.normalized * 0.3f, Relative, Relative.magnitude, ProtectionMask);
            if (SightHit.collider == null) {
                Hit.attachedRigidbody.AddForce(Relative.normalized * Mathf.Min(((Relative.magnitude - Distance) * ( -1 / (Distance -1))), 1) * Force, ForceMode2D.Impulse);
                //Debug.Log((Relative.normalized * Mathf.Max(0, Distance - Relative.magnitude)) * Force);
                
                Hittable Hittable;

                if (Hit.attachedRigidbody.TryGetComponent<Hittable>(out Hittable) && (DamageMask == ( DamageMask | 1 << Hit.attachedRigidbody.gameObject.layer))) {
                    Hittable.Hit(Mathf.RoundToInt(Damage * Mathf.Min(((Relative.magnitude - Distance) * ( -1 / (Distance -1))), 1)), Relative.normalized, 0, HitType.Explosion);
                }

                //Debug.Log(Hittable);
            }
        }
    }
}
