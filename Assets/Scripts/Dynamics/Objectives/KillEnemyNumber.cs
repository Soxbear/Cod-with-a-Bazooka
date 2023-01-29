using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyNumber : MonoBehaviour
{
    public int Number;

    public bool Area;

    public MonoBehaviour Output;

    bool Active;

    void Start()
    {
        EnemyManager.Manager.OnEnemyDeath.AddListener((Info) => {
            if (!Active)
                return;

            if (Area && !((Info.Position.x > transform.position.x - transform.localScale.x / 2 && Info.Position.x < transform.position.x + transform.localScale.x / 2) && (Info.Position.y > transform.position.y - transform.localScale.y / 2 && Info.Position.y < transform.position.y + transform.localScale.y / 2)))
                return;

            Number--;

            if (Number == 0) {
                (Output as Dynamic<bool>).PassValue(true);

                enabled = false;
            }                
        });
    }

    void OnTriggerEnter2D(Collider2D Col) {
        if (Col.GetComponent<Player>() || Col.GetComponentInParent<Player>())
            Active = true;
    }
}
