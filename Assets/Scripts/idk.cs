using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class idk : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    private BoxCollider2D coll;
    private float dirX = 0f;
    
    [SerializeField] private LayerMask jumpableGround;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX*moveSpeed, rb.velocity.y);

        if (Input.GetButton("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
           
        }
    }


    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
*/
public class idk : MonoBehaviour
{
    public Rigidbody2D rb;
    //private float dirX = 0f;
    [SerializeField] private LayerMask whatIsGround;

    public float jumpForce;
    private float moveInput;
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    public float moveSpeed;
    


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput*moveSpeed, rb.velocity.y);

        Jump();
    }
    void FixedUpdate(){
        rb.velocity = new Vector2(moveInput*moveSpeed, rb.velocity.y);
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



    /*

    private bool IsGrounded(){
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }*/




            /*if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Jumping");
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;

        }

        if(Input.GetKey(KeyCode.Space) && isJumping == true){
            Debug.Log("Jumping 2");

            if(jumpTimeCounter > 0){
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }else {
                isJumping = false;
            }
        }

        if(Input.GetKey(KeyCode.Space)){
            Debug.Log("Jumping 3");
            isJumping = false;
        }
        
        jumpSoundEffect.Play();
        
        */
      
      /*  if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }

            else
            {
                isJumping = false;
            }
        }
               
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false; 
        }
        
    */
}