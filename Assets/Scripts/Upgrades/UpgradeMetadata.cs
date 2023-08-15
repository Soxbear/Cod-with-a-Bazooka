using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades {
    public class UpgradeMetadata : ScriptableObject
    {
        public string upgradeName;

        public Sprite[] image;
        
        public string[] description;

        public string[] indicators;

        public int maxLevel;

        public int[] dna;

        public int[] tech;

        public int[] coins;
    }

    public enum UpgradeStation {
        BIOTECH
    }
}

