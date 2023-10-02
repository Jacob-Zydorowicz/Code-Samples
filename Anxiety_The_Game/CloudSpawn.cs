/*
 * CIS 350 Game Production
 * Jacob Zydorowicz
 * Anxiety The Game
 * Spawns conflict clouds around the map
 * Last Updated: October first 2023
 */
#region imported namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
#endregion

public class CloudSpawn : MonoBehaviour
{
    Header["Cloud Spawner"]
    [Serializable] GameObject cloudPrefab;

    //spawns clouds randomly between a 1 nd 5 second interval
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

            //spawns on left side of screen
            if ((side == 0) && gameObject.CompareTag("Spawn Left"))
            {
                spawnPos = new Vector2(transform.position.x, Random.Range(-3.0f, 5.0f));
            }
            //spawns on right side of screen
            else if ((side == 1) && gameObject.CompareTag("Spawn Right"))
            {
                spawnPos = new Vector2(transform.position.x, Random.Range(-3.0f, 5.0f));
            }

            Instantiate(cloudPrefab, spawnPos, cloudPrefab.transform.rotation);
    }
}
