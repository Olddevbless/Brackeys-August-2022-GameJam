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
    Vector3 offSet = new Vector3(0f, -5f, 0f);
    private void Awake()
    {
        followingPlayer = false;
        doggo = FindObjectOfType<FollowPlayer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!darknessInitiated)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
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
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponentInChildren<MeshRenderer>().enabled = true;
        darknessInitiated = true;
      this.transform.position = (Vector3.MoveTowards(transform.position,target.transform.position+offSet,1*darknessSpeed*Time.deltaTime));
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
        Invoke("ReloadLevel",2);
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
