using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Int Upgrade", menuName = "Upgrades/Int Upgrade", order = 3)]
public class IntUpgrade : Upgrade {
    [Header("Int Upgrade Locations \nHealth = 0 \nRegeneration = 1")]
    public List<int> Upgrades;
}
public enum DefaultIntUpgrade : int {
    Health = 0,
    Regeneration = 1,
    AccessLevel = 2
}