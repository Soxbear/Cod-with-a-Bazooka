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
    private int hp;
    public int health {
        get { return hp; }
        set {
            hp = value;
            UIManager.healthUI.health = value;
            if (hp > maxHealth + intUpgrades[(int) DefaultIntUpgrade.Health])
                hp = maxHealth + intUpgrades[(int) DefaultIntUpgrade.Health];
            else if (hp <= 0)
                OnDeath();
        }
    }
    protected Action OnDeath;

    protected Action OnPrimary;

    protected bool primary {
        get {
            return (controls != null) ? controls.Player.Fire.IsPressed() : false;
        }
    }
    
    protected Action OnSecondary;

    protected bool secondary {
        get {
            return (controls != null) ? controls.Player.Secondary.IsPressed() : false;
        }
    }

    private bool flip;
    public bool hasControl = true;
    public bool animationControl = true;

    [HideInInspector]
    public Vector2 moveVector;

    [HideInInspector]
    public Vector2 aimPos;

    private int dna;
    private int tech;

    public int dnaCount {
        get {
            return dna;
        }
        set {
            dna = value;
            // UI.Dna = value;
        }
    }

    public int techCount {
        get {
            return tech;
        }
        set {
            tech = value;
            // UI.Tech = value;
        }
    }

    public List<int> intUpgrades;
    public List<float> floatUpgrades;
    public List<List<UnityEvent>> activeUpgrades;

    protected bool UIOpen {
        set {
            openUI = value;
            if (value)
                controls.Player.Disable();
            else
                controls.Player.Enable();
        }
        get {
            return openUI;
        }
    }
    bool openUI;

    public List<Vector2> inertialReferences {
        get; set;
    }


    [Header("Settings")]
    [SerializeField]
    private int _maxHealth;
    public int maxHealth {
        get {
            return _maxHealth;
        }
        set {
            _maxHealth = value;
            UIManager.healthUI.maxHealth = value;
        }
    }

    public float maxSpeed;
    public float acceleration;
    public float idleDrag;

    public float maxRotationChange;

    public float maxInteractionRange;

    public List<Upgrade> possibleUpgrades;
    [HideInInspector]
    public List<int> possibleUpgradesLevel;

    public List<Upgrade> specialUpgrades;
    [HideInInspector]
    public List<int> specialUpgradesLevel;

    protected bool useDefaultMovement = true;
    protected bool useDefaultRotation = true;
    protected bool useDefaultAnimator = true;

    public LayerMask interactableMask;
    
    public InputMaster controls;


    [Header("References")]

    [HideInInspector]
    public Rigidbody2D body;

    [HideInInspector]
    Animator animator;

    [HideInInspector]
    public Transform playerBody;

    // protected UIController UI;

    protected EnemyManager enemyManager;

    public bool Hit(int damage, Vector2 direction, float knockback, HitType hitType) {
        health -= damage;
        //UI.Health = health;

        if (health <= 0) {
            //Death code
            return true;
        }

        body.AddForce(direction.normalized * knockback, ForceMode2D.Impulse);
        return false;
    }

    public void Heal(int amount) {
        health += amount;
        //UI.Health = health;
    }

    public BuyResult BuyUpgrade(int number, UpgradeList list = UpgradeList.Normal) {
        Upgrade upgrade = possibleUpgrades[number];
        int level = possibleUpgradesLevel[number];

        if (!(dnaCount >= upgrade.Dna[level + 1] && techCount >= upgrade.Tech[level + 1]))
            return BuyResult.Fail;

        dnaCount -= upgrade.Dna[level + 1];
        techCount -= upgrade.Tech[level + 1];

        switch (list) {
            case UpgradeList.Normal:
                upgrade = possibleUpgrades[number];
                possibleUpgradesLevel[number]++;
                break;

            case UpgradeList.Special:
                break;
        }
        
        level++;

        bool max = false;

        if (upgrade is IntUpgrade) {
            intUpgrades[upgrade.Target] = (upgrade as IntUpgrade).Upgrades[level];
            if (level == upgrade.Dna.Count - 1)
                max = true;
        }
        else if (upgrade is FloatUpgrade) {
            floatUpgrades[upgrade.Target] = (upgrade as FloatUpgrade).Upgrades[level];
            if (level == upgrade.Dna.Count - 1)
                max = true;
        }
        else {
            activeUpgrades[upgrade.Target][level].Invoke();
            if (level == upgrade.Dna.Count - 1)
                max = true;
        }
        
        Heal((maxHealth + intUpgrades[(int) DefaultIntUpgrade.Health]) - health);

        if (max)
            return BuyResult.Max;
        return BuyResult.Success;
    }

    public enum UpgradeList {
        Normal,
        Special
    }

    IEnumerator Regen() {
        while (true) {
            if (intUpgrades[(int) DefaultIntUpgrade.Regeneration] == 0) {
                yield return null;
                continue;
            }
            
            health++;
            yield return new WaitForSeconds(1 / intUpgrades[(int) DefaultIntUpgrade.Regeneration]);
        }
    }

    void OnEnable()
    {     
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // UI = FindObjectOfType<UIController>();
        enemyManager = FindObjectOfType<EnemyManager>();
        controls = new InputMaster(); 
        playerBody = transform.GetChild(1).transform;

        // UI = FindObjectOfType<UIController>();
        
        UIManager.healthUI.maxHealth = _maxHealth;
        UIManager.healthUI.health = health;

        controls.Enable();
        controls.Player.Movement.performed += ctx => { 
            moveVector = ctx.ReadValue<Vector2>(); 
        };
        controls.Player.Movement.canceled += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        
        controls.Player.Fire.performed += (Info) => {
            OnPrimary();
        };
        controls.Player.Secondary.performed += (Info) => {
            OnSecondary();
        };
        controls.Player.Interact.performed += (Info) => {
            Collider2D interactObject = Physics2D.OverlapCircle(transform.position, maxInteractionRange, interactableMask);

            if (interactObject != null)
                interactObject.GetComponent<Interactable>().Interact();
        };

        if (possibleUpgrades == null)
            possibleUpgrades = new List<Upgrade>();

        if (specialUpgrades == null)
            specialUpgrades = new List<Upgrade>();

        possibleUpgradesLevel = new List<int>();
        for (int i = 0; i < possibleUpgrades.Count; i++) {
            possibleUpgradesLevel.Add(0);
        }

        specialUpgradesLevel = new List<int>(specialUpgrades.Count);
        for (int i = 0; i < specialUpgrades.Count; i++) {
            specialUpgradesLevel.Add(0);
        }

        foreach (Upgrade upgrade in possibleUpgrades) {
            if (upgrade is IntUpgrade) {
                intUpgrades[upgrade.Target] = (upgrade as IntUpgrade).Upgrades[0];
            }
            else if (upgrade is FloatUpgrade) {
                floatUpgrades[upgrade.Target] = (upgrade as FloatUpgrade).Upgrades[0];
            }
            //No need for active upgrade prep
        }

        if (intUpgrades == null)
            intUpgrades = new List<int>();

        if (floatUpgrades == null)
            floatUpgrades = new List<float>();

        if (activeUpgrades == null)
            activeUpgrades = new List<List<UnityEvent>>();

        OnDeath += () => {

        };

        enemyManager.OnEnemyDeath.AddListener((Info) => {
            dnaCount += Info.Dna;
            techCount += Info.Tech;
        });

        inertialReferences = new List<Vector2>();
        
        StartCoroutine(UpdateInternalLoop());
        StartCoroutine(FixedUpdateInternalLoop());

        StartCoroutine(Regen());
    }

    void FixedUpdateInternal()
    {
        if (!useDefaultMovement)
            goto EndDefaultMovement;

        if (!((maxSpeed * floatUpgrades[(int) DefaultFloatUpgrade.Speed]) < ((body.velocity - this.GetTotalInertialReference()) + (moveVector * (acceleration * floatUpgrades[(int) DefaultFloatUpgrade.Acceleration]) * Time.fixedDeltaTime)).magnitude))
            body.AddForce((moveVector * (acceleration * floatUpgrades[(int) DefaultFloatUpgrade.Acceleration])));
        
        if (moveVector == Vector2.zero)
            body.drag = idleDrag;
        else
            body.drag = 1;

        EndDefaultMovement:;
    }

    float lastTrueAngle;

    void UpdateInternal() {
        if (!useDefaultAnimator)
            goto EndDefaultAnimation;

        animator.SetFloat("Speed", moveVector.magnitude * maxSpeed, 0.2f, Time.deltaTime);

        EndDefaultAnimation:


        if (!useDefaultRotation)
            goto EndDefaultRotation;

        if (moveVector.magnitude > 0.05f) {
            Vector2 rotVector = moveVector;

            float desiredAngle = Vector2.SignedAngle(Vector2.right, rotVector);
            //DesiredAngle = (DesiredAngle < 0) ? (360 + DesiredAngle) : DesiredAngle;

            float trueAngle = Mathf.MoveTowardsAngle(lastTrueAngle, desiredAngle, maxRotationChange * Time.deltaTime);

            playerBody.localRotation = Quaternion.Euler(0f, 0f, trueAngle);

            if (Mathf.Cos(trueAngle * Mathf.Deg2Rad) < -0.01f) {
                playerBody.GetComponent<SpriteRenderer>().flipY = true;
                flip = true;
            }
            else if (Mathf.Cos(trueAngle * Mathf.Deg2Rad) > 0.01f) {
                playerBody.GetComponent<SpriteRenderer>().flipY = false;
                flip = false;
            }

            lastTrueAngle = trueAngle;
        }
        
        EndDefaultRotation:
        
        // aimPos = controls.Player.AimRelative.ReadValue<Vector2>() * 10;
        aimPos = (Vector2) Camera.main.ScreenToWorldPoint(controls.Player.AimScreen.ReadValue<Vector2>()) - (Vector2) transform.position;
    }
    
    public Player() {
        Enemy.player = this;
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
    Transform weaponPivot {
        get; set;
    }
    Rigidbody2D rb {
        get; set;
    }
    WeaponObject weapon {
        get; set;
    }
    Weapon weaponClass {
        get; set;
    }
}