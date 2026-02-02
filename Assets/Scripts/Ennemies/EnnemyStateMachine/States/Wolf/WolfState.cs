public class WolfState: State
{
    protected Wolf wolf;

    public WolfState(Wolf _wolf, string _animationName)
    {
        wolf = _wolf;
        stateMachine = _wolf.stateMachine;
        animator = _wolf.anim;
        animationName = _animationName;
    }
}