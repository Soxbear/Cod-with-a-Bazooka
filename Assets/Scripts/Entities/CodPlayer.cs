using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CodPlayer : Player, WeaponUser
{
    [Header("Stats")]

    Transform AimPivot;

    public WeaponObject Weapon {
        get {
            return HeldWeapon;
        }
        set {
            HeldWeapon = value;
            if (transform.GetChild(0).GetChild(0).childCount > 0)
                Destroy(transform.GetChild(0).GetChild(0).GetChild(0));

            Instantiate(HeldWeapon.WeaponGameobject, transform.GetChild(0).GetChild(0));
            WeaponClass = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Weapon>();
        }
    }
    public WeaponObject HeldWeapon;

    [HideInInspector]
    public Weapon WeaponClass {
        get; set;
    }


    //[Header("References")]
    [HideInInspector]
    public Transform WeaponPivot {
        get {
            return AimPivot;
        }
        set {
            AimPivot = value;
        }
    }
    [HideInInspector]
    public Rigidbody2D Rb {
        get {
            return Body;
        }
        set {
            Body = value;
        }
    }

    void Update() {
        float Angle = Vector2.SignedAngle(Vector2.right, AimPos);

        float Subtract = Mathf.Asin(WeaponClass.AimOffset / AimPos.magnitude) * Mathf.Rad2Deg;

        if (!float.IsNaN(Angle-Subtract)) {
            AimPivot.Rotate(0f, 0f, (Angle - Subtract) - AimPivot.rotation.eulerAngles.z);
        }
    }

    void Start() {
        AimPivot = transform.GetChild(0);

        Weapon = HeldWeapon;

        OnPrimary += () => {
            WeaponClass.OnPrimary();
        };
        OnSecondary += () => {
            WeaponClass.OnSecondary();
        };
        WeaponClass.GetPrimary += () => {
            return Primary;
        };
        WeaponClass.GetSecondary += () => {
            return Secondary;
        };


        OnDeath += () => {
            SceneManager.LoadScene("ded");
        };
    }
}

