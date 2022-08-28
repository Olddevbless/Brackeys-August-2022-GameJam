using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarknessController : MonoBehaviour
{
    GameObject target;
    [SerializeField] public float darknessSpeed;
    [SerializeField] public bool followingPlayer;
    FollowPlayer doggo;
    [SerializeField] float distanceFromDoggo;
    [SerializeField] float barkRange;
    [SerializeField] bool darknessInitiated;
    Vector3 offSet = new Vector3(0f, -5f, 0f);

    private bool scared = false;
    private int secondScared = 3;
    private void Awake()
    {
        followingPlayer = false;
        doggo = FindObjectOfType<FollowPlayer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;
        secondScared = 3;
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
        if (distanceFromDoggo <= barkRange && doggo.isBarking && scared == false && secondScared>0)
        {
            StartCoroutine(Scared());
        }
        if (followingPlayer)
        {
            MoveTowardsPlayer();
        }


    }
    IEnumerator Scared()
    {
        scared = true;
        followingPlayer = false;
        yield return new WaitForSeconds(secondScared);
        secondScared--;
        darknessSpeed++;
        followingPlayer = true;
        scared = false;
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
            //StopCoroutine(GameManager.current.Notice(""));
            //StartCoroutine(GameManager.current.Notice("you died..."));
        }
        // play animation for darkness grabbing the player
        Invoke("ReloadLevel",2);
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
