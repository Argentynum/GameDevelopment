using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager playerInstance;

    private void Awake()
    {
        playerInstance = this;
    }

    #endregion

    public GameObject player;

    public HealthBar healthBar;

    public float health = 50;
    public int mana = 2;
    public float hunger = 100;
    protected int gems;
    protected int score;

    protected float currentHunger;
    protected float currentHealth;
    protected int currentScore;
    protected int currentMana;
    protected int currentGems;

    protected const float dyingValue = 4f;

    protected bool hasMana = true;

    private void Update()
    {
        mana = currentMana;
        score = currentScore;
        gems = currentGems;
    }

    //check was is player's hunger level
    protected void CheckHunger()
    {
        //if there are no hunger points, take hunger points over time and eventually, die
        if (currentHunger <= 0)
        {
            currentHealth -= dyingValue * Time.deltaTime;
            healthBar.SetHealth(currentHealth);

            Debug.Log("Health Going Down: " + currentHealth);
            if (currentHealth <= 0)
            {
                GetComponent<AnimationController>().Die();
                GetComponent<AnimationController>().isDead = true;
            }
        }
    }

    //check player's current health level
    protected void checkHealth()
    {
        CheckHunger();
        if (currentHealth <= 0)
        {
            GetComponent<AnimationController>().Die();
            GetComponent<AnimationController>().isDead = true;
        }
    }
}

