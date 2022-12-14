using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource Audio;

    public AudioClip shoot;
    public AudioClip statsUp;
    public AudioClip playerHit;
    public AudioClip enemyHit;
    public AudioClip playerAttack;
    public AudioClip enemyAttack;
    public AudioClip enemyDeath;
    public AudioClip playerDeath;
    public AudioClip bossDeath;
    public AudioClip gemCollected;

    public static SFXManager sfxInstance;

    private void Awake()
    {
        if (sfxInstance != null && sfxInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        sfxInstance = this;
        DontDestroyOnLoad(this);
    }
}
