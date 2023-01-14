using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectilePrimaryWeapon : Weapon
{
    [Header("Primary")]
    public float ShotsPerSecond = 1;
    public GameObject Projectile;
    public float Recoil = 0.05f;
    public bool Automatic = true;
    public Transform ShootPoint;

    [Header("Secondary")]
    private bool Reloading;

    protected virtual void Shoot() {
        if (ShootPoint.rotation.eulerAngles.y == 180f)
            Instantiate(Projectile, ShootPoint.position, Quaternion.Euler(0f, 0f, -ShootPoint.eulerAngles.z + 180f));
        else
            Instantiate(Projectile, ShootPoint.position, ShootPoint.rotation);

        ApplyRecoil(-Recoil);

        ReloadPrimary(1/ShotsPerSecond);
    }

    public override void OnPrimary()
    {
        if (!PrimaryReloading)
            Shoot();
    }

    void UpdateInternal() {
        if (!PrimaryReloading && Primary)
            Shoot();
    }

    private IEnumerator InternalUpdatePing() {
        while (true) {
            UpdateInternal();
            yield return null;
        }
    }

    protected override void OnEnable() {
        base.OnEnable();

        if (Automatic)
            StartCoroutine(InternalUpdatePing());
    }
}
