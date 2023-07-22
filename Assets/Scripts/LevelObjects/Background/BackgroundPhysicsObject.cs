using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPhysicsObject : MonoBehaviour, Hittable
{
    Rigidbody2D body;

    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    float EntryPenetration(Vector2 position, HitType hitType) {
        return 0f;
    }

    public bool Hit(int damage, Vector2 direction, float knockback, HitType hitType) {
        if (hitType == HitType.Bullet || hitType == HitType.Effect || hitType == HitType.Electric || hitType == HitType.Melee)
            return false;

        body.AddForce(direction * knockback);
        
        return false;
    }
}