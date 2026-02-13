using System.Linq;
using UnityEngine;

public class Bat : Ennemy
{
    [SerializeField] public float chaseSpeed;
    [SerializeField] public float fallSpeed;
    [SerializeField] public float attackForce;

    // Animation Data
    public BatIdleState idleState;
    public BatChaseState chaseState;
    public BatAttackState attackState;
    public BatHurtState hurtState;
    public BatFallState fallingState;
    public BatDeathState deathState;
    public string currentState;

    public Collider2D hitboxCollider;

    public override void Start()
    {
        base.Start();

        idleState = new BatIdleState(this, "isIdle");
        chaseState = new BatChaseState(this, "isIdle");
        attackState = new BatAttackState(this, "isAttacking");
        hurtState = new BatHurtState(this, "isHurt");
        fallingState = new BatFallState(this, "isFalling");
        deathState = new BatDeathState(this, "isDead");

        stateMachine.InitializeStateMachine(idleState);
    }

    public override void TakeDamage(float damageAmount, GameObject attacker)
    {
        stateMachine.ChangeState(hurtState);
        base.TakeDamage(damageAmount, attacker);
    }

    public override void Die()
    {
        base.Die();
        hitboxCollider.enabled = false;
        stateMachine.ChangeState(fallingState);
    }

    private void HandleCollision(Collider2D col)
    {
        if (isDead) return;
        Character character = col.GetComponentInParent<Character>();

        if (character == null) return;

        // Attack collision are already in the character script
        Collider2D[] characterHitBoxes = character.attackHitboxes;
        if (characterHitBoxes.Any(hb => hb == col)) return;

        Attack(character);
    }

    public bool IsGrounded()
    {
        return IsTouching(groundCheck, checkRadius, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        HandleCollision(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        HandleCollision(col);
    }

    public void AnimationEvent()
    {
        stateMachine._CurrentState.AnimationEvent();
    }
}
