using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallEnemyController : MonoBehaviour
{

    [SerializeField] GameObject target;
    [SerializeField] float speed;
    GameObject player;
    GameObject doggo;
    [SerializeField] bool isPositioning;
    [SerializeField] bool isAttacking;
    [SerializeField] float detectionRadius;
    [Tooltip("Determines the action of the enemy - 1 = kill player, 2= slow player, 3= disable flashlight")]
    [SerializeField] [Range(1, 3)] int attackOptions;
    [SerializeField] float attackRange;
    [SerializeField] float attackRadius;
    [SerializeField] float scaredRadius;

    public enum smallEnemyFSM
    {
        Idle,
        MoveTowardsPosition,
        MoveTowardsPlayer,
        Attack
    }
    public smallEnemyFSM enemyMode;

    void Start()
    {

        doggo = FindObjectOfType<FollowPlayer>().gameObject;
        player = FindObjectOfType<PlayerMovement>().gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > detectionRadius)
        {
            enemyMode = smallEnemyFSM.Idle;
        }
        if (Vector3.Distance(this.transform.position, player.transform.position) < detectionRadius)
        {
            enemyMode = smallEnemyFSM.MoveTowardsPosition;
        }
        if (Vector3.Distance(target.transform.position, player.transform.position) < attackRadius)
        {
            enemyMode = smallEnemyFSM.MoveTowardsPlayer;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            enemyMode = smallEnemyFSM.Attack;
        }
        switch (enemyMode)
        {
            case smallEnemyFSM.Idle:
                break;
            case smallEnemyFSM.MoveTowardsPosition:
                this.transform.rotation = Quaternion.LookRotation(target.transform.position - this.transform.position);
                this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                break;
            case smallEnemyFSM.MoveTowardsPlayer:
                this.transform.rotation = Quaternion.LookRotation(player.transform.position - this.transform.position);
                this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                break;
            case smallEnemyFSM.Attack:

                if (attackOptions == 1)
                {
                    Invoke("DestroyPlayer", 1.5f);
                }
                if (attackOptions == 2)
                {
                    Invoke("SlowPlayer", 1.5f);
                }
                if (attackOptions == 3)
                {
                    Invoke("DisableFlashLight", 1.5f);
                }
                break;
        }

        void DestroyPlayer()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        void SlowPlayer()
        {
            player.GetComponent<PlayerMovement>().isSlowed = true;
        }
        void DisableFlashlight()
        {
            player.GetComponentInChildren<FlashLight>().currentBatteryLife = -3;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRadius);
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
        void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
            if (other.CompareTag("Light"))
            {

                StartCoroutine(DeathByLight());
            }
        }
        IEnumerator DeathByLight()
        {
            //play death animation
            Debug.Log("play death animation");
            //isAttacking = false;
            //isPositioning = false;
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
    }
}
