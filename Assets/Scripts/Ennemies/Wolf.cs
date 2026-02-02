using UnityEngine;

public class Wolf: Ennemy
{
    public WolfIdleState idleState { get; private set; }
    public WolfRunState runState { get; private set; }
    public WolfAttackState attackState { get; private set; }

    public override void Start()
    {
        base.Start();

        stateMachine = new StateMachine();

        idleState = new WolfIdleState(this, "isIdle");
        runState = new WolfRunState(this, "isRunning");
        attackState = new WolfAttackState(this, "isAttacking");

        stateMachine.InitializeStateMachine(idleState);
    }
}