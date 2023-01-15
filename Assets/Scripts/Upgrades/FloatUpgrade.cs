using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Float Upgrade", menuName = "Upgrades/Float Upgrade", order = 1)]
public class FloatUpgrade : Upgrade {
    [Header("Float Upgrade Locations \nSpeed, Fire Rate = 0 \nAcceleration = 1 \nDamage = 2")]
    public List<float> Upgrades;
}
public enum DefaultFloatUpgrade {
    Speed = 0, FireRate = 0,
    Acceleration = 1,
    Damage = 2
}