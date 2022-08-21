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
    [SerializeField] public bool isBarking;
    
    NavMeshAgent nav;

    void Start()
    {

       
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerIsMoving = target.GetComponent<PlayerMovement>().playerIsMoving;
        if (isFollowing == true && playerIsMoving == true)
        {
            Invoke("Follow", 1f);
        }
        if (Mathf.Abs(target.transform.position.x - this.transform.position.x) > followDistance&& playerIsMoving)
        {
            isFollowing = true;
        }
        if (Mathf.Abs(target.transform.position.x - this.transform.position.x) < followDistance)
        {
            isFollowing = false;
        }
        if (isFollowing)
        {
            isBarking = false;
        }
        
        
       
    }
    void Follow()
    {
        nav.destination = new Vector3(target.transform.position.x-followDistance,target.transform.position.y,target.transform.position.z);
    }
    public void Stay()
    {
        isFollowing =false;
        
        isBarking = true;

    }
    public void ComeHere()
    {
        isFollowing = true;
    }
}
