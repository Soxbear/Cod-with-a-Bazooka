using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour, Interactable
{
    public UpgradeTarget UpgradeTarget;

    public string UpgradeTitle;

    UIController Controller;

    List<int> UpgradeIds;

    Player Player;

    bool Generated;

    public void Buy(int Index) {
        switch (UpgradeTarget) {
            case UpgradeTarget.Player:
                switch (Player.BuyUpgrade(UpgradeIds[Index])) {
                    case BuyResult.Fail:
                        break;

                    case BuyResult.Success:
                        //switch ()
                        break;

                    case BuyResult.Max:
                        Controller.UpgradeCards[Index].Hide();
                        break;
                }
                break;

            case UpgradeTarget.Weapon:
                if (!(Player is WeaponUser))
                    return;

                switch ((Player as WeaponUser).WeaponClass.BuyUpgrade(UpgradeIds[Index])) {
                    case BuyResult.Fail:
                        break;

                    case BuyResult.Success:
                        break;

                    case BuyResult.Max:
                        Controller.UpgradeCards[Index].Hide();
                        break;
                }                    
                break;
        }
    }

    public void UpgradeCard(int Index, Upgrade Upgrade, int Level) {
        UpgradeCardUIController Card = Controller.UpgradeCards[Index];

        Card.Show();
        Card.Name = Upgrade.name;
        Card.Description = Upgrade.Description;
        Card.CurrentLevel = Level;
        Card.NewLevel = Level + 1;
        Card.DNACost = Upgrade.Dna[Level + 1];
        Card.TechCost = Upgrade.Tech[Level + 1];

        if (Upgrade is IntUpgrade) {
            Card.CurrentValue = (Upgrade as IntUpgrade).Upgrades[Level].ToString();
            Card.NewValue = (Upgrade as IntUpgrade).Upgrades[Level + 1].ToString();
        }
        else if (Upgrade is FloatUpgrade) {
            Card.CurrentValue = (Upgrade as FloatUpgrade).Upgrades[Level].ToString();
            Card.NewValue = (Upgrade as FloatUpgrade).Upgrades[Level + 1].ToString();
        }
        else {
            Card.CurrentValue = "";
            Card.NewValue = "";
        }
    }

    public void Interact() {
        if (UpgradeTarget == UpgradeTarget.Weapon && !(Player is WeaponUser))
            return;

        if (Generated)
            goto OpenMenu;

        Generated = true;

        List<Upgrade> PossibleUpgrades = new List<Upgrade>();
        List<int> UpgradeLevels = new List<int>();

        switch (UpgradeTarget) {
            case UpgradeTarget.Player:
                PossibleUpgrades = Player.PossibleUpgrades;
                UpgradeLevels = Player.PossibleUpgradesLevel;
                break;

            case UpgradeTarget.Weapon:
                WeaponUser User = Player as WeaponUser;
                PossibleUpgrades = User.Weapon.Upgrades;
                UpgradeLevels = User.WeaponClass.UpgradeLevels;           
                break;
        }
        
        List<int> ToRemove = new List<int>();

        for (int i = 0; i < PossibleUpgrades.Count; i++) {
            if (PossibleUpgrades[i].Dna.Count == UpgradeLevels[i])
                ToRemove.Add(i);
        }

        for (int i = 0; i < ToRemove.Count; i++) {
            PossibleUpgrades.RemoveAt(ToRemove[ToRemove.Count - 1 - i]);
            UpgradeLevels.RemoveAt(ToRemove[ToRemove.Count - 1 - i]);
        }

        if (PossibleUpgrades.Count > 3) {
            int r = Random.Range(0, PossibleUpgrades.Count - 1);
            UpgradeIds[0] = r;
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);

            Random.Range(0, PossibleUpgrades.Count - 1);
            UpgradeIds[1] = r;
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);

            Random.Range(0, PossibleUpgrades.Count - 1);
            UpgradeIds[2] = r;
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);
        }
        else {
            if (PossibleUpgrades.Count > 0) {
                int r = Random.Range(0, PossibleUpgrades.Count - 1);
                UpgradeIds[0] = r;
                PossibleUpgrades.RemoveAt(r);
                UpgradeLevels.RemoveAt(r);

                if (PossibleUpgrades.Count > 1) {
                    r = Random.Range(0, PossibleUpgrades.Count - 1);
                    UpgradeIds[1] = r;
                    PossibleUpgrades.RemoveAt(r);
                    UpgradeLevels.RemoveAt(r);

                    if (PossibleUpgrades.Count > 2) {
                        r = Random.Range(0, PossibleUpgrades.Count - 1);
                        UpgradeIds[2] = r;
                        PossibleUpgrades.RemoveAt(r);
                        UpgradeLevels.RemoveAt(r);
                    }
                    else {
                        UpgradeIds[2] = -1;
                    }
                }
                else {
                    UpgradeIds[1] = -1;
                    UpgradeIds[2] = -1;
                }
            }
            else {
                UpgradeIds[0] = -1;
                UpgradeIds[1] = -1;
                UpgradeIds[2] = -1;
            }
        }

        OpenMenu:

        for (int i = 0; i < 3; i++) {
            UpgradeCardUIController Card = Controller.UpgradeCards[i];

            int Id = UpgradeIds[i];

            if (Id == -1) {
                Card.Hide();
                return;
            }

            int Level = -1;
            Upgrade Upgrade = new Upgrade();

            switch (UpgradeTarget) {
                case UpgradeTarget.Player:
                    Level = Player.PossibleUpgradesLevel[Id];
                    Upgrade = Player.PossibleUpgrades[Id];
                    break;

                case UpgradeTarget.Weapon:
                    Level = (Player as WeaponUser).WeaponClass.UpgradeLevels[Id];
                    Upgrade = (Player as WeaponUser).Weapon.Upgrades[Id];
                    break;
            }

            Card.Show();
            Card.Name = Upgrade.name;
            Card.Description = Upgrade.Description;
            Card.CurrentLevel = Level;
            Card.NewLevel = Level + 1;
            Card.DNACost = Upgrade.Dna[Level + 1];
            Card.TechCost = Upgrade.Tech[Level + 1];

            if (Upgrade is IntUpgrade) {
                Card.CurrentValue = (Upgrade as IntUpgrade).Upgrades[Level].ToString();
                Card.NewValue = (Upgrade as IntUpgrade).Upgrades[Level + 1].ToString();
            }
            else if (Upgrade is FloatUpgrade) {
                Card.CurrentValue = (Upgrade as FloatUpgrade).Upgrades[Level].ToString();
                Card.NewValue = (Upgrade as FloatUpgrade).Upgrades[Level + 1].ToString();
            }
            else {
                Card.CurrentValue = "";
                Card.NewValue = "";
            }
        }

        Controller.UpgradeTitle = UpgradeTitle;  

        UpgradeStationManager.MostRecent = this;      

        Controller.OpenMenu(1);
    }

    void Start()
    {
        Controller = FindObjectOfType<UIController>();

        Player Player = FindObjectOfType<Player>();
    }
}

public static class UpgradeStationManager {
    public static UpgradeStation MostRecent;
}

public enum UpgradeTarget {
    Player,
    Weapon
}

public enum BuyResult {
    Fail,
    Success,
    Max
}