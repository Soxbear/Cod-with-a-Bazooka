using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using TMPro;

public class BiotechUpgradeManager : MonoBehaviour
{
    public static BiotechUpgradeManager singleton;

    public Menu menu;
    
    public UpgradeTileController[] tiles;

    public TMPro.TextMeshProUGUI none;

    Upgradable upgradable;

    Upgrade[] upgrades = new Upgrade[3];

    public void SetUpgrades(Upgradable u, Upgrade[] us) {
        if (us.Length == 0)
            none.enabled = true;
        else
            none.enabled = false;

        us = new Upgrade[] {(us.Length >= 1) ? us[0] : new Upgrade(false), (us.Length >= 2) ? us[1] : new Upgrade(false), (us.Length >= 3) ? us[2] : new Upgrade(false)};
        
        upgradable = u;
        upgrades = us;

        tiles[0].SetTile(upgrades[0], upgradable.upgradeLevels[upgrades[0]]);
        tiles[1].SetTile(upgrades[1], upgradable.upgradeLevels[upgrades[1]]);
        tiles[2].SetTile(upgrades[2], upgradable.upgradeLevels[upgrades[2]]);
    }

    public void Buy(int number) {
        if (upgrades[number].dna[upgradable.upgradeLevels[upgrades[number]]] > Player.dnaCount || upgrades[number].tech[upgradable.upgradeLevels[upgrades[number]]] > Player.techCount)
            return;

        Player.dnaCount -= upgrades[number].dna[upgradable.upgradeLevels[upgrades[number]]];
        Player.techCount -= upgrades[number].tech[upgradable.upgradeLevels[upgrades[number]]];
        
        upgrades[number].method(upgradable.upgradeLevels[upgrades[number]]);

        upgradable.upgradeLevels[upgrades[number]]++;

        tiles[number].SetTile(upgrades[number], upgradable.upgradeLevels[upgrades[number]]);
    }

    public void HealDna() {
        if (Player.dnaCount < 10 || (upgradable as Player).health == (upgradable as Player).maxHealth)
            return;

        Player.dnaCount -= 10;

        (upgradable as Player).Heal((upgradable as Player).maxHealth);
    }

    public void HealTech() {
        if (Player.techCount < 10 || (upgradable as Player).health == (upgradable as Player).maxHealth)
            return;

        Player.techCount -= 10;

        (upgradable as Player).Heal((upgradable as Player).maxHealth);
    }

    public TextMeshProUGUI dna;

    public TextMeshProUGUI tech;

    public UIMouseOverGrow dnaG;
    public UIMouseOverGrow techG;

    public Button dnaB;
    public Button techB;

    public CanvasGroup buttons;

    public TextMeshProUGUI maxHp;

    public Color afford;
    public Color cantAfford;

    void FixedUpdate() {
        if (!menu.Opened)
            return;

        if (Player.dnaCount < 10 || (upgradable as Player).health == (upgradable as Player).maxHealth) {
            dna.color = cantAfford;
            dnaG.enabled = false;
            dnaB.enabled = false;
        }
        else {
            dna.color = afford;
            dnaG.enabled = true;
            dnaB.enabled = true;
        }

        if (Player.techCount < 10 || (upgradable as Player).health == (upgradable as Player).maxHealth) {
            tech.color = cantAfford;
            techG.enabled = false;
            techB.enabled = false;
        }
        else {
            tech.color = afford;
            techG.enabled = true;
            techB.enabled = true;
        }

        if ((upgradable as Player).health == (upgradable as Player).maxHealth) {
            buttons.alpha = 0;
            buttons.blocksRaycasts = false;
            maxHp.enabled = true;
        }
        else {
            buttons.alpha = 1;
            buttons.blocksRaycasts = true;
            maxHp.enabled = false;
        }
    }

    public BiotechUpgradeManager() {
        singleton = this;
    }
}
