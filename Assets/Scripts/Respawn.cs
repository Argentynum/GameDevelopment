using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject spawnee;

    public static Respawn respawnInstance;
   // public bool stopSpawning = false;
   //public float spawnTime;
    //public float spawnDelay;

    // Start is called before the first frame update
     /*void Start()
    {
        InvokeRepeating("RespawnObject", spawnTime, spawnDelay);
    }*/

    public void RespawnObject()
    {
        /*var newInstance = Instantiate(spawnee, transform.position, transform.rotation);
        //newInstance.transform.localScale += new Vector3(0.3778286f, 0.3778286f, 0.3778286f);
        if (stopSpawning)
        {
            CancelInvoke("RespawnObject");
        }*/

        //GameObject nb = Instantiate(spawnee, this.transform) as GameObject;
        StartCoroutine(SpawnAfter());
    }

    IEnumerator SpawnAfter()
    {
        yield return new WaitForSeconds(2);
        GameObject nb = Instantiate(spawnee, this.transform) as GameObject;
    }
}
