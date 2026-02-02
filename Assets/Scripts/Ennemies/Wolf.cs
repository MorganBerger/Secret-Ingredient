using UnityEngine;

public class Wolf: Ennemy
{
    public float patrolPauseTime = 1f;

    public float chaseSpeed = 3f;
    public float patrolDistance = 3.5f;

    public WolfIdleState idleState { get; private set; }
    public WolfRunState runState { get; private set; }
    public WolfAttackState attackState { get; private set; }
    public WolfDeathState deathState { get; private set; }

    public Collider2D attackCollider;

    public override void Start()
    {
        base.Start();

        stateMachine = new StateMachine();

        idleState = new WolfIdleState(this, "isIdle");
        runState = new WolfRunState(this, "isRunning");
        attackState = new WolfAttackState(this, "isAttacking");
        deathState = new WolfDeathState(this, "isDead");

        stateMachine.InitializeStateMachine(idleState);
    }

    public bool IsLedgeAhead() 
    {
        return !IsTouching(groundCheck, checkRadius, whatIsGround);;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponentInParent<Character>();
        if (character == null) return;

        var charHitBoxes = character.attackHitboxes;
        var isCharacterAttack = false;
        foreach (var hitbox in charHitBoxes)
            isCharacterAttack = isCharacterAttack || (collision == hitbox);

        if (isCharacterAttack)
        {
            return;
        }

        Attack(character);
    }
}