using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarknessController : MonoBehaviour
{
    GameObject target;
    [SerializeField] float darknessSpeed;
    [SerializeField] public bool followingPlayer;
    FollowPlayer doggo;
    [SerializeField] float distanceFromDoggo;
    [SerializeField] float barkRange;
    private void Awake()
    {
        followingPlayer = false;
        doggo = FindObjectOfType<FollowPlayer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromDoggo = Mathf.Abs(transform.position.x - doggo.transform.position.x);
        
        if (target.GetComponent<Rigidbody>().velocity != Vector3.zero&& distanceFromDoggo>barkRange )
        {
            followingPlayer = true;
        }
        if (distanceFromDoggo > barkRange && !doggo.isBarking)
        {
            followingPlayer = true;
        }
        if (distanceFromDoggo <= barkRange && doggo.isBarking)
        {
            followingPlayer = false;
        }
        if (followingPlayer)
        { 
            MoveTowardsPlayer();
        }
        
        
    }
    public void MoveTowardsPlayer()
    {
      transform.Translate(Vector3.right*darknessSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().FreezeMovement();
        }
        // play animation for darkness grabbing the player
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
