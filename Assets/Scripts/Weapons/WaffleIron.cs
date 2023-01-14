using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaffleIron : ProjectilePrimaryWeapon
{   
    public float BakeRadius;

    public int BakeDamage;

    public float BakeShotsPerSecond;

    public GameObject Waffle;

    public LayerMask EnemyMask;

    public override void OnSecondary()
    {
        if (SecondaryReloading)
            return;

        foreach (Collider2D Col in Physics2D.OverlapCircleAll(transform.position, BakeRadius, EnemyMask)) {
            Enemy Enemy = Col.GetComponent<Enemy>();
            
            if (Enemy == null)
                continue;

            if (Enemy.HitNoConfetti(BakeDamage, Enemy.transform.position - transform.position, 0)) {
                GameObject NewWaffle = Instantiate(Waffle, Enemy.transform.position, Quaternion.identity);
                NewWaffle.GetComponent<SpriteRenderer>().color = Enemy.ConfettiColor;
            }

        }

        StartCoroutine(AttackAnim());
        
        ReloadSecondary(1/BakeShotsPerSecond);
    }

    IEnumerator AttackAnim() {
        float TimeElapsed = 0;

        while (TimeElapsed <= 0.5f) {
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 360 * DynamicUtil.EvaluateCurve(DynamicUtil.EvalCurve.Sine, TimeElapsed * 2));            
            TimeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
    }
}
