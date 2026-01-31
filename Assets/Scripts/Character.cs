using NUnit.Framework;
using UnityEngine;

public class Character: MonoBehaviour
{
    public CharacterStateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public PeakState peakState { get; private set; }
    public WallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }
    public DoubleJumpState doubleJumpState { get; private set; }

    public bool canDoubleJump { get; set; }

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    public Transform groundCheck;
    public Transform wallCheck;

    public float checkRadius {
        get { return 0.025f; }
        private set {}
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        stateMachine = new CharacterStateMachine();

        idleState = new IdleState(this, "isIdle");
        runState = new RunState(this, "isRunning");
        jumpState = new JumpState(this, "isJumping");
        fallState = new FallState(this, "isFalling");
        peakState = new PeakState(this, "isPeaking");
        wallSlideState = new WallSlideState(this, "isWallSliding");
        wallJumpState = new WallJumpState(this, "isJumping");
        doubleJumpState = new DoubleJumpState(this, "isJumping");

        stateMachine.InitializeStateMachine(idleState);
    }

    void Update()
    {
        stateMachine._CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        stateMachine._CurrentState.PhysicsUpdate();
    }

    public bool IsTouchingGround()
    {
        var isTouching = IsTouching(groundCheck, checkRadius, whatIsGround);
        return isTouching;
    }

    public bool IsTouchingWall()
    {
        var isTouching = IsTouching(wallCheck, checkRadius, whatIsWall);
        return isTouching;
    }

    public bool IsTouching(Transform checkPoint, float checkRadius, LayerMask targetLayer)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPoint.position, checkRadius, targetLayer);
        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheck.position, checkRadius);
    }
}