using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Upgrades {

    public struct Upgrade
    {
        public bool empty;

        public UpgradeMethod method;

        public string upgradeName;

        public Sprite[] image;
        
        public string[] description;
        
        public string[] indicators;

        public int maxLevel;

        public int[] dna;

        public int[] tech;

        public int[] coins;

        public Upgrade(UpgradeMethod _method, UpgradeMetadata data) {
            empty = false;
            method = _method;
            upgradeName = data.upgradeName;
            image = data.image;
            description = data.description;
            maxLevel = data.maxLevel;
            dna = data.dna;
            tech = data.tech;
            coins = data.coins;
            indicators = data.indicators;
        }

        public Upgrade(bool b) {
            empty = true;
            method = null;
            upgradeName = "";
            image = null;
            description = null;
            indicators = null;
            maxLevel = -1;
            dna = null;
            tech = null;
            coins = null;
        }
    }

    public static class UpgradeUtil {
        public static Upgrade[] NotMax(this IEnumerable<Upgrade> upgradeList, Upgradable upgradable) {
            List<Upgrade> list = upgradeList.ToList();

            List<Upgrade> toRemove = new List<Upgrade>();

            foreach (Upgrade u in list) {
                if (upgradable.upgradeLevels[u] == u.maxLevel)
                    toRemove.Add(u);
            }

            foreach (Upgrade u in toRemove) {
                list.Remove(u);
            }

            return list.ToArray();
        }

        public static Upgrade[] RandomCount(this IEnumerable<Upgrade> upgradeList, int count) {
            List<Upgrade> fullList = upgradeList.ToList();

            List<Upgrade> list = new List<Upgrade>();

            for (int i = 0; i < Mathf.Min(count, fullList.Count); i++) {
                int r = UnityEngine.Random.Range(0, fullList.Count() - 1);

                list.Add(fullList[r]);

                fullList.RemoveAt(r);
            }

            return list.ToArray();
        }

        public static Upgrade[] GetUpgrades(this Upgradable u, UpgradeStation station) {
            if (u.upgradeList == null || u.upgradeList.Length == 0) {

                List<Upgrade> upgrades = new List<Upgrade>();

                foreach (MethodInfo m in u.GetType().GetMethods()) {
                    foreach (UpgradeHandler h in m.GetCustomAttributes<UpgradeHandler>(false)) {
                        if (h.station != station)
                            continue;
                        
                        UpgradeMethod method;

                        try {
                            method = (UpgradeMethod) m.CreateDelegate(typeof(UpgradeMethod));
                        }
                        catch (Exception) {
                            Debug.LogError("Upgrade Method did not match the required delegate");
                            continue;
                        }

                        upgrades.Add(new Upgrade(method, Resources.Load<UpgradeMetadata>($"Upgrades/{h.metadataName}")));

                        // if (!u.upgradeLevels.Keys.Contains(upgrades.Last()))
                            u.upgradeLevels.Add(upgrades.Last(), -1);
                    }
                }

                foreach (Upgrade up in upgrades) {
                    u.upgradeLevels.Add(up, -1);
                }

                u.upgradeList = upgrades.ToArray();
            }

            return u.upgradeList;
        }
    }

    public delegate void UpgradeMethod(int level);
}
