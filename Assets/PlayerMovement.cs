using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    #region public variables
    
    public float speed = 1;
    public float horizontal;
    public float jumpForce= 10;
    public FollowPlayer doggo;
    public float gravity = -9.81f;
    
    #endregion
    
    
    #region hidden in inspector
    
    [HideInInspector]
    public bool playerIsMoving = false;
    
    [HideInInspector]
    public bool isGrounded;
    
    #endregion
    
    
    #region private
    
    private Rigidbody playerRB;
    private bool isFrozen = false;
    
    #endregion
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody>();
        doggo = FindObjectOfType<FollowPlayer>();

    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
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
        ApplyGravity();
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void ApplyGravity()
    {
        playerRB.velocity += Vector3.down * gravity * Time.deltaTime;
    }
    private void Movement()
    {
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