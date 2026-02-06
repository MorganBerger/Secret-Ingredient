using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public struct CharacterSkills
{
    public static bool canDash = true;
    public static bool canDoubleJump = true;
    public static bool canWallClimb = true;
}

public class Character: MonoBehaviour
{
    public CharacterStateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public PeakState peakState { get; private set; }
    public WallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public DoubleJumpState doubleJumpState { get; private set; }
    public DashState dashState { get; private set; }
    public DrinkState drinkState { get; private set; }
    public GroundAttackState groundAttackState { get; private set; }
    public AirAttackState airAttackState { get; private set; }
    public HurtState hurtState { get; private set; }
    public DeathState deathState { get; private set; }

    public bool canDoubleJump { get; set; }
    public bool canDash { get; set; }

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public Transform groundCheck;
    public Transform wallCheck;

    public float health = 3;
    public float speed = 2f;
    public float attackSpeed = 1f;
    public float damage = 1f;

    public float checkRadius {
        get { return 0.025f; }
        private set {}
    }

    public Collider2D[] attackHitboxes;
    private bool canTakeDamage = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        stateMachine = new CharacterStateMachine();

        idleState = new IdleState(this, "isIdle");
        runState = new RunState(this, "isRunning");
        jumpState = new JumpState(this, "isJumping");
        fallState = new FallState(this, "isFalling");
        peakState = new PeakState(this, "isPeaking");
        wallSlideState = new WallSlideState(this, "isWallSliding");
        wallJumpState = new WallJumpState(this, "isJumping");
        doubleJumpState = new DoubleJumpState(this, "isJumping");
        dashState = new DashState(this, "isDashing");
        drinkState = new DrinkState(this, "isDrinking");
        groundAttackState = new GroundAttackState(this, "isAttacking");
        airAttackState = new AirAttackState(this, "isAirAttacking");
        hurtState = new HurtState(this, "isHurting");
        deathState = new DeathState(this, "isDead");

        stateMachine.InitializeStateMachine(idleState);
    }

    void Update()
    {
        stateMachine._CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        stateMachine._CurrentState.PhysicsUpdate();
    }

    public bool isDead()
    {
        return health <= 0;
    }

    public bool IsTouchingGround()
    {
        var isTouching = IsTouching(groundCheck, checkRadius, whatIsGround);
        return isTouching;
    }

    public bool IsTouchingWall()
    {
        var isTouching = IsTouching(wallCheck, checkRadius, whatIsWall);
        return isTouching;
    }

    public bool IsTouching(Transform checkPoint, float checkRadius, LayerMask targetLayer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPoint.position, checkRadius, targetLayer);
        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public void ConsumeItem(Items item, bool playAnimation = true)
    {
        if (item == null) return;

        if (playAnimation) stateMachine.ChangeState(drinkState);
        switch (item.consumableType)
        {
            case ConsumableType.HealthUp:
                health += .5f;
                break;
            case ConsumableType.MediumHealthUp:
                health += 1;
                break;
            case ConsumableType.BigHealthUp:
                health += 2;
                break;
            case ConsumableType.HealthDown:
                health -= 1;
                break;
            case ConsumableType.SpeedUp:
                speed += 0.2f;
                break;
            case ConsumableType.SpeedDown:
                speed -= 0.2f;
                break;
            case ConsumableType.AttackSpeedUp:
                attackSpeed += 0.2f;
                break;
            case ConsumableType.AttackSpeedDown:
                attackSpeed -= 0.2f;
                break;
            case ConsumableType.DamageUp:
                damage += 0.5f;
                break;
            case ConsumableType.DamageDown:
                damage -= 0.5f;
                break;
            case ConsumableType.Dash:
                CharacterSkills.canDash = true;
                break;
            case ConsumableType.DoubleJump:
                CharacterSkills.canDoubleJump = true;
                break;
            case ConsumableType.ClawHook:
                CharacterSkills.canWallClimb = true;
                break;
            case ConsumableType.Random:
                // Apply a random effect
                Array values = Enum.GetValues(typeof(ConsumableType));
                System.Random random = new();
                ConsumableType randomEffect = (ConsumableType)values.GetValue(random.Next(values.Length - 1));
                Items randomItem = ScriptableObject.CreateInstance<Items>();
                randomItem.consumableType = randomEffect;
                ConsumeItem(randomItem, false);
                break;
            case ConsumableType.None:
                break;
            default:
                break;

        }
        InventoryManager.Instance.RemoveItem(item, 1);
        FindFirstObjectByType<InventoryGrid>().RefreshInventory();
    }

    void AnimationFinished()
    {
        stateMachine._CurrentState.AnimationTrigger();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (stateMachine._CurrentState != groundAttackState && stateMachine._CurrentState != airAttackState)
        {
            return;
        }

        var touching = false;
        foreach (var hitbox in attackHitboxes)
            touching = touching || hitbox.IsTouching(collision);
        
        if (!touching)
        {
            return;
        }

        var ennemy = collision.GetComponentInParent<Ennemy>();
        if (ennemy != null)
        {
            ennemy.TakeDamage(damage, gameObject);

            if (ennemy.health > 0) 
            {
                ApplyKnockback(ennemy.gameObject, force: .6f);
            }
        }
    }

    public void TakeDamage(float damageAmount, GameObject attacker)
    {
        if (isDead()) return;
        if (!canTakeDamage) return;

        canTakeDamage = false;
        StartCoroutine(TakeDamageCooldown());

        health -= damageAmount;

        spriteRenderer.color = new Color(1, 1, 1, health <= 0 ? 1f : 0.75f);
  
        stateMachine.ChangeState(hurtState);

        ApplyKnockback(attacker);
    }

    IEnumerator TakeDamageCooldown()
    {
        yield return new WaitForSeconds(1f);

        spriteRenderer.color = new Color(1, 1, 1, 1f);
        canTakeDamage = true;
    }

    private void ApplyKnockback(GameObject from, float force = 2f)
    {
        Vector2 knockbackDirection = (transform.position - from.transform.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * force, ForceMode2D.Impulse);
    }
}