using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRB;
    [SerializeField] float speed = 1;
    [SerializeField] float horizontal;
    [SerializeField] float jumpForce= 10;
    [SerializeField] bool isGrounded;
    [SerializeField] public bool playerIsMoving;
    [SerializeField] FollowPlayer doggo;
    [SerializeField] bool isFrozen;
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
        horizontal = Input.GetAxis("Horizontal");
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
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& isGrounded==true)
        {
            playerRB.AddForce(Vector3.up *jumpForce, ForceMode.Impulse);
        }
    }
    void Movement()
    {
      
        if (Input.GetKey(KeyCode.A))
        {
           
            //playerRB.AddForce(new Vector3(horizontal, 0, 0)*speed, ForceMode.Force);
            this.transform.Translate(new Vector3(horizontal,0,0) * speed * Time.deltaTime);
            playerIsMoving = true ;
        }
        if (Input.GetKey(KeyCode.D))
        {
            
            //playerRB.AddForce(new Vector3(horizontal, 0, 0) * speed, ForceMode.Force);
            this.transform.Translate(new Vector3(horizontal,0,0)  * speed * Time.deltaTime);
            playerIsMoving = true;
        }
        
        
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
