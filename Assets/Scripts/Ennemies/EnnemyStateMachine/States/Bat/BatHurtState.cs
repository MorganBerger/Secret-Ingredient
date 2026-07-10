public class BatHurtState: BatState
{
    public BatHurtState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(bat.idleState);
    }
}