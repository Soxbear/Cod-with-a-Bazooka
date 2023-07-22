using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelComponentGenerator : MonoBehaviour
{
    public abstract void Generate();

    public abstract void PostGenerate();
}

public abstract class SingleLevelComponentGenerator : LevelComponentGenerator {

}

public abstract class DoubleLevelComponentGenerator : LevelComponentGenerator {
    
}