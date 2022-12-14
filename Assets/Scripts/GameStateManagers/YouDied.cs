using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    [SerializeField] GameObject youDiedScreen;

    public void DisplayScreen()
    {        
        youDiedScreen.SetActive(true);
        Time.timeScale = 0f;        
    }

    public void ChangeScreen(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}
