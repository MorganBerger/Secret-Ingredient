using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 400f;
    [SerializeField] private bool _jump = false;
    [SerializeField] private bool _isWalling = false; //If there is a wall in front of the player
	private bool _isWallSliding = false; //If player is sliding in a wall
	private bool _oldWallSlidding = false; //If player is sliding in a wall in the previous frame

    private bool _canDoubleJump = false;
    
    public float moveSpeed = 2f;
    private float horizontalInput;

    private float prevVelocityX = 0f;

    [SerializeField] private bool _grounded;

    private Rigidbody2D rb;
    private Animator anim;

    public LayerMask whatIsGround;

    public Transform _groundCheck;
    public Transform _wallCheck;

    public UnityEvent OnLandEvent;

    const float _groundedRadius = .025f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Jump()
    {
        if (_grounded && _jump)
        {
            // Add a vertical force to the player.

            Debug.Log("Jumpiiiing");

            anim.SetBool("IsJumping", true);
            anim.SetBool("JumpUp", true);
            _grounded = false;
            rb.AddForce(new Vector2(0f, _jumpForce));
            _canDoubleJump = true;
            // particleJumpDown.Play();
            // particleJumpUp.Play();
        }
        _jump = false;

        // Debug.Log("Vertical Velocity: " + rb.linearVelocity.y);

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = 0f;
        bool moving = false;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
            moving = true;
        } 
        
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
            moving = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
		{
			_jump = true;
		}

        if (Input.GetKeyUp(KeyCode.Z) && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }

        if (rb.linearVelocity.y <= 0.1f && !_grounded)
        {
            anim.SetBool("JumpUp", false);
            // This is a great place to trigger a "Fall" animation if you have one
        }

        // anim.SetBool("Moving", moving);

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        // Debug.Log("H Input: " + horizontalInput);
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        bool wasGrounded = _grounded;
		_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
            {
				_grounded = true;
                Debug.Log("Grounded");

				if (!wasGrounded && !anim.GetBool("JumpUp"))
				{
					// OnLandEvent.Invoke();

                    Debug.Log("Landed");
                    anim.SetBool("IsJumping", false);
					// if (!m_IsWall && !isDashing) 
                    // {
						// particleJumpDown.Play();
                    // }
					_canDoubleJump = true;
					if (rb.linearVelocity.y < 0f)
                    {
						// limitVelOnWallJump = false;
                    }
				}
            }
		}

        _isWalling = false;

		if (!_grounded)
		{
			// OnFallEvent.Invoke();
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(_wallCheck.position, _groundedRadius, whatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != null)
				{
					// isDashing = false;
					_isWalling = true;
				}
			}
			prevVelocityX = rb.linearVelocity.x;
		}

        // if (!oldWallSlidding && m_Rigidbody2D.velocity.y < 0 || isDashing)
        // {
        //     isWallSliding = true;
        //     m_WallCheck.localPosition = new Vector3(-m_WallCheck.localPosition.x, m_WallCheck.localPosition.y, 0);
        //     Flip();
        //     StartCoroutine(WaitToCheck(0.1f));
        //     canDoubleJump = true;
        //     animator.SetBool("IsWallSliding", true);
        // }
        // isDashing = false;

        Jump();
    }

    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundedRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_wallCheck.position, _groundedRadius);
    }

    #endregion
}

