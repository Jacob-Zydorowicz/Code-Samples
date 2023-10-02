/*
 * CIS 350 Game Production
 * Jacob Zydorowicz, Ian Connors, Caleb Kahn
 * PlayerMovement.cs
 * Controls player movement and actions in overworld
 * Last Updated: October first 2023
 */
#region imported namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
#endregion

public class PlayerMovement : MonoBehaviour
{
    Header["MOVEMENT"]
    #region variables
    public float xSpeed;
    public float ySpeed;
    public float walkSpeed;
    public Animator Anim;
    public bool canMove;

    private PlayerStats drunkStat;
    private bool inDrunkenMovment;

    public float maxDistence = .3f;
    private Vector2 previousLocation;
    #endregion

    private void Start()
    {
        canMove = false;
        StartCoroutine(WaitOnStart());
        StartCoroutine(UpdatePosition());
        StartCoroutine(SpriteGlitch());
        drunkStat = GetComponent<PlayerStats>();
        inDrunkenMovment = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (drunkStat.attributes[1].value.BaseValue >= 3 && inDrunkenMovment == false)
        {
            Debug.Log("Corutine Started");
            StartCoroutine(DrunkenMovment());
        }
        if (drunkStat.attributes[1].value.BaseValue < 3 && Mathf.Sign(walkSpeed) == -1)
        {
            walkSpeed = walkSpeed * -1;
        }

        //determines player rotation based on input
        if (canMove)
        {
            xSpeed = Input.GetAxis("Horizontal");
            ySpeed = Input.GetAxis("Vertical");
            Vector2 addedPosition = new Vector2(xSpeed, ySpeed).normalized * walkSpeed * Time.deltaTime;
            if (Mathf.Abs(previousLocation.x - (transform.position.x + addedPosition.x)) < maxDistence && Mathf.Abs(previousLocation.y - (transform.position.y + addedPosition.y)) < maxDistence)
            {
                transform.Translate(addedPosition);
            }


            /*WalkDir:
             * 0: None
             * 1: Up
             * 2: Down
             * 3: Right
             * 4: Left
             */
            if (xSpeed == 0 && ySpeed == 0)
            {
                Anim.SetInteger("WalkDir", 0);
            }
            else if (xSpeed > 0)
            {
                Anim.SetInteger("WalkDir", 3);
            }
            else if (xSpeed < 0)
            {
                Anim.SetInteger("WalkDir", 4);
            }
            else if (ySpeed > 0)
            {
                Anim.SetInteger("WalkDir", 1);
            }
            else if (ySpeed < 0)
            {
                Anim.SetInteger("WalkDir", 2);
            }
        }
    }

    #region IEnumerators
    IEnumerator WaitOnStart()
    {
        yield return new WaitForSeconds(.5f);
        canMove = true;
    }

    IEnumerator DrunkenMovment()
    {
        inDrunkenMovment = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 7f));
        walkSpeed = walkSpeed * -1;
        inDrunkenMovment = false;
    }

    IEnumerator UpdatePosition()
    {
        while (true)
        {
            previousLocation = transform.position;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SpriteGlitch()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.1f, 2.1f));
            Anim.SetBool("Glitch", true);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.7f));
            Anim.SetBool("Glitch", false);
        }


    }
    #endregion
}

