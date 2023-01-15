using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : ScriptableObject
{
    public string Name;
    public string Description;
    public int Target;
    public List<int> Dna;
    public List<int> Tech;
    public List<int> Badges;
}