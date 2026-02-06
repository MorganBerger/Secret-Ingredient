using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public float health = 5f;
    public float damage = 1f;
    public float speed = 2f;

    public StateMachine stateMachine { get; protected set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public LayerMask playerLayer;
    public float detectRange = 1.75f;
    public float attackRange = 0.275f;

    public float checkRadius {
        get { return 0.025f; }
        private set {}
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Start()
    {
        stateMachine = new StateMachine();
    }

    public void Update()
    {
        stateMachine._CurrentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
        stateMachine._CurrentState.PhysicsUpdate();
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

    public Collider2D DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRange, playerLayer);
        
        if (player != null)
        {
            float directionToPlayer = player.transform.position.x - transform.position.x;
            float facingDirection = transform.localScale.x;

            // Check if player is in front of the wolf 
            // (Both must have the same sign: e.g., both positive for Right)
            bool isPlayerInFront = (directionToPlayer > 0 && facingDirection > 0) || 
                                   (directionToPlayer < 0 && facingDirection < 0);

            if (isPlayerInFront) return player;
        }

        return null;
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void Attack(Character character)
    {
        character.TakeDamage(damage, gameObject);
    }

    private void ApplyKnockback(GameObject from)
    {
        Vector2 knockbackDirection = (transform.position - from.transform.position).normalized;
        rb.AddForce(knockbackDirection * 1.5f, ForceMode2D.Impulse);
    }

    public virtual void TakeDamage(float damageAmount, GameObject attacker)
    {
        ApplyKnockback(attacker);

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {}

    void AnimationFinished()
    {
        stateMachine._CurrentState.AnimationTrigger();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.darkCyan;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
