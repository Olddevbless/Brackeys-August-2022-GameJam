using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] float followDelay;
    [SerializeField] float followDistance;
    [SerializeField] bool isFollowing;
    [SerializeField] bool playerIsMoving;
    public bool isBarking;
    public bool isStaying;
    NavMeshAgent nav;
    [SerializeField] bool isIdle;

    void Start()
    {
        isIdle = false;
        target = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.isActiveAndEnabled == false)
        {
            isIdle = true;
        }
        if (isIdle == false)
        {
            playerIsMoving = target.GetComponent<PlayerMovement>().playerIsMoving;
            if (isFollowing == true && playerIsMoving == true)
            {
                Invoke("Follow", 1f);
            }
            if (Mathf.Abs(target.transform.position.x - this.transform.position.x) > followDistance && playerIsMoving && !isStaying)
            {
                isFollowing = true;
            }
            if (Mathf.Abs(target.transform.position.x - this.transform.position.x) < followDistance && !isBarking)
            {
                isFollowing = false;
            }
            if (isFollowing)
            {
                isBarking = false;
            }
        }
       
        
        
        
       
    }
    void Follow()
    {
        nav.destination = new Vector3(target.transform.position.x-followDistance,target.transform.position.y+2,target.transform.position.z);
    }
    public void Stay()
    {
        Debug.Log("Staying");
        isFollowing =false;
        nav.destination = transform.position;
        isStaying = true;

    }
    public void ComeHere()
    {
        Debug.Log("coming!");
        nav.destination = target.transform.position;
        isFollowing = true;
        isBarking = false;
        isStaying = false;
    }
    public void Bark()
    {
        if (isStaying)
        {
            Debug.Log("Barking");
            isBarking = true;
        }
        if (!isStaying)
        {
            Debug.Log("Buddy cannot bark, it is following");
        }
    }
}
