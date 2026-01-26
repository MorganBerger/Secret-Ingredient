using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 400f;
    [SerializeField] private bool _jump = false;
    private bool _canDoubleJump = false;
    
    public float moveSpeed = 2f;
    private float horizontalInput;

    [SerializeField] private bool _grounded;

    private Rigidbody2D rb;
    private Animator anim;

    public LayerMask whatIsGround;

    public Transform _groundCheck;
    public Transform _wallCheck;

    public UnityEvent OnLandEvent;

    const float _groundedRadius = .2f;

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
            anim.SetBool("IsJumping", true);
            anim.SetBool("JumpUp", true);
            _grounded = false;
            rb.AddForce(new Vector2(0f, _jumpForce));
            _canDoubleJump = true;
            // particleJumpDown.Play();
            // particleJumpUp.Play();
        }
        _jump = false;
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

        anim.SetBool("Moving", moving);

        if (horizontalInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        Debug.Log("H Input: " + horizontalInput);
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        bool wasGrounded = _grounded;
		_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, _groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				_grounded = true;
				if (!wasGrounded)
				{
					OnLandEvent.Invoke();
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

        Jump();
    }
}
