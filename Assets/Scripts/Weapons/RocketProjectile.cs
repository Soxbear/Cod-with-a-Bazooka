using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public float InitialSpeed;
    public float PropelSpeed;

    public float MaxLifetime;
    float Lifetime;

    public GameObject Explosion;

    Rigidbody2D Body;

    void Start() {
        Body = GetComponent<Rigidbody2D>();
        Body.AddRelativeForce(new Vector2(InitialSpeed, 0f), ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        Body.AddRelativeForce(new Vector2(PropelSpeed, 0f));
        Lifetime += Time.deltaTime;
        if (Lifetime >= MaxLifetime) {
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D Col) {
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
