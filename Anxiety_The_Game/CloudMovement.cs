/*
 * CIS 350 Game Production
 * Jacob Zydorowicz, Caleb Kahn
 * Anxiety The Game
 * Allows clouds to fly towards player
 * Last Updated: October first 2023
 */
#region imported namespaces
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
#endregion

public class CloudMovement : MonoBehaviour
{
    Header["Cloud movement"]
    #region Variables
    private Transform player;
    public float averageSpeed = 3;
    public bool inBattle = false;
    private float speed;
    private float minDistance = 10f;
    private float range;

    private Vector3 direction;

    private bool increaseSpeed = false;
    public bool canDie = true;
    public ParticleSystem smoke;
    #endregion

    private void Start()
    {
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
        }
    }
}
