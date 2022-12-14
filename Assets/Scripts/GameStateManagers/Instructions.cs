using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [SerializeField] GameObject intructionsPopUp;

    private void Start()
    {
        //if inctructions are active, pause the time
        Time.timeScale = 0f;        
    }

    private void Update()
    {       
        //when I button is pressed, stop the time and display the pop up
         if (Input.GetKey(KeyCode.I))
         {
             Pause();
             Time.timeScale = 0f;
         }
    }

    public void Resume()
    {
        intructionsPopUp.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        intructionsPopUp.SetActive(true);
        //stop the time
        Time.timeScale = 0f;
    }
}
