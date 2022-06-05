using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    private Animator anim;
    private SpriteRenderer sprite;

    public float jumpForce = 0.0f;
    private float moveInput;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    public float moveSpeed = 0;
    public bool canJump;


    private enum MovementState { idle, running, jumping, falling, croush};

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource groundedSoundEffect;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = 7;
        //mov horizontal
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput*moveSpeed, rb.velocity.y);

        Jump2();
        
        UpdateAnimationState();
    }

    void FixedUpdate(){
        rb.velocity = new Vector2(moveInput*moveSpeed, rb.velocity.y);
    }

    private void UpdateAnimationState(){
        
        MovementState state;

        //cambiar iddle a run
        if(moveInput > 0f){
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if(moveInput < 0f){
            state = MovementState.running;
            sprite.flipX = true;
        }
        else{
            state = MovementState.idle;
        }

        //charging jump
        if(Input.GetKey("space")){
            state = MovementState.croush;
        }
        else if(rb.velocity.y > .1f){
            state = MovementState.jumping;
            moveSpeed = 12;
        }else if(rb.velocity.y < -0.1f){
            state = MovementState.falling;
            moveSpeed = 0;
        }

       anim.SetInteger("state", (int)state);
    }

    void Jump(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetButtonDown("Jump")){
            isJumping=true;
            jumpTime=jumpStartTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if(Input.GetButton("Jump") && isJumping==true){
            if(jumpTime > 0){
                rb.velocity = Vector2.up * jumpForce;
                jumpTime-=Time.deltaTime;
            }else{
                isJumping=false;
            }
        }

        if(Input.GetButtonUp("Jump")){
            isJumping=false;
        }

    }

    void Jump2(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        

        if(Input.GetKey("space") && isGrounded && canJump)
        {
            moveSpeed = 0;
            jumpForce+=0.1f;
        }

        if(Input.GetKeyDown("space") && isGrounded && canJump)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        
        /*if(jumpForce >= 30f && isGrounded)
        {
            float tempx = moveInput * moveSpeed;
            float tempy = jumpForce;
            rb.velocity = new Vector2(tempx, tempy);
            Invoke("ResetJump", 0.2f);
        }*/

        if(Input.GetKeyUp("space"))
        {
            if(jumpForce >= 30f){
                jumpForce=30;
            }

            if(isGrounded)
            {
                rb.velocity = new Vector2(moveInput * moveSpeed, jumpForce);
                jumpForce = 0.0f;
            }
            canJump = true;
            jumpSoundEffect.Play();
        }
    }
/*
    void ResetJump()
    {
        canJump = false;
        jumpForce = 0;
    }
*/
}
