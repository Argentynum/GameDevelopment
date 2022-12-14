using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossManager : EnemyManager
{
    //public GameObject bossRagdoll;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        target = PlayerManager.playerInstance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        isWalkingHash = Animator.StringToHash("IsWalking");
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        //if health drops below a value, infuriate 
        if (currentHealth <= 20)
        {
            Furia();
        }
 
        if (isDead)
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject);
            Instantiate(bossRagdoll, transform.position, transform.rotation);
            Debug.Log("instantiated at: " + transform.position);
        }
    }

    //draw detection range gizmo 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    //when boss infuriates, get higher damage & speed
    void Furia()
    {    
        damageStrength = 38;
        agent.speed = 4;
        //animator.SetBool("IsWalking", false);
        //animator.SetBool("IsRunning", true);
             
    }
}
