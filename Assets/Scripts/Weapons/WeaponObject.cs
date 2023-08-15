using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons/Weapon Object", order = 0)]
public class WeaponObject : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public string Primary;
    public string Secondary;
    public string PrimaryDescription;
    public string SecondaryDescription;
    public int PrimaryDamage;
    public int SecondaryDamage;
    public GameObject WeaponGameobject;
}
