﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    #region movement in inspector
    
    public float speed = 1;
    public bool isSlowed;
    public float slow=1;
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
    GameManager gameManager;
    [SerializeField] bool grabBoulder;
    [SerializeField] FollowPlayer doggo;
    Rigidbody playerRB;
    RigidbodyConstraints originalConstraints;
    [SerializeField] FlashLight flashLight;
    [SerializeField] bool noteReading;
    public bool isDead;
    [Header("Climbing")]
    //[SerializeField] bool onWall;
    //[SerializeField] bool onRightWall;
    //[SerializeField] bool onLeftWall;
    
    [SerializeField] float rayHeightOffset;
    [SerializeField] float climbSpeed;
    
    int layerID = 8;
    LayerMask scalableWallsMask;

    private void Awake()
    {
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

    private void Update()
    {
        //horizontal = Input.GetAxis("Horizontal");
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
        else if (!gameManager.pauseMenuIsActive)
        {
            Time.timeScale =1;
        }
        OnWall();
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
        if (isSlowed)
        {
            slow = 0.5f;

          if (slow<1)
            {
                slow += 0.1f * Time.deltaTime;
            }
          if (slow>=1)
            {
                isSlowed = false;
            }
            
        }
        var horizontalInput = Input.GetAxisRaw("Horizontal");

        var direction = Vector3.right * horizontalInput;
        direction *= Time.deltaTime;

        transform.Translate(direction * speed*slow);

        playerIsMoving = horizontalInput != 0;

    }
    public void ApplyGravity()
    {
        if (!OnAnyWall())
        {
            playerRB.velocity += Vector3.down * gravity * Time.deltaTime;
        }

    }
   
    public void FreezeMovement()
    {
        isFrozen = true;
        playerRB.constraints = RigidbodyConstraints.FreezePositionY;
        playerRB.constraints = RigidbodyConstraints.FreezePositionX;
        if (!isDead)
        {
            Invoke("UnfreezeMovement", 2f);
        }
        
    }
    public void UnfreezeMovement()
    {
        isFrozen = false;
    }
    void OnWall ()
    {
        if (!OnAnyWall())
            return;
        
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

        
        
    }
    [SerializeField] private bool BotLeftRay() => RayCheck(Vector3.left, false);
    [SerializeField] private bool TopLeftRay() => RayCheck(Vector3.left, true);
    [SerializeField] private bool BotRightRay() => RayCheck(Vector3.right, false);
    [SerializeField] private bool TopRightRay() => RayCheck(Vector3.right, true);
    [SerializeField] private bool OnAnyWall() => BotLeftRay()||TopLeftRay()||BotRightRay()||TopRightRay();
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

    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (other.CompareTag("Note"))
            {
                Debug.Log("reading" + other.gameObject.name);
                noteReading = !noteReading;
                other.GetComponent<Note>().ReadNote();
            }
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.left);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - rayHeightOffset, transform.position.z), Vector3.right);
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + rayHeightOffset, transform.position.z), Vector3.right);
    }
    
}