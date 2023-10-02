
/*
 * Jacob Zydorowicz
 * Project 5
 * Spawns conflict clouds
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawn : MonoBehaviour
{
    public GameObject cloudPrefab;

  
    void Start()
    {
        //StartCoroutine(SpawnRandomPrefabWithCoroutine());
    }

    IEnumerator SpawnRandomPrefabWithCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            SpawnClouds();
        }
    }

    //Spawns clouds randomly on left and right sides of screen
    private void SpawnClouds()
    {
       
            Vector2 spawnPos = new Vector2(transform.position.x, Random.Range(-3.0f, 5.0f));
            int side = Random.Range(0, 1);

            //side = 0,left side
            //side = 1,right side
            if ((side == 0) && gameObject.CompareTag("Spawn Left"))
            {
                spawnPos = new Vector2(transform.position.x, Random.Range(-3.0f, 5.0f));
            }
            else if ((side == 1) && gameObject.CompareTag("Spawn Right"))
            {
                spawnPos = new Vector2(transform.position.x, Random.Range(-3.0f, 5.0f));
            }

            Instantiate(cloudPrefab, spawnPos, cloudPrefab.transform.rotation);
        
     
    }
}
