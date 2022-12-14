using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagement : PlayerManager
{
    public static ScoreManagement scoreInstance;

    public GameObject playerManager;

    public TextMeshProUGUI gemsAmount;
    public TextMeshProUGUI manaAmount;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    int highscore = 0;

    private void Awake()
    {
        scoreInstance = this;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("highscore"))
        {
            PlayerPrefs.SetInt("highscore", 0);
            Load();
        } else
        {
            Load();
        }
    }

    public void AddGems(int amount)
    {
        gems += amount;
        gemsAmount.text = gems.ToString();
    }

    public void AddMana()
    {
        //int manaNow = GetMana();
        //currentMana = manaNow;
        currentMana++;
        manaAmount.text = currentMana.ToString();
    }

    public void LoseMana()
    {
        if (currentMana > 0)
        {
            currentMana--;
            manaAmount.text = currentMana.ToString();
        }        
    }

    public void SetMana(int amount)
    {
        //int manaNow = GetMana();
        //currentMana = manaNow;
        currentMana = amount;
        manaAmount.text = currentMana.ToString();
    }

    //get mana value
    public int GetMana()
    {
        int manaNow = currentMana;
        
        Debug.Log("Mana now " + manaNow);
        return manaNow;
    }

    public void SetPoints()
    {
        PlayerPrefs.SetInt("highscore", currentScore);
    }

    public void Load()
    {
        currentScore = PlayerPrefs.GetInt("highscore");
        highscoreText.text = currentScore.ToString();
    }

    public void AddPoints(int points)
    {
        currentScore += points;
        scoreText.text = currentScore.ToString();

        //save highscore if its smaller than currentscore
        if(highscore < currentScore)
        {
            PlayerPrefs.SetInt("highscore", currentScore);
            highscoreText.text = currentScore.ToString();
        }
    }
}
