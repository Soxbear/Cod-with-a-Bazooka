using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Upgrades {
    public interface Upgradable
    {
        Dictionary<string, int> upgradeLevels {
            get;
            set;
        }

        Upgrade[] upgradeList {
            get; set;
        }

        // Upgrade[] GetUpgrades(UpgradeStation station) {
        //     if (upgradeList == null) {
        //         List<Upgrade> upgrades = new List<Upgrade>();

        //         foreach (MethodInfo m in this.GetType().GetMethods()) {
        //             foreach (UpgradeHandler h in m.GetCustomAttributes<UpgradeHandler>(false)) {
        //                 if (h.station != station)
        //                     continue;
                        
        //                 UpgradeMethod method;

        //                 try {
        //                     method = (UpgradeMethod) m.CreateDelegate(typeof(UpgradeMethod));
        //                 }
        //                 catch (Exception) {
        //                     Debug.LogError("Upgrade Method did not match the required delegate");
        //                     continue;
        //                 }

        //                 upgrades.Add(new Upgrade(method, Resources.Load<UpgradeMetadata>($"Upgrades/{h.metadataName}")));

        //                 if (!upgradeLevels.Keys.Contains(upgrades.Last()))
        //                     upgradeLevels.Add(upgrades.Last(), -1);
        //             }
        //         }

        //         foreach (Upgrade u in upgrades) {
        //             upgradeLevels.Add(u, -1);
        //         }

        //         upgradeList = upgrades.ToArray();
        //     }

        //     return upgradeList;
        // }
    }
}