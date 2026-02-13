public class BatHurtState: BatState
{
    public BatHurtState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        stateMachine.ChangeState(bat.idleState);
    }
}