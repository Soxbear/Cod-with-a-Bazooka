using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour
{
    public WeaponUser WeaponUser;

    WeaponObject Object;

    public List<int> IntUpgrades;
    public List<float> FloatUpgrades;
    public List<List<UnityEvent>> ActiveUpgrades;


    public List<int> UpgradeLevels;


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
        WeaponUser.Rb.AddForce(WeaponUser.WeaponPivot.rotation * new Vector2(Amount, 0f), ForceMode2D.Impulse);
    }


    public BuyResult BuyUpgrade(int Number, UpgradeList List = UpgradeList.Normal) {
        Player Player = FindObjectOfType<Player>();

        Upgrade Upgrade = new Upgrade();
        int Level = UpgradeLevels[Number];

        if (!(Player.DnaCount >= Upgrade.Dna[Level + 1] && Player.TechCount >= Upgrade.Tech[Level + 1]))
            return BuyResult.Fail;

        switch (List) {
            case UpgradeList.Normal:
                Upgrade = Object.Upgrades[Number];
                UpgradeLevels[Number]++;
                break;
        }
        
        Level++;

        if (Upgrade is IntUpgrade) {
            IntUpgrades[Upgrade.Target] = (Upgrade as IntUpgrade).Upgrades[Level];
        }
        else if (Upgrade is FloatUpgrade) {
            FloatUpgrades[Upgrade.Target] = (Upgrade as FloatUpgrade).Upgrades[Level];
        }
        else {
            ActiveUpgrades[Upgrade.Target][Level].Invoke();
        }

        return BuyResult.Fail;
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
        Object = WeaponUser.Weapon;
    }
}

public delegate bool GetBool();