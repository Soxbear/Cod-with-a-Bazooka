using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active Upgrade", menuName = "Upgrades/Active Upgrade", order = 2)]
public class ActiveUpgrade : Upgrade {
    public List<int> Upgrades;
}
public enum DefaultActiveUpgrade : int {

}