/*
 * Jacob Zydorowicz
 * Project 2 Mole Mania
 * Detects if hammer hits mole and raises score
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMole : MonoBehaviour
{
    //public int score;
    public MoleMove moleMoveScript;
    private ScoreManager scoreManObj;

    void Start()
    {
        scoreManObj = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    //public ScoreManager scoreManObj;
    private void OnMouseDown()
    {
        //object click detection based on https://answers.unity.com/questions/1128405/how-do-i-detect-a-click-on-an-object-with-a-partic.html
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //if mole is clicked on, resets mole & raise score
            if (moleMoveScript.isUp && hit.collider.CompareTag("Mole"))
            {
                //score++;
                scoreManObj.score++; 

                //moleMoveScript.transform.Translate(Vector3.down * moleMoveScript.speed * Time.deltaTime); //<<<commented this out so we can still go back to it if we need to
                //moleMoveScript.isUp = false;

                Destroy(gameObject);
            }




        }
    }
}   
