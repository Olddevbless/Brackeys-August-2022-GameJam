using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyController : MonoBehaviour
{
    
    [SerializeField] GameObject target;
    [SerializeField] float speed;
    GameObject player;
    GameObject doggo;
    [SerializeField] bool isPositioning;
    [SerializeField] bool isAttacking;
    [SerializeField]float detectionRadius;
    [SerializeField][Range(1,4)] int attackOptions;
    [SerializeField]float attackRange;
    [SerializeField] float attackRadius;

    void Start()
    {
        doggo = FindObjectOfType<FollowPlayer>().gameObject;
        player = FindObjectOfType<PlayerMovement>().gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectTarget();
        
        if (Vector3.Distance(this.transform.position, player.transform.position) < detectionRadius&& !isAttacking)
        {
            isPositioning = true;

        }
        if (Vector3.Distance(target.transform.position,player.transform.position)<attackRadius)
        {
            isAttacking = true;
            
        }
        AttackTarget(attackOptions);
    }
    void DetectTarget()
    {
        if (isPositioning&&!isAttacking)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            
        }
        




    }
    void AttackTarget(int attackOption)
    {
        if (isAttacking)
        {
            isPositioning = false;
            Debug.Log("Moving to attack position");
            this.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        
        if (Vector3.Distance(transform.position,player.transform.position)<attackRange&& attackOption ==1)
        {
            //player kill player animation
            player.GetComponent<PlayerMovement>().FreezeMovement();
            Invoke("DestroyPlayer", 1.5f) ;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange && attackOption == 2)
        {
            //attack player leg animation
            player.GetComponent<PlayerMovement>().FreezeMovement();
            Invoke("SlowPlayer", 1.5f);
        }
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange && attackOption == 3)
        {
            //flashlight malfunction animation
            player.GetComponent<PlayerMovement>().FreezeMovement();
            Invoke("DisableFlashLight", 1.5f);
        }

    }
    void DestroyPlayer()
    {
        Destroy(player);
    }
    void SlowPlayer()
    {
        player.GetComponent<PlayerMovement>().isSlowed = true;
    }
    void DisableFlashlight()
    {
        player.GetComponentInChildren<FlashLight>().currentBatteryLife = -3;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
