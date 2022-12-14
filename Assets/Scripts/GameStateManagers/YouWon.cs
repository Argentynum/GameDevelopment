using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWon : MonoBehaviour
{
    [SerializeField] GameObject youWonScreen;
    public void DisplayScreen()
    {
        youWonScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ChangeScreen(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
