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
    [SerializeField] float jumpHeight;
    public bool isBarking;
    public bool isStaying;
    NavMeshAgent nav;
    [SerializeField] bool isIdle;
    [SerializeField] Animator buddyAnimator;
    void Start()
    {
        isIdle = false;
        target = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nav.isActiveAndEnabled)
        {
            isIdle = true;
        }
        if (!isIdle)
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
        if (isFollowing)
        {
            buddyAnimator.SetBool("isRunning", true);
        }
        if (isFollowing == false)
        {
            buddyAnimator.SetBool("isRunning", false);
        }
        if (isBarking)
        {
            buddyAnimator.SetBool("isBarking", true);
        }
        else
        {
            buddyAnimator.SetBool("isBarking", false);
        }
        if (transform.position.y > transform.position.y+jumpHeight)
        {
            buddyAnimator.SetTrigger("Jump");
        }
        
    }
    void Follow()
    {
        nav.destination = new Vector3(target.transform.position.x-(followDistance/2),target.transform.position.y+2,target.transform.position.z);


        // example of how to play sounds.
        // the "Walk" sound is registered on the SoundRegistry object (inspector).
        // just use SoundManager.Play(ID)
        SoundManager.PlaySound("");

    }
    public void Stay()
    {
        StopCoroutine(GameManager.current.Notice(""));
        StartCoroutine( GameManager.current.Notice("Dogo Stay"));

        Debug.Log("Staying");
        isFollowing =false;
        nav.destination = transform.position;
        nav.isStopped=true;
        isStaying = true;

    }
    public void ComeHere()
    {
        StopCoroutine(GameManager.current.Notice(""));
        StartCoroutine(GameManager.current.Notice("Calling Dogo"));

        Debug.Log("coming!");
        nav.destination = target.transform.position;
        isFollowing = true;
        isBarking = false;
        isStaying = false;
        nav.isStopped = false;
    }
    public void Bark()
    {
        Stay();
        StopCoroutine(GameManager.current.Notice(""));
        StartCoroutine(GameManager.current.Notice("Dogo Bark"));

        Debug.Log("Barking");
        isBarking = true;

        /*if (isStaying)
        {
        }*/
        if (!isStaying)
        {
            Debug.Log("Buddy cannot bark, it is following");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NavMeshLinks"))
        {
            buddyAnimator.SetTrigger("Jump");
        }
    }
}
