using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
    public float Speed;

    void Start() {
        ParticleSystem Particles = GetComponent<ParticleSystem>();
        AreaEffector2D Effector = GetComponent<AreaEffector2D>();

        ParticleSystem.MainModule Main = Particles.main;
        Main.startSpeed = Speed;
        Main.startLifetime = transform.localScale.y / Speed;
        ParticleSystem.EmissionModule Emit = Particles.emission;
        Emit.rateOverTime = 5 * transform.localScale.x;

        Effector.forceMagnitude = Speed;
    }

    void OnTriggerEnter2D(Collider2D Col) {
        List<IntertalReferenceUser> Users = new List<IntertalReferenceUser>();
        Users.AddRange(Col.GetComponents<IntertalReferenceUser>());
        Users.AddRange(Col.GetComponentsInParent<IntertalReferenceUser>());

        foreach (IntertalReferenceUser User in Users)
            User.inertialReferences.Add(transform.up * Speed);
    }

    void OnTriggerExit2D(Collider2D Col) {
        List<IntertalReferenceUser> Users = new List<IntertalReferenceUser>();
        Users.AddRange(Col.GetComponents<IntertalReferenceUser>());
        Users.AddRange(Col.GetComponentsInParent<IntertalReferenceUser>());

        foreach (IntertalReferenceUser User in Users)
            User.inertialReferences.Remove(transform.up * Speed);
    }
}