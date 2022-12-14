using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{    
    public Animator animator;
    public float lookRadius = 10f;
    public int proejctileDamage = 50;

    protected Transform target;
    protected NavMeshAgent agent;

    public int maxHealth = 100;
    public int currentHealth;

    public int damageStrength = 18;

    protected int isWalkingHash;

    protected bool isDead = false;

    public GameObject bossRagdoll;
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
    }
    
    protected void FollowPlayer()
    {
        //follow player
        float distance = Vector3.Distance(target.position, transform.position);

        //If the distance from player is smaller than lookradius sphere, follow player
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            //start walking animation
            animator.SetBool(isWalkingHash, true);
            //Debug.Log("Enemy is Walking");


            if (distance <= agent.stoppingDistance)
            {
                //if enemy starts, stop walking animation
                animator.SetBool(isWalkingHash, false);

                //face target
                FaceTarget();
            }
        } else if(this.tag == "Boss" && currentHealth <= 20)
        {
            //when enfuriates, start running
            animator.SetBool("IsRunning", true);
        }
        //when player is outside the look radius, stop walking
        else
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    public void TakeDamage(int damage)
    {
        //on hit get healthh down
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        //play sound
        SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.enemyHit);

        //when health goes below 0, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        if (this.tag == "Boss")
        {
            Debug.Log("Boss Died");
            isDead = true;
            SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.bossDeath);

            this.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;

            FindObjectOfType<YouWon>().DisplayScreen();
        }
        else 
        {
            Debug.Log("Enemy Died");
            animator.SetBool("IsDead", true);

            SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.enemyDeath);

            //disable the enemy
            this.enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }

    }

    //On collision with projectiles, take damage 
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            TakeDamage(proejctileDamage);
        }
    }

    //draw detection range gizmo 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    protected void FaceTarget()
    {
        //get direction to the player
        Vector3 direction = (target.position - transform.position).normalized;

        //rotation to point to target
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        //update to look at this location (with smoothing)
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); ;

    }

    //When the player enters trigger zone, attack and take damage from player
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            animator.SetTrigger("Attack");
            other.gameObject.GetComponent<AnimationController>().TakeDamage(damageStrength);

            //play sound
            SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.enemyAttack);
        }
        
    }
}
