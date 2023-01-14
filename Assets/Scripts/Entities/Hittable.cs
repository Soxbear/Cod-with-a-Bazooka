using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Hittable
{
    bool Hit(int Damage, Vector2 Direction, float Knockback);
}