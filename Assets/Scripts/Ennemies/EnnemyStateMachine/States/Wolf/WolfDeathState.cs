using UnityEngine;

public class WolfDeathState: WolfState
{
    public WolfDeathState(Wolf _wolf, string _animationName)
        : base(_wolf, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        GameObject.Destroy(wolf.gameObject);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        wolf.rb.linearVelocity = Vector2.zero;
    }
}