using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, Hittable, IntertalReferenceUser
{

    [Header("Stats")]
    public int Health;
    private Vector2 TargetPos;

    private bool NavErrorReached;

    private bool CanAttack;

    public List<Vector2> InertialReferences {
        get; set;
    }

    [Header("Settings")]
    public int MaxHealth;
    public float Speed;
    public float Acceleration;
    public float MaxNavError;
    public float VelocityCorrection;
    public float AttackRange;
    public bool Rotation;
    public float MaxRotationChange;
    public float AttackCooldown;
    public bool CanAttackOnStart;
    public float KnockbackMultiplier;
    public Vector2Int DnaRange;
    public Vector2Int TechRange;
    public Color32 ConfettiColor;
    bool DisableConfetti;
    public LayerMask PlayerMask;
    public LayerMask WallMask;

    public SpriteRenderer[] ToFlipSprite;
    public Transform[] ToFlipObject;

    [Header("References")]
    protected Rigidbody2D Body;
    protected Animator Animator;

    protected EnemyManager EnemyManager;

    protected void Knockback(float Strength, Vector2 Direction) {
        Body.AddForce(Direction.normalized * Strength, ForceMode2D.Impulse);
    }

    protected bool Detect(Vector2 Position, bool SetNav = true) {
        if (Physics2D.OverlapCircle(transform.position, AttackRange, PlayerMask)) {
            if (Physics2D.Raycast(transform.position, Position - (Vector2) transform.position, (Position - (Vector2) transform.position).magnitude, WallMask).collider == null) {
                if (SetNav)
                    Navigate(Position);
                
                return true;
            }
        }

        return false;
    }

    protected void Navigate(Vector2 Position) {
        TargetPos = Position;
        NavErrorReached = false;
    }

    public OnEnemyDeath OnDeath;

    protected Vector2 MoveVector;

    float LastTrueAngle;

    void UpdateInternal() {
        MoveVector = TargetPos - (Vector2) transform.position;

        if (MoveVector.magnitude < MaxNavError)
            NavErrorReached = true;        

        if (Rotation && !NavErrorReached) {
            Vector2 RotVector = MoveVector;
            
            float DesiredAngle = Vector2.SignedAngle(Vector2.right, RotVector);

            float TrueAngle = Mathf.MoveTowardsAngle(LastTrueAngle, DesiredAngle, MaxRotationChange * Time.deltaTime);
            
            Body.MoveRotation(TrueAngle);

            if (Mathf.Cos(TrueAngle * Mathf.Deg2Rad) < -0.01f) {
                foreach (SpriteRenderer Sprite in ToFlipSprite)
                    Sprite.flipY = true;
                
                foreach (Transform Obj in ToFlipObject)
                    Obj.localScale = new Vector3(1, -1, 1);
            }
            else if (Mathf.Cos(TrueAngle * Mathf.Deg2Rad) > 0.01f) {
                foreach (SpriteRenderer Sprite in ToFlipSprite)
                    Sprite.flipY = false;
                
                foreach (Transform Obj in ToFlipObject)
                    Obj.localScale = new Vector3(1, 1, 1);
            }

            LastTrueAngle = TrueAngle;
        }
    }

    void FixedUpdateInternal() {
        MoveVector = TargetPos - (Vector2) transform.position;

        if (!(Speed < ((Body.velocity - this.GetTotalInertialReference()) + (MoveVector.normalized * Acceleration * Time.fixedDeltaTime)).magnitude) && !NavErrorReached) {
            Body.AddForce(Vector2.Lerp(MoveVector.normalized, MoveVector.normalized - (Body.velocity.normalized - MoveVector.normalized), VelocityCorrection).normalized * Acceleration);
        }
    }

    public bool Hit(int Damage, Vector2 Direction, float Kb, HitType HitType) {
        Health -= Damage;
        if (Health <= 0) {
            if (!DisableConfetti) {
                ParticleSystem Particles = ((GameObject) Instantiate(Resources.Load("Confetti"), transform.position, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, Direction)))).transform.GetChild(0).GetComponent<ParticleSystem>();
                ParticleSystem.MainModule main = Particles.main;
                main.startColor = new ParticleSystem.MinMaxGradient(ConfettiColor);
            }

            OnDeath(new EnemyDeathInfo(gameObject.name, this, transform.position, Random.Range(DnaRange.x, DnaRange.y), Random.Range(TechRange.x, TechRange.y)));

            Destroy(gameObject);
            return true;
        }
        Knockback(Kb * KnockbackMultiplier, Direction);
        return false;
    }

    public bool HitNoConfetti(int Damage, Vector2 Direction, float Kb, HitType HitType) {
        Health -= Damage;
        if (Health <= 0) {
            OnDeath(new EnemyDeathInfo(gameObject.name, this, transform.position, Random.Range(DnaRange.x, DnaRange.y), Random.Range(TechRange.x, TechRange.y)));

            Destroy(gameObject);
            return true;
        }
        Knockback(Kb * KnockbackMultiplier, Direction);
        return false;
    }

    protected bool RequestAttack() {
        if (CanAttack) {
            CanAttack = false;
            StartCoroutine(ResetAttack());
            return true;
        }
        return false;
    }

    IEnumerator ResetAttack() {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    private bool Initialized = false;

    void OnEnable() {
        if (Initialized)
            return;

        Animator = GetComponent<Animator>();
        Body = GetComponent<Rigidbody2D>();
        EnemyManager = FindObjectOfType<EnemyManager>();
        StartCoroutine(UpdateInternalLoop());
        StartCoroutine(FixedUpdateInternalLoop());

        if (CanAttackOnStart)
            CanAttack = true;
        else
            StartCoroutine(ResetAttack());

        OnDeath += (Info) => {
            EnemyManager.OnEnemyDeath.Invoke(Info);
        };
        EnemyManager.RegisterEnemy.Invoke(this);

        InertialReferences = new List<Vector2>();

        TargetPos = transform.position;

        Initialized = true;
    }

    IEnumerator UpdateInternalLoop() {
        while (true) {
            UpdateInternal();
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator FixedUpdateInternalLoop() {
        while (true) {
            yield return new WaitForFixedUpdate();
            FixedUpdateInternal();
        }
    }
}