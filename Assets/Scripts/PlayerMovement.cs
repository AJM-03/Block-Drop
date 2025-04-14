using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float moveInput;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [HideInInspector] public bool isGrounded = true;
    private bool groundedLast = true;
    private bool isMoving = false;
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool inGround = true;
    [HideInInspector] public bool isSelecting = false;
    [HideInInspector] public bool isControlling = false;

    public AudioClip jumpClip;

    public ParticleSystem landParticles;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer rend;
    private AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !groundedLast) landParticles.Play();  // Change colour depending on platform?

        if (!inGround && !isSelecting && !isControlling)
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }

        if (!facingRight && moveInput > 0) Flip();
        else if (facingRight && moveInput < 0) Flip();

        anim.SetBool("Moving", isMoving);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Selecting", isSelecting);
        anim.SetBool("Controling", isControlling);
        anim.SetBool("InGround", inGround);

        if (!isGrounded && rb.velocity.y < 0) anim.SetBool("Falling", true);
        else anim.SetBool("Falling", false);

        if (isSelecting || inGround) MusicController.instance.ChangeVersion(1);
        else if (isControlling || !isGrounded) MusicController.instance.ChangeVersion(2);
        else MusicController.instance.ChangeVersion(3);

        groundedLast = isGrounded;
    }

    private void OnMovement(InputValue value)
    {
        if (!isSelecting && !isControlling)
        {
            float input = value.Get<Vector2>().x;
            if (input > 0) moveInput = 1;
            else if (input < 0) moveInput = -1;
            else moveInput = 0;

            if (moveInput != 0) isMoving = true;
            else isMoving = false;
        }
    }

    private void OnJump(InputValue value)
    {
        if (isGrounded && !isSelecting && !isControlling)
        {
            rb.velocity = Vector2.up * jumpForce;

            source.PlayOneShot(jumpClip);

            if (inGround) inGround = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        rend.flipX = !facingRight;
    }
}
