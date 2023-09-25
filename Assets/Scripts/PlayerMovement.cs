using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private BoxCollider2D playerBC;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float xDir = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSound;

    // Start is called before the first frame update
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerBC = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        xDir = Input.GetAxisRaw("Horizontal");

        playerRB.velocity = new Vector2(xDir * moveSpeed, playerRB.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            jumpSound.Play();
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;
        if (xDir > 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (xDir < 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if (playerRB.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (playerRB.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(playerBC.bounds.center, playerBC.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
