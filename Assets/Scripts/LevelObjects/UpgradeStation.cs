using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgrades;

public class UpgradeStation : MonoBehaviour, Interactable
{
    Upgrade[] upgrades;

    public void Interact() {
        if (upgrades == null)
            upgrades = FindObjectOfType<Player>().GetUpgrades(Upgrades.UpgradeStation.BIOTECH).NotMax(FindObjectOfType<Player>()).RandomCount(3);

        BiotechUpgradeManager.singleton.SetUpgrades(FindObjectOfType<Player>(), upgrades);

        MenuController.OpenMenu(MenuController.MenuType.BIOTECH);
    }
}