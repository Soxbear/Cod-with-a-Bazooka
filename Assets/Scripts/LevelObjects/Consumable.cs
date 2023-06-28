using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public int Healing;
    public GameObject ConsumeEffect;

    void OnTriggerEnter2D(Collider2D Col) {
        Col.GetComponentInParent<Player>().Heal(Healing);
        
        if (ConsumeEffect != null)
            Instantiate(ConsumeEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
