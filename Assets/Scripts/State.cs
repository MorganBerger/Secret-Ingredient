using UnityEngine;

public class State
{
    protected StateMachine stateMachine;

    protected Animator animator;
    protected string animationName;

    protected bool isExitingState;
    protected bool isAnimationFinished;
    protected float startTime;

    public State(StateMachine _stateMachine, Animator _animator, string _animationName)
    {
        stateMachine = _stateMachine;
        animator = _animator;
        animationName = _animationName;
    }

    public virtual void Enter()
    {
        isAnimationFinished = false;
        isExitingState = false;
        startTime = Time.time;
        animator.SetBool(animationName, true);
    }
    public virtual void Exit()
    {
        isExitingState = true;
        if (!isAnimationFinished) isAnimationFinished = true;
        animator.SetBool(animationName, false);
    }
    public virtual void LogicUpdate()
    {
        TransitionChecks();   
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void TransitionChecks()
    {
   
    }
    
    public virtual void AnimationTrigger()
    {
        isAnimationFinished = true;
    }
}

