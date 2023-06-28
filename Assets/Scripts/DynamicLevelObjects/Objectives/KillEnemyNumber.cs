using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Soxbear.Channels;

public class KillEnemyNumber : MonoBehaviour
{
    public int Number;

    public bool Area;

    public ChannelReadWrite<bool> output;

    public ChannelReadWrite<bool> active;

    void Start()
    {
        EnemyManager.Manager.OnEnemyDeath.AddListener((Info) => {
            if (!active.channelValue)
                return;

            if (Area && !((Info.Position.x > transform.position.x - transform.localScale.x / 2 && Info.Position.x < transform.position.x + transform.localScale.x / 2) && (Info.Position.y > transform.position.y - transform.localScale.y / 2 && Info.Position.y < transform.position.y + transform.localScale.y / 2)))
                return;

            Number--;

            if (Number == 0) {
                output.channelValue = true;

                enabled = false;
            }                
        });
    }

    // void OnTriggerEnter2D(Collider2D Col) {
    //     if (Col.GetComponent<Player>() || Col.GetComponentInParent<Player>())
    //         Active = true;
    // }
}
