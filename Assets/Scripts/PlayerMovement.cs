using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    private Animator anim;
    private SpriteRenderer sprite;

    Scene scene;
    public float jumpForce = 0.0f;
    private float moveInput;
    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public float jumpStartTime;
    private float jumpTime;
    private bool isJumping;
    public float moveSpeed = 0;
    public float hp=100;
    public float yPositionJumping, yPositionGrounding;
    public bool portalCheckpoint=false;

    private enum MovementState { idle, running, jumping, falling, croush};

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource portalSoundEffect;
    [SerializeField] private Slider hp_slider;
    [SerializeField] private Slider jump_slider;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Started");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        scene = SceneManager.GetActiveScene();
        jumpForce=0;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed = 7;
        moveInput = Input.GetAxisRaw("Horizontal");//mov horizontal
        rb.velocity = new Vector2(moveInput*moveSpeed, rb.velocity.y);
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


        Jump();
        UpdateAnimationState();
        if(scene.name == "Level2" || scene.name == "Level1"){
            checkHP();
            updateSliders();
        }

        if(isGrounded==true){
            CheckJumpHit();
        }
        if(rb.velocity.y < -0.1f && yPositionJumping==0){
            yPositionJumping = rb.position.y;
        }

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
        if(Input.GetKey("space") && isGrounded)
        {
            moveSpeed = 0;
            if(jumpForce<30){
                jumpForce+=0.05f;
            }
        }

        if(Input.GetKeyDown("space") && isGrounded)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
        

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
            jumpSoundEffect.Play();
        }

    }

    public void CheckJumpHit(){
        yPositionGrounding = rb.position.y;
        if(yPositionJumping!=0){
            if((yPositionJumping-yPositionGrounding)>12){
                hp-=(int)((yPositionJumping-yPositionGrounding)/2);
            }
            yPositionGrounding=0;
            yPositionJumping=0;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ){
        //MENU
        if(collision.gameObject.tag == "Trigger1" && scene.name == "MENU"){
            SceneManager.LoadScene("Level1");
        }
        if(collision.gameObject.tag == "Trigger2" && scene.name == "MENU"){
            SceneManager.LoadScene("Level2");
        }
        //LEVEL 1
        if(collision.gameObject.tag == "Goal1"){
            SceneManager.LoadScene("Level2");
        }
        //LEVEL 2
        if(collision.gameObject.tag == "DiamondPortal" && scene.name == "Level2"){
            rb.position = new Vector2(53.7f, 3.09f);
            portalSoundEffect.Play();
            hp+=32;
            portalCheckpoint=true;
        }
        if(collision.gameObject.tag == "Fire"){
           hp=0;
        }
        if(collision.gameObject.tag == "Goal2"){
            SceneManager.LoadScene("FIN");
        }

    }

    private void checkHP(){
        if(hp<=0 && scene.name == "Level1"){
            SceneManager.LoadScene("Level1");
        }
        if(hp<=0 && scene.name == "Level2" && portalCheckpoint==true){
            rb.position = new Vector2(53.7f, 3.09f);
            portalSoundEffect.Play();
            hp+=50;
        }
        else if(hp<=0 && scene.name == "Level2"){
            rb.position = new Vector2(-23f, 7.1f);
            hp=100;
        }
    }

    private void updateSliders(){
        hp_slider.value=hp;
        jump_slider.value=jumpForce;
    }


}
