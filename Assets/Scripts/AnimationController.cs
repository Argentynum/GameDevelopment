using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : PlayerManager
{
    //animation variables
    Animator animator;
    int isWalkingHash;
    int isSprintingHash;

    private int keyCounter = 0;

    //attack variables
    public Transform attackPoint;
    public LayerMask enemyLayers;

    public float attackRange = 0.8f;
    public int attackDamage = 18;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    //projectile variables 
    public Camera cam;
    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 20;
    public float arcRange = 0.5f;
    public bool stars;

    bool berries = false;
    bool berriesPicked = false;

    public HungerBar hungerBar;

    public bool isDead = false;

    //public InputAction playerControls;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isSprintingHash = Animator.StringToHash("isSprinting");

        gems = 0;
        score = 0;
        currentHunger = hunger;
        currentHealth = health;
        currentScore = score;
        currentMana = mana;

        healthBar.SetMaxHealth(currentHealth);
        hungerBar.SetMaxHunger(currentHunger);

        ScoreManagement.scoreInstance.SetMana(currentMana);

    }

    void Update()
    {
        //setting animator booleans for running and sprinting
        bool forwardPressed = Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("a") || Input.GetKey("d");
        bool isWalking = animator.GetBool("isWalking");

        bool sprintPressed = Input.GetKey("left shift");
        bool isSprinting = animator.GetBool("isSprinting");

        checkHealth();

        //Counter checking if a mouse button is clicked - if so, counter's value goes up
        if (Input.GetKeyDown(KeyCode.Mouse0) || (Input.GetKeyDown(KeyCode.Mouse1)))
        {
            keyCounter++;

            Debug.Log("Value: " + keyCounter);

            //when mouse button is released, key counter goes down
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            keyCounter--;
        }

        //running
        //if player presses any of the "forward" keys, play running animation
        if (forwardPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        //id player is not pressing any of the "forward" keys, stop playing running animation
        if (!forwardPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        //sprinting
        //if isn't already sprinting and keys to Run and Sprint are pressed, then sprint
        if (!isSprinting && (forwardPressed && sprintPressed))
        {
            animator.SetBool(isSprintingHash, true);
        }

        //if is sprinting but sprint key is not pressed, stop sprinting
        if (isSprinting && ((!forwardPressed && !sprintPressed) || (forwardPressed && !sprintPressed)))
        {
            animator.SetBool(isSprintingHash, false);
        }

        //attack
        //unsure not to spam attacks one after another
        if (Time.time >= nextAttackTime)
        {
            //attack when mouse button clicked and key counter has a given value
            if (Input.GetKeyDown(KeyCode.Mouse0) && keyCounter <= 1)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

        //shootig
        //if key counter has a given value, play shooting animation
        if(ScoreManagement.scoreInstance.GetMana() > 0)
        {
            if (keyCounter >= 2)
            {
                animator.SetTrigger("Shoot");
                Shoot();
                //set stars to true 
                stars = true;


            }
            
            //if stars are true and mouse button is released, instantiate projectile
            if (stars == true && Input.GetMouseButtonUp(0))
            {
                InstantiateProjectile();

                //make mana score and current hunger go down
                ScoreManagement.scoreInstance.LoseMana();
                currentHunger -= 10;
                SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.shoot);

                hungerBar.SetHunger(currentHunger);
                Debug.Log(currentMana);
            }
        }
        //do not spam projectiles by setting minimum time after which another shoot can be executed
        else
        {
            StartCoroutine(ExecuteAfterTime(0.05f));
        }


        //if berries are true but havent been picked yet and key E is pressed, collect berries
        if (berries && Input.GetKeyUp("e") && !berriesPicked)
        {
            //stats up

            if(currentHunger + 30 <= 100)
            {
                currentHunger += 30;
                hungerBar.SetHunger(currentHunger);
                ScoreManagement.scoreInstance.AddPoints(30);

                //play sound
                SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.statsUp);

                //berries picked
                berriesPicked = true;

                Debug.Log("Berries: " + berries + "Gave hunger: " + currentHunger);
            } else
            {
                currentHunger = 100;
                hungerBar.SetHunger(currentHunger);
                ScoreManagement.scoreInstance.AddPoints(30);

                //play sound
                SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.statsUp);

                //berries picked
                berriesPicked = true;

                Debug.Log("Berries: " + berries + "Gave hunger: " + currentHunger);
            }
            
        }
    }

    void Attack()
    {
        //Play attack animation
        animator.SetTrigger("Attack");

        //play sound
        SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.playerAttack);

        //Detect enemies
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
        Debug.Log("Hunger is now: " + currentHunger);

        //take hunger points
        currentHunger -= 5;
        hungerBar.SetHunger(currentHunger);

        //Apply damage
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyManager>().TakeDamage(attackDamage);
            ScoreManagement.scoreInstance.AddPoints(20);

            if (enemy.tag == "Boss")
            {
                enemy.GetComponent<BossManager>().TakeDamage(attackDamage);
                Debug.Log(enemy.GetComponent<BossManager>().currentHealth);
                ScoreManagement.scoreInstance.AddPoints(40);
            }
            Debug.Log(enemy.GetComponent<EnemyManager>().currentHealth);
        }
    }

    void Shoot()
    {
        if (ScoreManagement.scoreInstance.GetMana() > 0)
        {
            animator.SetTrigger("Shoot");
            Debug.Log("Hunger: " + currentHunger);
        }
    }

    //Draw attack point sphere
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        //on hit get healthh down
        currentHealth -= damage;
        currentHunger -= 5;
        hungerBar.SetHunger(currentHunger);
        animator.SetTrigger("Damaged");
        //Change healthbar
        healthBar.SetHealth(currentHealth);
        Debug.Log("Damage taken.Health: " + currentHealth + " Hunger: " + currentHunger);

        //play sound
        SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.playerHit);

        //when health goes below 0, die
        if (currentHealth <= 0)
        {
            Die();
            isDead = true;
        }
    }

    //on death disable the character
    public void Die()
    {
        animator.SetBool("IsDead", true);
        isDead = true;        

        this.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<ThirdPersonMovement>().enabled = false;

        FindObjectOfType<YouDied>().DisplayScreen();
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        stars = false;
    }

    void InstantiateProjectile()
    {
        //Instantiate a projectile as a Rigidbody that travels straight from fire point
        var projectileObj = Instantiate(projectile, firePoint.position, firePoint.rotation) as GameObject;
        projectileObj.GetComponent<Rigidbody>().velocity = (transform.forward).normalized * projectileSpeed;

        //making little wiggle while the projectile is travelling
        iTween.PunchPosition(projectileObj, new Vector3(Random.Range(-arcRange, arcRange), Random.Range(-arcRange, arcRange), 0), Random.Range(0.5f, 10));

        //Detect enemies
        Collider[] hitEnemies = Physics.OverlapSphere(projectileObj.transform.position, 0.4f, enemyLayers);

        //Apply damage
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyManager>().TakeDamage(attackDamage);
            //enemy.GetComponent<BossManager>().TakeDamage(attackDamage);
            Debug.Log("Projectile Damage Taken");
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //on collision with a mana potion, destroy the flask and get more mana points
        if (other.gameObject.tag == "ManaPotion")
        {
            Destroy(other.gameObject);

            //manipulate player stats
            ScoreManagement.scoreInstance.AddMana();
            ScoreManagement.scoreInstance.AddPoints(20);          
            currentHunger -= 2;

            hungerBar.SetHunger(currentHunger);

            //play sound
            SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.statsUp);

            Debug.Log("mana: " + currentMana + " Score: " + currentScore + " Hunger: " + currentHunger);
        }// on trigger with health potion collider, gain health if it's currently less than 100 
        else if (other.gameObject.tag == "HealthPotion")
        {
            if (healthBar.slider.value == 100)
            {
                Debug.Log("Can't collect the flask");
            }
            else if (healthBar.slider.value < 100 & healthBar.slider.value >= 0)
            {
                Destroy(other.gameObject);

                //play sound
                SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.statsUp);

                //manipulate player stats
                if (currentHealth + 20 > 100)
                {
                    currentHealth = 100;
                    currentHunger -= 2;
                } else
                {
                    currentHealth += 20;
                    currentHunger -= 2;
                    ScoreManagement.scoreInstance.AddPoints(10);
                }
                             
                

                //manipulate stats bars values
                healthBar.SetHealth(currentHealth);
                hungerBar.SetHunger(currentHunger);

                Debug.Log(currentHealth + " Score: " + currentScore + " Hunger: " + currentHunger);
            }
        }
        //on trigger whith gem collider, collect the gem
        else if (other.gameObject.tag == "Gem")
        {
            Destroy(other.gameObject);

            //manipulate player stats
            ScoreManagement.scoreInstance.AddGems(10);
            ScoreManagement.scoreInstance.AddPoints(50);
            currentHunger -= 2;

            hungerBar.SetHunger(currentHunger);

            //play sound
            SFXManager.sfxInstance.Audio.PlayOneShot(SFXManager.sfxInstance.gemCollected);

            Debug.Log("Gem collected. Gem points: " + gems + " Score: " + currentScore + " Hunger: " + currentHunger);
        }

        //when colliding with berries, set berries to true
        else if (other.gameObject.tag == "Berries")
        {
            berries = true;
        }
    }

    //on exit from berries, if berries are true and were picked, destroy berries and set variables to false
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Berries" && berries && berriesPicked)
        {
            Destroy(other.gameObject);
            berries = false;
            berriesPicked = false;
        }
    }
}
