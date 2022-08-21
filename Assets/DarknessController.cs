using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    GameObject target;
    [SerializeField] float darknessSpeed;
    [SerializeField] public bool followingPlayer;
    FollowPlayer doggo;

    private void Awake()
    {
        followingPlayer = false;
        doggo = FindObjectOfType<FollowPlayer>();
        target = FindObjectOfType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            followingPlayer = true;
        }
        if (!doggo.isBarking&& followingPlayer)
        { 
            MoveTowardsPlayer();
        }
        
    }
    public void MoveTowardsPlayer()
    {
      this.transform.Translate(Vector3.right*darknessSpeed*Time.deltaTime);
    }
}
