using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool collided;
    public int damage = 40;
    private void OnCollisionEnter(Collision collision)
    {     
        //if hit object isn't the player or projectile, destroy the projectile
        if (collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Player" && !collided)
        {
            Destroy(gameObject);
            collided = true;
        } 
    }

}
