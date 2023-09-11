using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades {
    [AttributeUsage(AttributeTargets.Method)]
    public class UpgradeHandler : Attribute
    {
        public string metadataName;

        public UpgradeStation station;

        public UpgradeHandler(string _metadataName, UpgradeStation _station) {
            metadataName = _metadataName;
            station = _station;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class UpgradeDirectory : Attribute {
        public string directory;

        public UpgradeDirectory(string _directory) {
            directory = _directory;
        }
    }
}
