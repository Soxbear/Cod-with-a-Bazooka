using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hittable
{
    float EntryPenetration(Vector2 position, HitType hitType) {
        return 10f;
    }

    float ContinuousPenetration(Vector2 position, HitType hitType) {
        return 0f;
    }

    bool Hit(int Damage, Vector2 Direction, float Knockback, HitType HitType);
}

public enum HitType {
    Bullet,
    Explosion,
    Melee,
    Sharapnel,
    Effect,
    Electric,
    Special
}