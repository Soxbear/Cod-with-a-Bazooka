using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class Player : MonoBehaviour, Hittable, IntertalReferenceUser
{
    [Header("Stats")]

    [SerializeField]
    private int Hp;
    [property: SerializeField]
    public int Health {
        get { return Hp; }
        set {
            Hp = value;
            if (Hp > MaxHealth + IntUpgrades[(int) DefaultIntUpgrade.Health])
                Hp = MaxHealth + IntUpgrades[(int) DefaultIntUpgrade.Health];
            else if (Hp <= 0)
                OnDeath();
        }
    }
    protected Action OnDeath;

    protected Action OnPrimary;

    protected bool Primary {
        get {
            return (Controls != null) ? Controls.Player.Fire.IsPressed() : false;
        }
    }
    
    protected Action OnSecondary;

    protected bool Secondary {
        get {
            return (Controls != null) ? Controls.Player.Secondary.IsPressed() : false;
        }
    }

    private bool Flip;
    public bool HasControl = true;
    public bool AnimationControl = true;

    [HideInInspector]
    public Vector2 MoveVector;

    [HideInInspector]
    public Vector2 AimPos;

    private int Dna;
    private int Tech;

    public int DnaCount {
        get {
            return Dna;
        }
        set {
            Dna = value;
            UI.Dna = value;
        }
    }

    public int TechCount {
        get {
            return Tech;
        }
        set {
            Tech = value;
            UI.Tech = value;
        }
    }

    public List<int> IntUpgrades;
    public List<float> FloatUpgrades;
    public List<List<UnityEvent>> ActiveUpgrades;

    protected bool UIOpen {
        set {
            OpenUI = value;
            if (value)
                Controls.Player.Disable();
            else
                Controls.Player.Enable();
        }
        get {
            return OpenUI;
        }
    }
    bool OpenUI;

    public List<Vector2> InertialReferences {
        get; set;
    }


    [Header("Settings")]
    public int MaxHealth;

    public float MaxSpeed;
    public float Acceleration;
    public float IdleDrag;

    public float MaxRotationChange;

    public float MaxInteractionRange;

    public List<Upgrade> PossibleUpgrades;
    [HideInInspector]
    public List<int> PossibleUpgradesLevel;

    public List<Upgrade> SpecialUpgrades;
    [HideInInspector]
    public List<int> SpecialUpgradesLevel;

    protected bool UseDefaultMovement = true;
    protected bool UseDefaultRotation = true;
    protected bool UseDefaultAnimator = true;

    public LayerMask InteractableMask;
    
    public InputMaster Controls;


    [Header("References")]

    [HideInInspector]
    public Rigidbody2D Body;

    [HideInInspector]
    Animator Animator;

    [HideInInspector]
    public Transform PlayerBody;

    protected UIController UI;

    protected EnemyManager EnemyManager;

    public bool Hit(int Damage, Vector2 Direction, float Knockback, HitType HitType) {
        Health -= Damage;
        UI.Health = Health;

        if (Health <= 0) {
            //Death code
            return true;
        }

        Body.AddForce(Direction.normalized * Knockback, ForceMode2D.Impulse);
        return false;
    }

    public void Heal(int Amount) {
        Health += Amount;
        UI.Health = Health;
    }

    public BuyResult BuyUpgrade(int Number, UpgradeList List = UpgradeList.Normal) {
        Upgrade Upgrade = PossibleUpgrades[Number];
        int Level = PossibleUpgradesLevel[Number];

        if (!(DnaCount >= Upgrade.Dna[Level + 1] && TechCount >= Upgrade.Tech[Level + 1]))
            return BuyResult.Fail;

        DnaCount -= Upgrade.Dna[Level + 1];
        TechCount -= Upgrade.Tech[Level + 1];

        switch (List) {
            case UpgradeList.Normal:
                Upgrade = PossibleUpgrades[Number];
                PossibleUpgradesLevel[Number]++;
                break;

            case UpgradeList.Special:
                break;
        }
        
        Level++;

        bool Max = false;

        if (Upgrade is IntUpgrade) {
            IntUpgrades[Upgrade.Target] = (Upgrade as IntUpgrade).Upgrades[Level];
            if (Level == Upgrade.Dna.Count - 1)
                Max = true;
        }
        else if (Upgrade is FloatUpgrade) {
            FloatUpgrades[Upgrade.Target] = (Upgrade as FloatUpgrade).Upgrades[Level];
            if (Level == Upgrade.Dna.Count - 1)
                Max = true;
        }
        else {
            ActiveUpgrades[Upgrade.Target][Level].Invoke();
            if (Level == Upgrade.Dna.Count - 1)
                Max = true;
        }
        
        Heal((MaxHealth + IntUpgrades[(int) DefaultIntUpgrade.Health]) - Health);

        if (Max)
            return BuyResult.Max;
        return BuyResult.Success;
    }

    public enum UpgradeList {
        Normal,
        Special
    }

    IEnumerator Regen() {
        while (true) {
            if (IntUpgrades[(int) DefaultIntUpgrade.Regeneration] == 0) {
                yield return null;
                continue;
            }
            
            Health++;
            yield return new WaitForSeconds(1 / IntUpgrades[(int) DefaultIntUpgrade.Regeneration]);
        }
    }

    void OnEnable()
    {     
        Body = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        UI = FindObjectOfType<UIController>();
        EnemyManager = FindObjectOfType<EnemyManager>();
        Controls = new InputMaster(); 
        PlayerBody = transform.GetChild(1).transform;

        UI = FindObjectOfType<UIController>();

        Controls.Enable();
        Controls.Player.Movement.performed += ctx => { 
            MoveVector = ctx.ReadValue<Vector2>(); 
        };
        Controls.Player.Movement.canceled += ctx => { MoveVector = ctx.ReadValue<Vector2>(); };
        
        Controls.Player.Fire.performed += (Info) => {
            OnPrimary();
        };
        Controls.Player.Secondary.performed += (Info) => {
            OnSecondary();
        };
        Controls.Player.Interact.performed += (Info) => {
            Collider2D InteractObject = Physics2D.OverlapCircle(transform.position, MaxInteractionRange, InteractableMask);

            if (InteractObject != null)
                InteractObject.GetComponent<Interactable>().Interact();
        };

        if (PossibleUpgrades == null)
            PossibleUpgrades = new List<Upgrade>();

        if (SpecialUpgrades == null)
            SpecialUpgrades = new List<Upgrade>();

        PossibleUpgradesLevel = new List<int>();
        for (int i = 0; i < PossibleUpgrades.Count; i++) {
            PossibleUpgradesLevel.Add(0);
        }

        SpecialUpgradesLevel = new List<int>(SpecialUpgrades.Count);
        for (int i = 0; i < SpecialUpgrades.Count; i++) {
            SpecialUpgradesLevel.Add(0);
        }

        foreach (Upgrade Upgrade in PossibleUpgrades) {
            if (Upgrade is IntUpgrade) {
                IntUpgrades[Upgrade.Target] = (Upgrade as IntUpgrade).Upgrades[0];
            }
            else if (Upgrade is FloatUpgrade) {
                FloatUpgrades[Upgrade.Target] = (Upgrade as FloatUpgrade).Upgrades[0];
            }
            //No need for active upgrade prep
        }

        if (IntUpgrades == null)
            IntUpgrades = new List<int>();

        if (FloatUpgrades == null)
            FloatUpgrades = new List<float>();

        if (ActiveUpgrades == null)
            ActiveUpgrades = new List<List<UnityEvent>>();

        OnDeath += () => {

        };

        EnemyManager.OnEnemyDeath.AddListener((Info) => {
            DnaCount += Info.Dna;
            TechCount += Info.Tech;
        });

        InertialReferences = new List<Vector2>();
        
        StartCoroutine(UpdateInternalLoop());
        StartCoroutine(FixedUpdateInternalLoop());

        StartCoroutine(Regen());
    }

    void FixedUpdateInternal()
    {
        if (!UseDefaultMovement)
            goto EndDefaultMovement;

        if (!((MaxSpeed * FloatUpgrades[(int) DefaultFloatUpgrade.Speed]) < ((Body.velocity - this.GetTotalInertialReference()) + (MoveVector * (Acceleration * FloatUpgrades[(int) DefaultFloatUpgrade.Acceleration]) * Time.fixedDeltaTime)).magnitude))
            Body.AddForce((MoveVector * (Acceleration * FloatUpgrades[(int) DefaultFloatUpgrade.Acceleration])));
        
        if (MoveVector == Vector2.zero)
            Body.drag = IdleDrag;
        else
            Body.drag = 1;

        EndDefaultMovement:;
    }

    float LastTrueAngle;

    void UpdateInternal() {
        if (!UseDefaultAnimator)
            goto EndDefaultAnimation;

        Animator.SetFloat("Speed", MoveVector.magnitude * MaxSpeed, 0.2f, Time.deltaTime);

        EndDefaultAnimation:


        if (!UseDefaultRotation)
            goto EndDefaultRotation;

        if (MoveVector.magnitude > 0.05f) {
            Vector2 RotVector = MoveVector;

            float DesiredAngle = Vector2.SignedAngle(Vector2.right, RotVector);
            //DesiredAngle = (DesiredAngle < 0) ? (360 + DesiredAngle) : DesiredAngle;

            float TrueAngle = Mathf.MoveTowardsAngle(LastTrueAngle, DesiredAngle, MaxRotationChange * Time.deltaTime);

            PlayerBody.localRotation = Quaternion.Euler(0f, 0f, TrueAngle);

            if (Mathf.Cos(TrueAngle * Mathf.Deg2Rad) < -0.01f) {
                PlayerBody.GetComponent<SpriteRenderer>().flipY = true;
                Flip = true;
            }
            else if (Mathf.Cos(TrueAngle * Mathf.Deg2Rad) > 0.01f) {
                PlayerBody.GetComponent<SpriteRenderer>().flipY = false;
                Flip = false;
            }

            LastTrueAngle = TrueAngle;
        }
        
        EndDefaultRotation:
        
        AimPos = Controls.Player.AimRelative.ReadValue<Vector2>() * 10;
        AimPos = (Vector2) Camera.main.ScreenToWorldPoint(Controls.Player.AimScreen.ReadValue<Vector2>()) - (Vector2) transform.position;
    }
    
    IEnumerator UpdateInternalLoop() {
        while (true) {
            UpdateInternal();
            yield return null;
        }
    }
    IEnumerator FixedUpdateInternalLoop() {
        while (true) {
            yield return new WaitForFixedUpdate();
            FixedUpdateInternal();
        }
    }
}

public interface WeaponUser {
    Transform WeaponPivot {
        get; set;
    }
    Rigidbody2D Rb {
        get; set;
    }
    WeaponObject Weapon {
        get; set;
    }
    Weapon WeaponClass {
        get; set;
    }
}