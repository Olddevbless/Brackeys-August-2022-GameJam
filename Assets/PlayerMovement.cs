using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerMovement : MonoBehaviour
{
    Rigidbody playerRB;
    [SerializeField] float speed = 1;
    [SerializeField] float horizontal;
    [SerializeField] float jumpForce= 10;
    [SerializeField] bool isGrounded;
    [SerializeField] public bool playerIsMoving;
    

    private void Awake()
    {
        playerRB = this.GetComponent<Rigidbody>();
           

    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (playerRB.velocity != Vector3.zero)
        {
            playerIsMoving = true;
        }
        else
        {
            playerIsMoving = false;
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
        }
        if (Input.GetKey(KeyCode.D))
        {
            
            //playerRB.AddForce(new Vector3(horizontal, 0, 0) * speed, ForceMode.Force);
            this.transform.Translate(new Vector3(horizontal,0,0)  * speed * Time.deltaTime);
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
}
