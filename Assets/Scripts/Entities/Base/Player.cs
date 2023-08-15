using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Upgrades;

public abstract class Player : MonoBehaviour, Hittable, IntertalReferenceUser, Upgradable
{
    [Header("Stats")]

    [SerializeField]
    private int hp;
    public int health {
        get { return hp; }
        set {
            hp = value;
            UIManager.healthUI.health = value;
            if (hp > maxHealth)
                hp = maxHealth;
            else if (hp <= 0)
                OnDeath();
        }
    }

    public Dictionary<Upgrade, int> upgradeLevels {
        get {
            return lvls;
        }
        set {
            lvls = value;
        }
    }

    public Upgrade[] upgradeList {
        get {
            return upgs;
        }
        set {
            upgs = value;
        }
    }

    Upgrade[] upgs;

    Dictionary<Upgrade, int> lvls = new Dictionary<Upgrade, int>();

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

    private static int dna;
    private static int tech;

    public static int dnaCount {
        get {
            return dna;
        }
        set {
            dna = value;
            UIManager.resourceUI.dna = value;
            // UI.Dna = value;
        }
    }

    public static int techCount {
        get {
            return tech;
        }
        set {
            tech = value;
            UIManager.resourceUI.tech = value;
            // UI.Tech = value;
        }
    }

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

    // IEnumerator Regen() {
    //     while (true) {
    //         if (intUpgrades[(int) DefaultIntUpgrade.Regeneration] == 0) {
    //             yield return null;
    //             continue;
    //         }
            
    //         health++;
    //         yield return new WaitForSeconds(1 / intUpgrades[(int) DefaultIntUpgrade.Regeneration]);
    //     }
    // }

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
        UIManager.resourceUI.dna = dna;
        UIManager.resourceUI.tech = tech;

        controls.Enable();
        controls.Player.Movement.performed += ctx => { 
            moveVector = ctx.ReadValue<Vector2>(); 
        };
        controls.Player.Movement.canceled += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        
        controls.Player.Fire.performed += (Info) => {
            if (Time.timeScale != 0)
                OnPrimary();
        };
        controls.Player.Secondary.performed += (Info) => {
            if (Time.timeScale != 0)
                OnSecondary();
        };
        controls.Player.Interact.performed += (Info) => {
            if (Time.timeScale == 0)
                return;

            Collider2D interactObject = Physics2D.OverlapCircle(transform.position, maxInteractionRange, interactableMask);

            if (interactObject != null)
                interactObject.GetComponent<Interactable>().Interact();
        };
        controls.Menu.Exit.performed += (Info) => {
            MenuController.Exit();
        };

        OnDeath += () => {

        };

        enemyManager.OnEnemyDeath.AddListener((Info) => {
            dnaCount += Info.Dna;
            techCount += Info.Tech;
        });

        inertialReferences = new List<Vector2>();
        
        StartCoroutine(UpdateInternalLoop());
        StartCoroutine(FixedUpdateInternalLoop());

        // StartCoroutine(Regen());
    }

    void FixedUpdateInternal()
    {
        if (!useDefaultMovement)
            goto EndDefaultMovement;

        if (!((maxSpeed) < ((body.velocity - this.GetTotalInertialReference()) + (moveVector * (acceleration) * Time.fixedDeltaTime)).magnitude))
            body.AddForce((moveVector * (acceleration)));
        
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