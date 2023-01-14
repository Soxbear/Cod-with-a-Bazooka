using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> Enemies;

    [HideInInspector]
    public UnityEvent<Enemy> RegisterEnemy;

    [HideInInspector]
    public UnityEvent<EnemyDeathInfo> OnEnemyDeath;

    void OnEnable() {
        Enemies = new List<Enemy>();

        RegisterEnemy.AddListener((Enemy) => {
            Enemies.Add(Enemy);
        });

        OnEnemyDeath.AddListener((Info) => {
            Enemies.Remove(Info.Enemy);
        });
    }
}

public delegate void RegisterEnemy(Enemy Enemy);

public delegate void OnEnemyDeath(EnemyDeathInfo Info);

public struct EnemyDeathInfo {
    public string EnemyName;
    public Enemy Enemy;
    public Vector2 Position;
    public int Dna;
    public int Tech;

    public EnemyDeathInfo(string _EnemyName, Enemy _Enemy, Vector2 _Position, int _Dna, int _Tech) {
        EnemyName = _EnemyName;
        Enemy = _Enemy;
        Position = _Position;
        Dna = _Dna;
        Tech = _Tech;
    }
}