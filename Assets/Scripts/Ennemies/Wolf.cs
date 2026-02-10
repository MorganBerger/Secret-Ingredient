using UnityEngine;

public class Wolf: Ennemy
{
    public float patrolPauseTime = 1f;

    public float chaseSpeed = 1.5f;
    public float patrolDistance = 3.5f;

    public WolfIdleState idleState { get; private set; }
    public WolfRunState runState { get; private set; }
    public WolfPatrolState patrolState { get; private set; }
    public WolfChaseState chaseState { get; private set; }

    public WolfAttackState attackState { get; private set; }

    public WolfHitState hitState { get; private set; }
    public WolfDeathState deathState { get; private set; }
    
    public string currentState;

    public Collider2D attackCollider;

    public override void Start()
    {
        base.Start();

        idleState = new WolfIdleState(this, "isIdle");

        // runState = new WolfRunState(this, "isRunning");
        patrolState = new WolfPatrolState(this, "isRunning");
        chaseState = new WolfChaseState(this, "isRunning");

        attackState = new WolfAttackState(this, "isAttacking");
        deathState = new WolfDeathState(this, "isDead");
        hitState = new WolfHitState(this, "isHurting");

        stateMachine.InitializeStateMachine(idleState);
    }

    public bool IsLedgeAhead() 
    {
        return !IsTouching(groundCheck, checkRadius, whatIsGround);
    }

    public override void TakeDamage(float damageAmount, GameObject attacker)
    {
        stateMachine.ChangeState(hitState);

        base.TakeDamage(damageAmount, attacker);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    private void HandleCollision(Collider2D collision)
    {
        var character = collision.GetComponentInParent<Character>();
        
        if (character == null) return;
        if (health <= 0) return;

        var characterAttackHitBoxes = character.attackHitboxes;

        var isCharacterAttack = false;
        foreach (var hitbox in characterAttackHitBoxes)
        {
            isCharacterAttack = isCharacterAttack || (collision == hitbox);
        }

        if (isCharacterAttack)
        {
            return;
        }

        Attack(character);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollision(collision);
    }
}