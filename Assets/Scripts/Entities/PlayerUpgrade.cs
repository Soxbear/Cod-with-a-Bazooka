using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public int Target;
    public List<int> Dna;
    public List<int> Tech;
    public List<int> Badges;
}

[CreateAssetMenu(fileName = "New Int Upgrade", menuName = "Upgrades/Int Upgrade", order = 0)]
public class IntUpgrade : Upgrade {
    public List<int> Upgrades;
}
public enum DefaultIntUpgrade {
    Health = 0,
    Regeneration = 1
}

[CreateAssetMenu(fileName = "New Float Upgrade", menuName = "Upgrades/Float Upgrade", order = 1)]
public class FloatUpgrade : Upgrade {
    public List<float> Upgrades;
}
public enum DefaultFloatUpgrade {
    Speed = 0, FireRate = 0,
    Acceleration = 1,
    Damage = 2
}

[CreateAssetMenu(fileName = "New Active Upgrade", menuName = "Upgrades/Active Upgrade", order = 2)]
public class ActiveUpgrade : Upgrade {
    public List<int> Upgrades;
}
public enum DefaultActiveUpgrade {

}