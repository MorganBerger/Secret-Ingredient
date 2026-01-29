using UnityEngine;

public class Character: MonoBehaviour
{
    public CharacterStateMachine stateMachine { get; private set; }

    public IdleState idleState { get; private set; }
    public RunState runState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public PeakState peakState { get; private set; }

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
        return isTouching(groundCheck, checkRadius, whatIsGround);
    }

    public bool IsTouchingWall()
    {
        return isTouching(wallCheck, checkRadius, whatIsWall);
    }

    public bool isTouching(Transform checkPoint, float checkRadius, LayerMask targetLayer)
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