using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    public WeaponUser WeaponUser;

    WeaponObject Object;

    public float AimOffset;

    public virtual void OnPrimary() {

    }

    public GetBool GetPrimary;

    public bool Primary {
        get {
            return GetPrimary();
        }
    }

    public virtual void OnSecondary() {

    }

    public GetBool GetSecondary;

    public bool Secondary {
        get {
            return GetSecondary();
        }
    }

    public void ApplyRecoil(float Amount) {
        WeaponUser.rb.AddForce(WeaponUser.weaponPivot.rotation * new Vector2(Amount, 0f), ForceMode2D.Impulse);
    }

    public enum UpgradeList {
        Normal
    }

    protected bool PrimaryReloading = true;
    protected bool SecondaryReloading = true;

    protected void ReloadPrimary(float Time) {
        StartCoroutine(ReloadPrimaryIe(Time));
    }
    protected void ReloadSecondary(float Time) {
        StartCoroutine(ReloadSecondaryIe(Time));
    }

    protected IEnumerator ReloadPrimaryIe(float Time) {
        PrimaryReloading = true;
        yield return new WaitForSeconds(Time);
        PrimaryReloading = false;
    }
    protected IEnumerator ReloadSecondaryIe(float Time) {
        SecondaryReloading = true;
        yield return new WaitForSeconds(Time);
        SecondaryReloading = false;
    }

    protected virtual void OnEnable() {
        ReloadPrimary(0.1f);
        ReloadSecondary(0.1f);
        WeaponUser = GetComponentInParent<WeaponUser>();
        Object = WeaponUser.weapon;
    }
}

public delegate bool GetBool();