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
    #endregion

    [SerializeField] FollowPlayer doggo;
    Rigidbody playerRB;
    RigidbodyConstraints originalConstraints;

    private void Awake()
    {
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
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            doggo.Stay();
            
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            doggo.ComeHere();
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
    public void FreezeMovement()
    {
        isFrozen = true;
        playerRB.constraints = RigidbodyConstraints.FreezePositionY;
        playerRB.constraints = RigidbodyConstraints.FreezePositionX;
    }
}
