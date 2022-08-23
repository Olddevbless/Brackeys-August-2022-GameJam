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
    [SerializeField] bool darknessInitiated;
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

        if (distanceFromDoggo > barkRange && !doggo.isBarking && darknessInitiated)
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
        darknessInitiated = true;
      this.transform.Translate(Vector3.right*darknessSpeed*Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            followingPlayer = false;
            other.GetComponent<PlayerMovement>().FreezeMovement();
            other.GetComponent<PlayerMovement>().isDead = true;
        }
        // play animation for darkness grabbing the player
        Invoke("LoadNextLevel",2);
    }
    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
