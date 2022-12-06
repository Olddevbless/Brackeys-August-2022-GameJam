using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [Header("Movement")]
    public float speed = 1;
    public bool isSlowed;
    public float slow=1;
    [SerializeField] float horizontal;
     public bool playerIsMoving;
    [SerializeField] bool isFrozen;
    RigidbodyConstraints originalConstraints;
    Rigidbody playerRB;
    GameObject playerModel;

    [Header("Jumping")]
    [SerializeField] float gravity = -9.81f; // test
    [SerializeField] float jumpForce = 10;
    [SerializeField] bool isGrounded;
    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField]  float coyoteTimeCounter;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField]  float jumpBufferCounter;
    
   

    [Header("Crouching")]

    [SerializeField] float crouchSpeed = 2;
    [SerializeField] float crouchHeight;
    [SerializeField] float centerChange;
    
    
    [Header("Interactions")]

    [SerializeField] FlashLight flashLight;
    [SerializeField] bool noteReading;
    [SerializeField] bool notePrompt;
    [SerializeField] GameObject notePromptCanvas;
    public bool isDead;
    GameManager gameManager;
    [SerializeField] bool grabBoulder = true;
    Transform playerHands;
    [SerializeField] FollowPlayer doggo;
    public bool isGrabable;
    public GameObject touchObject;
    private GameObject noteTouch;

    [Header("Climbing")]
    //[SerializeField] bool onWall;
    [SerializeField] bool onRightWall;
    [SerializeField] bool onLeftWall;
    [SerializeField] float rayHeightOffset;
    [SerializeField] float climbSpeed;
    int layerID = 8;
    LayerMask scalableWallsMask;

    //[Header("SFX")]
    //[SerializeField] AudioSource playerAudio;
    //[SerializeField] AudioClip walkingSurfaceSFX;
    
    



    private void Awake()
    {
        //playerAudio = this.GetComponent<AudioSource>();
        notePromptCanvas = GameObject.Find("NotePrompt");
        playerHands = transform.Find("PlayerHands");
        playerModel = GameObject.Find("PlayerModel");
        gameManager = FindObjectOfType<GameManager>();
        flashLight = FindObjectOfType<FlashLight>();
        scalableWallsMask = (1 << layerID);
        playerRB = this.GetComponent<Rigidbody>();
        doggo = FindObjectOfType<FollowPlayer>();

    }
    private void Start()
    {
        slow = 1;
        isSlowed = false;
        originalConstraints = playerRB.constraints;
        isFrozen = false;
        isDead = false;
    }
    private void OnTriggerStay(Collider other)
    {
            isGrabable = true;
            if (other.CompareTag("Boulder") )
            {
            touchObject = other.gameObject;
            if (playerHands.childCount == 0)
                    {
                        grabBoulder = true;
                        playerAnimator.SetBool("isPushing", true);
                    }
                    else
                    {
                        grabBoulder = false;
                        playerAnimator.SetBool("isPushing", false);
                    }
            }
            if (other.CompareTag("Note"))
            {
                notePrompt = true;
                noteTouch = other.gameObject;
            }
            if (other.CompareTag("Mud"))
        {
            isSlowed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        notePrompt = false;
        if (other.CompareTag("Boulder") && other.gameObject == touchObject)
        {
            touchObject = null;
            grabBoulder = false;
        }
        if(other.CompareTag("Note") && other.gameObject == noteTouch)
        {
            
            noteTouch = null;
        }
        if (other.CompareTag("Mud"))
        {
            isSlowed = false;
        }
    }
    private void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
       if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
       else
        {
            coyoteTimeCounter -= Time.deltaTime;
            
        }
       if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
            
        }
       if (!isFrozen)
        {
            playerRB.constraints = originalConstraints;
        }
       if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.ToggleLight();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            doggo.Stay();
            
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            doggo.ComeHere();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            doggo.Bark();
        }
        
        if (OnAnyWall()&&Input.GetKey(KeyCode.R))
        {
            playerRB.useGravity = false;

            
        }
        else
        {
            ApplyGravity();
            playerRB.useGravity = true;
        }
        if (Input.GetKeyDown(KeyCode.E)&& noteReading)
        {
            noteReading = false;
        }
        if (noteReading)
        {
            Time.timeScale = 0;
        }
        if (!gameManager.pauseMenuIsActive|| !noteReading)
        {
            Time.timeScale =1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!grabBoulder && playerHands.transform.childCount !=0)
            {
                //touchObject.transform.SetParent(null);

                grabBoulder = false;
                isSlowed = false;
                playerHands.DetachChildren();

            }
            if (grabBoulder)
            {
                isSlowed = true;
                touchObject.transform.SetParent(playerHands);
            }

            if(noteTouch!=null)
            {
                Debug.Log("reading" + noteTouch.gameObject.name);
                noteReading = !noteReading;
                noteTouch.GetComponent<Note>().ReadNote();
                noteTouch = null;
            }

        }
        if (notePrompt)
        {
            notePromptCanvas.SetActive(true);
        }
        if (!notePrompt && notePromptCanvas != null)
        {
            notePromptCanvas.SetActive(false);
        }
        if(playerHands.transform.childCount!=0 && grabBoulder==false)
        {
            if(Vector3.Distance(playerHands.transform.GetChild(0).transform.position,playerHands.transform.position) >=4)
            {
                //touchObject.transform.SetParent(null);

                grabBoulder = false;
                isSlowed = false;
                playerHands.DetachChildren();
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if(runing== false)
            {

            }
        }
        if (playerIsMoving)
        {
            playerAnimator.SetBool("isRunning", true);

        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
        OnWall();
        Movement();
        Jump();
        Crouching();
        
    }
    bool runing = false;
    public void Jump()
    {
        if (jumpBufferCounter>0 && coyoteTimeCounter>0&& playerHands.transform.childCount == 0)
        {
            playerRB.AddForce(Vector3.up *jumpForce, ForceMode.Impulse);
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;
            playerAnimator.SetTrigger("Jump");
        }
    }
    void Crouching()
    {
        Vector3 capsuleColliderCenter = gameObject.GetComponent<CapsuleCollider>().center;

        if (Input.GetKey(KeyCode.C))
        {
            playerAnimator.SetBool("isCrouching", true);
            speed = crouchSpeed;
            gameObject.GetComponent<CapsuleCollider>().height = crouchHeight; // set collider height to crouch height

            //animator.SetBool("IsCrouching", true); // start animation

            if (gameObject.GetComponent<CapsuleCollider>().center.y > centerChange) // change the center of the collider to match crouch hitbox
            {
                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(capsuleColliderCenter.x, capsuleColliderCenter.y + centerChange, capsuleColliderCenter.z);

            }
        }
        else
        {
            playerAnimator.SetBool("isCrouching", false);
            gameObject.GetComponent<CapsuleCollider>().height = crouchHeight * 2;
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(capsuleColliderCenter.x, 0f, capsuleColliderCenter.z);
            speed = 5;
        }
    }

            void Movement()
    {
        if (isSlowed)
        {
            slow = 0.5f;

          
          if (slow>=1)
            {
                isSlowed = false;
            }
            
        }
        if (slow < 1)
        {
            slow += 0.1f * Time.deltaTime;
        }
        var horizontalInput = Input.GetAxisRaw("Horizontal");

        var direction = Vector3.right * horizontalInput;
        direction *= Time.deltaTime;

        transform.Translate(direction * speed*slow);

        playerIsMoving = horizontalInput != 0;
        
        
        
        if (horizontalInput > 0)
        {
            playerModel.transform.rotation=Quaternion.Euler(new Vector3(0,0,0));
        }
        if (horizontalInput<0)
        {
            playerModel.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

    }
    public void ApplyGravity()
    {
        if (OnAnyWall())
        {
            return;
        }
        if (!OnAnyWall())
        {
            playerRB.velocity += Vector3.down * gravity * Time.deltaTime;
        }

    }
   
    public void FreezeMovement()
    {
        //StopCoroutine(GameManager.current.Notice(""));
        //StartCoroutine(GameManager.current.Notice("Freezed"));
        isFrozen = true;
        playerRB.constraints = RigidbodyConstraints.FreezeAll;
        
        if (!isDead)
        {
            Invoke("UnfreezeMovement", 2f);
        }
        
    }
    public void UnfreezeMovement()
    {
        isFrozen = false;
        playerRB.constraints = originalConstraints;
    }
    void OnWall ()
    {
        if (!OnAnyWall())
            return;
        FreezeMovement();
        UnfreezeMovement();
        
       if (Input.GetKey(KeyCode.W))
        {
            //play climb up animation
            //left/right wall animation
            //high or low animation
            transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
        }
       if (Input.GetKey(KeyCode.S))
        {
            // play slide down animation
            // left/right wall animation
            // high or low animation
            transform.Translate(Vector3.down * climbSpeed * Time.deltaTime);
        }
       if (Input.GetKeyDown(KeyCode.Space)&& (BotLeftRay()||TopLeftRay()))
        {
            playerRB.AddForce(Vector3.up *jumpForce, ForceMode.Impulse);
            playerRB.AddForce(Vector3.right*jumpForce, ForceMode.Impulse);
            
            
        }
       if (Input.GetKeyDown(KeyCode.Space) && (BotRightRay() || TopRightRay()))
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerRB.AddForce(Vector3.left * jumpForce, ForceMode.Impulse);
            
            
        }



    }
    [SerializeField] private bool BotLeftRay() => RayCheck(Vector3.left, false);
    [SerializeField] private bool TopLeftRay() => RayCheck(Vector3.left, true);
    [SerializeField] private bool BotRightRay() => RayCheck(Vector3.right, false);
    [SerializeField] private bool TopRightRay() => RayCheck(Vector3.right, true);
    [SerializeField] public bool OnAnyWall() => BotLeftRay()||TopLeftRay()||BotRightRay()||TopRightRay();
    public bool RayCheck(Vector3 dir, bool positive)
    {
        float offSetPos;
        if (positive)
            offSetPos = transform.position.y + rayHeightOffset;
        else
            offSetPos = transform.position.y - rayHeightOffset;
        return Physics.Raycast(new Vector3(transform.position.x, offSetPos, transform.position.z), dir, 1f, scalableWallsMask);
    }
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (other.CompareTag("Water"))
        {
            isDead = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
   /* public IEnumerator SpeedRecoverd()
    {
        yield return new WaitForSeconds(2);
        isSlowed = false;
        speed = 8;

    }
    public void EnemySpeedTouch()
    {
        isSlowed = true;
        SpeedRecoverd();
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.right);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.right);
    }
    
}
