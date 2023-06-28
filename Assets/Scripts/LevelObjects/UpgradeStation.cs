using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeStation : MonoBehaviour, Interactable
{
    public UpgradeTarget UpgradeTarget;

    public string UpgradeTitle;

    // UIController Controller;

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
                        UpdateCard(Index, Player.possibleUpgrades[UpgradeIds[Index]], Player.possibleUpgradesLevel[UpgradeIds[Index]]);
                        break;

                    case BuyResult.Max:
                        // Controller.UpgradeCards[Index].Hide();
                        break;
                }
                break;

            case UpgradeTarget.Weapon:
                if (!(Player is WeaponUser))
                    return;

                switch ((Player as WeaponUser).weaponClass.BuyUpgrade(UpgradeIds[Index])) {
                    case BuyResult.Fail:
                        break;

                    case BuyResult.Success:
                        UpdateCard(Index, (Player as WeaponUser).weapon.Upgrades[UpgradeIds[Index]], (Player as WeaponUser).weaponClass.UpgradeLevels[UpgradeIds[Index]]);
                        break;

                    case BuyResult.Max:
                        // Controller.UpgradeCards[Index].Hide();
                        break;
                }                    
                break;
        }
    }

    public void UpdateCard(int Index, Upgrade Upgrade, int Level) {
        // UpgradeCardUIController Card = Controller.UpgradeCards[Index];

        // Card.Show();
        // Card.Name = Upgrade.name;
        // Card.Description = Upgrade.Description;
        // Card.CurrentLevel = Level;
        // Card.NewLevel = Level + 1;
        // Card.DNACost = Upgrade.Dna[Level + 1];
        // Card.TechCost = Upgrade.Tech[Level + 1];

        // if (Upgrade is IntUpgrade) {
        //     Card.CurrentValue = (Upgrade as IntUpgrade).Upgrades[Level].ToString();
        //     Card.NewValue = (Upgrade as IntUpgrade).Upgrades[Level + 1].ToString();
        // }
        // else if (Upgrade is FloatUpgrade) {
        //     Card.CurrentValue = (Upgrade as FloatUpgrade).Upgrades[Level].ToString();
        //     Card.NewValue = (Upgrade as FloatUpgrade).Upgrades[Level + 1].ToString();
        // }
        // else {
        //     Card.CurrentValue = "";
        //     Card.NewValue = "";
        // }
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
                PossibleUpgrades.AddRange(Player.possibleUpgrades);
                UpgradeLevels.AddRange(Player.possibleUpgradesLevel);
                break;

            case UpgradeTarget.Weapon:
                WeaponUser User = Player as WeaponUser;
                PossibleUpgrades.AddRange(User.weapon.Upgrades);
                UpgradeLevels.AddRange(User.weaponClass.UpgradeLevels);           
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

        UpgradeIds = new List<int>();
        UpgradeIds.Add(-1);
        UpgradeIds.Add(-1);
        UpgradeIds.Add(-1);

        if (PossibleUpgrades.Count >= 3) {

            int r = Random.Range(0, PossibleUpgrades.Count - 1);
            switch (UpgradeTarget) {
                case UpgradeTarget.Player:
                    UpgradeIds[0] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                    break;

                case UpgradeTarget.Weapon:
                    UpgradeIds[0] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                    break;
            }
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);

            r = Random.Range(0, PossibleUpgrades.Count - 1);
            switch (UpgradeTarget) {
                case UpgradeTarget.Player:
                    UpgradeIds[1] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                    break;

                case UpgradeTarget.Weapon:
                    UpgradeIds[1] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                    break;
            }
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);

            r = Random.Range(0, PossibleUpgrades.Count - 1);
            switch (UpgradeTarget) {
                case UpgradeTarget.Player:
                    UpgradeIds[2] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                    break;

                case UpgradeTarget.Weapon:
                    UpgradeIds[2] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                    break;
            }
            PossibleUpgrades.RemoveAt(r);
            UpgradeLevels.RemoveAt(r);
        }
        else {
            if (PossibleUpgrades.Count > 0) {
                int r = Random.Range(0, PossibleUpgrades.Count - 1);
                switch (UpgradeTarget) {
                    case UpgradeTarget.Player:
                        UpgradeIds[0] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                        break;

                    case UpgradeTarget.Weapon:
                        UpgradeIds[0] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                        break;
                }
                PossibleUpgrades.RemoveAt(r);
                UpgradeLevels.RemoveAt(r);

                if (PossibleUpgrades.Count > 1) {
                    r = Random.Range(0, PossibleUpgrades.Count - 1);
                    switch (UpgradeTarget) {
                        case UpgradeTarget.Player:
                            UpgradeIds[1] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                            break;

                        case UpgradeTarget.Weapon:
                            UpgradeIds[1] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                            break;
                    }
                    PossibleUpgrades.RemoveAt(r);
                    UpgradeLevels.RemoveAt(r);

                    if (PossibleUpgrades.Count > 2) {
                        r = Random.Range(0, PossibleUpgrades.Count - 1);
                        switch (UpgradeTarget) {
                            case UpgradeTarget.Player:
                                UpgradeIds[2] = Player.possibleUpgrades.IndexOf(PossibleUpgrades[r]);
                                break;

                            case UpgradeTarget.Weapon:
                                UpgradeIds[2] = (Player as WeaponUser).weapon.Upgrades.IndexOf(PossibleUpgrades[r]);
                                break;
                        }
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
            // UpgradeCardUIController Card = Controller.UpgradeCards[i];

            // int Id = UpgradeIds[i];

            // if (Id == -1) {
            //     Card.Hide();
            //     continue;
            // }

            // int Level = -1;
            // Upgrade Upgrade;

            // switch (UpgradeTarget) {
            //     case UpgradeTarget.Player:
            //         Level = Player.possibleUpgradesLevel[Id];
            //         Upgrade = Player.possibleUpgrades[Id];
            //         break;

            //     case UpgradeTarget.Weapon:
            //         Level = (Player as WeaponUser).weaponClass.UpgradeLevels[Id];
            //         Upgrade = (Player as WeaponUser).weapon.Upgrades[Id];
            //         break;

            //     default:
            //         Upgrade = new Upgrade();
            //         break;
            // }

            // if (Upgrade.Dna.Count - 1 == Level) {
            //     Card.Hide();
            //     continue;
            // }

            // Card.Show();
            // Card.Name = Upgrade.name;
            // Card.Description = Upgrade.Description;
            // Card.CurrentLevel = Level;
            // Card.NewLevel = Level + 1;
            // Card.DNACost = Upgrade.Dna[Level + 1];
            // Card.TechCost = Upgrade.Tech[Level + 1];

            // if (Upgrade is IntUpgrade) {
            //     Card.CurrentValue = (Upgrade as IntUpgrade).Upgrades[Level].ToString();
            //     Card.NewValue = (Upgrade as IntUpgrade).Upgrades[Level + 1].ToString();
            // }
            // else if (Upgrade is FloatUpgrade) {
            //     Card.CurrentValue = (Upgrade as FloatUpgrade).Upgrades[Level].ToString();
            //     Card.NewValue = (Upgrade as FloatUpgrade).Upgrades[Level + 1].ToString();
            // }
            // else {
            //     Card.CurrentValue = "";
            //     Card.NewValue = "";
            // }
        }

        //Controller.UpgradeTitle = UpgradeTitle;  

        UpgradeStationManager.MostRecent = this;      

        //Controller.OpenMenu(1);
    }

    void Start()
    {
        // Controller = FindObjectOfType<UIController>();

        Player = FindObjectOfType<Player>();
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