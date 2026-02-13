public class BatState: State
{
    protected Bat bat;

    public BatState(Bat _bat, string _animationName) 
    : base(_bat.stateMachine, _bat.anim, _animationName)
    {
        bat = _bat;
    }

    public override void Enter()
    {
        base.Enter();
        bat.currentState = GetType().Name;
    }
}