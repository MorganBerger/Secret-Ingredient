using UnityEngine;

public class BatDeathState: BatState
{
    public BatDeathState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        GameObject.Destroy(bat.gameObject);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        bat.rb.linearVelocity = Vector2.zero;
    }
}