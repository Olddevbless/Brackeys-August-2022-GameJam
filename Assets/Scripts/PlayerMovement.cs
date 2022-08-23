using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    #region movement in inspector
    
    [SerializeField] float speed = 1;
    [SerializeField] float crouchSpeed = 2;
    [SerializeField] float horizontal;
    [SerializeField] float jumpForce= 10;
    [SerializeField] bool isGrounded;
     public bool playerIsMoving;
    [SerializeField] bool isFrozen;
    [SerializeField] float crouchHeight;
    [SerializeField] float centerChange;
    [SerializeField] float gravity = -9.81f; // test
    #endregion

    [SerializeField] bool grabBoulder;
    [SerializeField] FollowPlayer doggo;
    Rigidbody playerRB;
    RigidbodyConstraints originalConstraints;
    [Header("Climbing")]
    [SerializeField] bool onWall;
    [SerializeField] bool onRightWall;
    [SerializeField] bool onLeftWall;
    [SerializeField] bool onLeftWallLow;
    [SerializeField] bool onLeftWallHigh;
    [SerializeField] bool onRightWallLow;
    [SerializeField] bool onRightWallHigh;
    [SerializeField] float rayHeightOffset;
    [SerializeField] float climbSpeed;
    
    int layerID = 8;
    LayerMask scalableWallsMask;

    private void Awake()
    {
        scalableWallsMask = (1 << layerID);
        playerRB = this.GetComponent<Rigidbody>();
        doggo = FindObjectOfType<FollowPlayer>();

    }
    private void Start()
    {
        originalConstraints = playerRB.constraints;
        isFrozen = false;
    }

    private void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
       if (!isFrozen)
        {
            playerRB.constraints = originalConstraints;
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
        OnWall();
        if (onWall&&Input.GetKey(KeyCode.R))
        {
            playerRB.useGravity = false;
            
        }
        else
        {
            ApplyGravity();
            playerRB.useGravity = true;
        }
        Movement();
        Jump();
        Crouching();
        
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded==true)
        {
            playerRB.AddForce(Vector3.up *jumpForce, ForceMode.Impulse);
        }
    }
    void Crouching()
    {
        Vector3 capsuleColliderCenter = gameObject.GetComponent<CapsuleCollider>().center;

        if (Input.GetKey(KeyCode.C))
        {
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
            gameObject.GetComponent<CapsuleCollider>().height = crouchHeight * 2;
            gameObject.GetComponent<CapsuleCollider>().center = new Vector3(capsuleColliderCenter.x, 0f, capsuleColliderCenter.z);
            speed = 5;
        }
    }

            void Movement()
    {

        //if (Input.GetKey(KeyCode.A))
        //{

        //    //playerRB.AddForce(new Vector3(horizontal, 0, 0)*speed, ForceMode.Force);
        //    this.transform.Translate(new Vector3(horizontal,0,0) * speed * Time.deltaTime);
        //    playerIsMoving = true ;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{

        //    //playerRB.AddForce(new Vector3(horizontal, 0, 0) * speed, ForceMode.Force);
        //    this.transform.Translate(new Vector3(horizontal,0,0)  * speed * Time.deltaTime);
        //    playerIsMoving = true;
        //}
        var horizontalInput = Input.GetAxisRaw("Horizontal");

        var direction = Vector3.right * horizontalInput;
        direction *= Time.deltaTime;

        transform.Translate(direction * speed);

        playerIsMoving = horizontalInput != 0;

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

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
        
    }
    public void ApplyGravity()
    {
        if (!onWall)
        {
            playerRB.velocity += Vector3.down * gravity * Time.deltaTime;
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if (other.CompareTag ("Note"))
            //{
            //   other.GetComponent<Note>().ReadNote();
            //}
            if (other.CompareTag("Boulder"))
            {
                grabBoulder = !grabBoulder;
                
                if (grabBoulder)
                {
                    other.GetComponent<HingeJoint>().connectedBody = playerRB;
                }
                if (!grabBoulder)
                {
                    other.GetComponent<HingeJoint>().connectedBody = null;
                }
                    
            }
        }
    }
    public void FreezeMovement()
    {
        isFrozen = true;
        playerRB.constraints = RigidbodyConstraints.FreezePositionY;
        playerRB.constraints = RigidbodyConstraints.FreezePositionX;
    }
    void OnWall ()
    {
        bool botLeftRay = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.left, 1f, scalableWallsMask);
        bool topLeftRay = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.left, 1f, scalableWallsMask);
        bool botRightRay = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.right, 1f, scalableWallsMask);
        bool topRightRay = Physics.Raycast(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.right, 1f, scalableWallsMask);
        bool onWallBool = botLeftRay || botRightRay || topLeftRay || topRightRay; 
        if (onWallBool) //checks if player is on the wall at all
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }
        bool onLeftWallBool = botLeftRay || topLeftRay; // checks if the player is on the left wall 
        if (onLeftWallBool)
        {
            onLeftWall = true;
        }
        else
        {
            onLeftWall = false;
        }
        bool onRightWallBool = botRightRay || topRightRay; // checks if player is on the right wall
        if (onRightWallBool)
        {
            onRightWall = true;
        }
        else
        {
            onRightWall = false;
        }
        if (botLeftRay) // checks if the bottom left of the player is touching a wall (pull up animation)
        {
            onLeftWallLow = true;
        }
        else
        {
            onLeftWallLow = false;
        }
        if (botRightRay) // checks if the bottom right of the player is touching a wall (pull up animation)
        {   
            onRightWallLow = true;
        }
        else
        {    
            onRightWallLow = false;
        }
        if (topLeftRay) // checks if the top left of the player is touching a wall (hang animation)
        {     
            onLeftWallHigh = true;
        }
        else
        {           
            onLeftWallHigh = false;
        }
        if (topRightRay) // checks if the top right of the player is touching a wall (hang animation)
        {            
            onRightWallHigh = true;
        }
        else
        {
            onRightWallHigh = false;
        }
       if (onWall&&Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * climbSpeed * Time.deltaTime);
        }
       if (onWall&&Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * climbSpeed * Time.deltaTime);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.right);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.right);
    }
}
