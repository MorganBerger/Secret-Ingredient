public class GroundAttackState : MovementState
{
    public GroundAttackState(Character _character, string _animationName)
        : base(_character, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.PlayAttackSound();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (isAnimationFinished)
        {
            stateMachine.ChangeState(character.idleState);
            return;
        }
    }
}