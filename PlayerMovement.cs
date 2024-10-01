using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private LayerMask jumpableGround;
    private Animator anim;
    private SpriteRenderer sprite;
    private float dirx = 0f;
    [SerializeField] private float BASE_SPEED = 7f;
    [SerializeField] private float jumpForce = 14f;
    private bool double_jump = false;
    private bool shift_pressed = false;

    private float moveSpeed;

    private enum MovementState {idle, running, jumping, falling};

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        moveSpeed = BASE_SPEED;
    }

    // Update is called once per frame
    private void Update()
    {

        dirx = Input.GetAxisRaw("Horizontal");
        if (Input.GetButton("Sprint"))
        {
            shift_pressed = true;
            if (moveSpeed == BASE_SPEED)
            {
                moveSpeed = BASE_SPEED * 2;
            }
        }
        else
        {
            shift_pressed = false;
            moveSpeed = BASE_SPEED;
        }
        rb.velocity = new Vector2(dirx * moveSpeed, rb.velocity.y);
        


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (Input.GetButtonDown("Jump") && HasJumped() && !double_jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            double_jump = true;
        }

        if (double_jump && IsGrounded())
        {
            double_jump = false;
        }


        UpdateAnimationState();
    }
    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirx > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirx < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }
        //higher priority
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        anim.SetInteger("state", (int)state);

    }
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private bool HasJumped()
    {
        return !IsGrounded();
    }
}
