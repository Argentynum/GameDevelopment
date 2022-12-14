using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgAmbience : MonoBehaviour
{
    public static BgAmbience ambienceInstance;

    private void Awake()
    {
        if (ambienceInstance != null && ambienceInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        ambienceInstance = this;
        DontDestroyOnLoad(this);
    }
}
