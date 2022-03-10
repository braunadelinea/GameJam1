/*****************************************
 * Edited by: Ryan Scheppler
 * Last Edited: 1/27/2021
 * Description: This should be added to the player in a simple 2D platformer 
 * *************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    //speed and movement variables
    public float speed;
    public float airSpeed;
    private float moveInputH;
    //grab this to adjust physics
    private Rigidbody2D myRb;

    //used for checking what direction to be flipped
    private bool facingRight = true;

    //things for ground checking
    private bool isGrounded = false;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //jump things
    public int extraJumps = 1;
    private int jumps;
    public float jumpForce;
    private bool jumpPressed = true;

    private float jumpTimer = 0;
    public float jumpTime = 0.2f;

    public float gravityScale = 5;

    public float groundDrag = 5;
    public float airDrag = 1;

    //ladder things
    private bool isClimbing;
    public LayerMask whatIsLadder;
    public float ladderDist;
    private float moveInputV;
    public float climbSpeed;

    //jetpack things
    private bool jetpackOn = false;
    [SerializeField] private float jetPackForce = 0.5f;
    [SerializeField] private float jetpackMaxFuel = 15.0f;
    [SerializeField] private float jetpackFuelLossRate = 2.0f;
    [SerializeField] private float jetpackFuelRecoveryRate = 0.5f;

    private float jetpackFuel = 0.0f;
    private bool canUseJetpack = true;

    // jetpack cooldown bar things
    public GameObject jetpackFuelBar;
    public GameObject jetpackMaxFuelBar;

    //Respawn info
    [HideInInspector]
    public Vector3 RespawnPoint = new Vector3();

    //animation
    private Animator myAnim;
    [SerializeField] private Animator sauceAnim;

    public AudioSource myAud;
    public AudioClip tortiCollision;
    public AudioClip jumpNoise;
    public AudioClip tortiStabbed;

    public AudioSource jetpackAudioSource;
    public AudioClip jetpackNoise;

    // Fork
    [SerializeField] private Fork fork;
    
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        jumps = extraJumps;

        RespawnPoint = transform.position;

        jetpackFuel = jetpackMaxFuel;

        UpdateFuelBar();
        
        sauceAnim.SetInteger("SauceColor", 1);
    }

    //Update is called once per frame
    private void Update()
    {

        moveInputH = Input.GetAxisRaw("Horizontal");
        if (isGrounded == true)
        {
            jumps = extraJumps;
            canUseJetpack = true;
        }
        //check if jump can be triggered
        if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && isGrounded == true && isClimbing == false && jetpackOn == false)
        {
            myAud.PlayOneShot(jumpNoise);
            myRb.drag = airDrag;
            if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
            {
                myRb.velocity = (Vector2.up * jumpForce);
            }
            else
            {
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            }
            jumpPressed = true;
        }
        else if (Input.GetAxisRaw("Jump") == 1 && jumpPressed == false && jumps > 0 && isClimbing == false && jetpackOn == false)
        {
            myAud.PlayOneShot(jumpNoise);
            myRb.drag = airDrag;
            if ((myRb.velocity.x < 0 && moveInputH > 0) || (myRb.velocity.x > 0 && moveInputH < 0))
            {
                myRb.velocity = (Vector2.up * jumpForce);
            }
            else
            {
                myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            }
            jumpPressed = true;
            jumps--;
        }
        else if(Input.GetAxisRaw("Jump") == 0)
        {
            jumpPressed = false;
            jumpTimer = 0;
        }
        else if(jumpPressed == true && jumpTimer < jumpTime)
        {
            jumpTimer += Time.deltaTime;
            myRb.drag = airDrag;
            myRb.velocity = (Vector2.up * jumpForce) + new Vector2(myRb.velocity.x, 0);
            jumpPressed = true;
        }

        // jetpack things
        if (Input.GetAxisRaw("Jetpack") == 1 && jetpackFuel > 0 && canUseJetpack)
        {
            if (!jetpackOn)
            {
                jetpackAudioSource.Play();
            }
            jetpackOn = true;
            jetpackFuel -= Time.deltaTime * jetpackFuelLossRate;
        }
        else
        {
            if (jetpackOn)
            {
                jetpackAudioSource.Stop();
            }

            jetpackOn = false;

            if (jetpackFuel < jetpackMaxFuel / 5)
            {
                canUseJetpack = false;
                //change color ?? maybe
            }

            if (jetpackFuel > jetpackMaxFuel / 5)
            {
                canUseJetpack = true;
            }

            if (jetpackFuel < jetpackMaxFuel)
            {
                jetpackFuel += Time.deltaTime * jetpackFuelRecoveryRate;
            }
        }
        
        UpdateFuelBar();
    }

    void UpdateFuelBar() {
        // jetpack cooldown bar
        Vector3 blackScale = jetpackFuelBar.gameObject.transform.localScale;
        blackScale.x = (jetpackFuel / jetpackMaxFuel) * jetpackMaxFuelBar.gameObject.transform.localScale.x;
        jetpackFuelBar.gameObject.transform.localScale = blackScale;
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        //check for ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //set animators on ground
        myAnim.SetBool("OnGround", isGrounded);

        //ladder things

        moveInputV = Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Jump");
        //check for the ladder if around the player
        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheck.position, Vector2.up, ladderDist, whatIsLadder);
        
        //if ladder was found see if we are climbing, stop falling
        if (hitInfo.collider != null)
        {
            myRb.gravityScale = 0;
            isClimbing = true;
            if(moveInputV > 0)
            {
                myRb.AddForce(new Vector2(0, climbSpeed));
            }
            else if(moveInputV < 0)
            {
                myRb.AddForce(new Vector2(0, -climbSpeed));
            }
            else
            {
                myRb.velocity = new Vector2(myRb.velocity.x, 0);
            }
        }
        else
        {
            myRb.gravityScale = gravityScale;
            isClimbing = false;
        }

        //horizontal movement
        moveInputH = Input.GetAxisRaw("Horizontal");
        //animator settings
        if(moveInputH == 0)
        {
            myAnim.SetBool("Moving", false);
        }
        else
        {
            myAnim.SetBool("Moving", true);
        }

        if (isGrounded && !jumpPressed || isClimbing)
        {
            myRb.drag = groundDrag;
            myRb.AddForce(new Vector2(moveInputH * speed , 0));
        }
        else
        {
            myRb.drag = airDrag;
            myRb.AddForce(new Vector2(moveInputH * airSpeed  , 0));
        }
        //check if we need to flip the player direction
        if (facingRight == false && moveInputH > 0)
            Flip();
        else if(facingRight == true && moveInputH < 0)
        {
            Flip();
        }

        // jetpack stuff
        myAnim.SetBool("Flying", jetpackOn);
        sauceAnim.SetBool("Saucing", jetpackOn);

        if (jetpackOn)
        {
            if (myRb.velocity.y < 7)
            {
                myRb.velocity += (Vector2.up * jetPackForce * Time.deltaTime * 150);
            }
        }

    }
    //flip the player so sprite faces the other way
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // OnCollisionStay instead of Trigger so that the oven trap will kill when swapping tags
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            myAud.PlayOneShot(tortiCollision);
            myRb.velocity = Vector2.zero;
            transform.position = RespawnPoint;
            
            if (collision.gameObject.layer == LayerMask.NameToLayer("Fork"))
            {
                print("Stabbed Torti :(");
                fork.setTortiStabbed();
                // torti is hit
                myAud.PlayOneShot(tortiStabbed);
            }
        }
    }
}
