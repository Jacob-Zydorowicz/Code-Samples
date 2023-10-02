/*
 * Jacob Zydorowicz, Caleb Kahn
 * Project 5
 * Allows clouds to fly towards player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private Transform player;
    public float averageSpeed = 3;
    public bool inBattle = false;
    private float speed;
    private float minDistance = 10f;
    private float range;

    private Vector3 direction;
    //private Vector3 leftBound;
    //private Vector3 rightBound;

    private bool increaseSpeed = false;
    public bool canDie = true;
    public ParticleSystem smoke;

    private void Start()
    {
        //leftBound = GameObject.FindGameObjectWithTag("Spawn Left").GetComponent<Transform>().position;
        //rightBound = GameObject.FindGameObjectWithTag("Spawn Right").GetComponent<Transform>().position;
        increaseSpeed = (Random.value > 0.5f);
        speed = Random.Range(averageSpeed / 2, averageSpeed * 3 / 2);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        direction = (player.position + new Vector3 (Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0) - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inBattle)
        {
            if (increaseSpeed)
            {
                speed += Time.deltaTime * averageSpeed;
                if (speed >= averageSpeed * 3 / 2)
                {
                    increaseSpeed = false;
                    speed = averageSpeed * 3 / 2;
                }
            }
            else
            {
                speed -= Time.deltaTime * averageSpeed;
                if (speed <= averageSpeed / 2)
                {
                    increaseSpeed = true;
                    speed = averageSpeed / 2;
                }
            }

            //finds direction for clouds to move in towards the player
            range = Vector2.Distance(transform.position, player.position);

            if (range < minDistance)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else if (canDie)
            {
                Destroy(gameObject);
            }
            /*
            if (transform.position.x < leftBound.x || transform.position.x > rightBound.x)
            {
                Destroy(gameObject);

            }*/
        }
    }
}
