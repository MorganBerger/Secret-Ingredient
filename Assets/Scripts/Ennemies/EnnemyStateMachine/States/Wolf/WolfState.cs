using Unity.VisualScripting;

public class WolfState: State
{
    protected Wolf wolf;

    public WolfState(Wolf _wolf, string _animationName) 
    : base(_wolf.stateMachine, _wolf.anim, _animationName)
    {
        wolf = _wolf;
    }

    public override void Enter()
    {
        base.Enter();
        wolf.currentState = GetType().Name;
    }
}