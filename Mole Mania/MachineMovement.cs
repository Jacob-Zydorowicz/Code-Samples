/*
 * Jacob Zydorowicz, Anna Breuker, Ian Connors
 * Project 2 Mole Mania
 * Moves crit spots on machine based on running timer
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineMovement : MonoBehaviour
{
    public int correctPats;
    public int phaseNum;

    private bool activePattern;
    bool correctPattern;
    public bool firstLight;
    private bool activeHealth;

    public Vector3 newTopLightPos, oldTopPos, newBotLightPos, oldBotPos, oldHatchPos, newHatchPos;

    public GameObject[] lights;
    public Slider healthBar;
    public GameObject healthBarGameObject;

    public SpawnManager spawnManager;
    public ScoreManager scoreManager;

    public Timer timer;

    public Queue<int> lightQueue;

    public Material[] machineMat;
    private bool matChange = false;

    public AudioSource MachineSFX;
    public AudioClip machineHitSound;
    public AudioSource failedPatternSound;
    public AudioSource normalPlayMusic;
    public AudioSource goodEndingMusic;

    void Start()
    {
        activeHealth = false;
        firstLight = false;
        correctPattern = false;
        activePattern = false;

        phaseNum = 0;
        correctPats = 0;
        correctPats = 0;

        lightQueue = new Queue<int> { };

        healthBarGameObject = GameObject.FindGameObjectWithTag("Slider");
        healthBar = GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>();
        
        healthBarGameObject.SetActive(false);

        //gets current and future positons of moving lights
        newTopLightPos = new Vector3(lights[4].gameObject.transform.position.x, lights[4].gameObject.transform.position.y, lights[4].gameObject.transform.position.z) + new Vector3(0f, 3f, 0f);
        oldTopPos = new Vector3(lights[4].gameObject.transform.position.x, lights[4].gameObject.transform.position.y, lights[4].gameObject.transform.position.z);

        newBotLightPos = new Vector3(lights[3].gameObject.transform.position.x, lights[3].gameObject.transform.position.y, lights[3].gameObject.transform.position.z) + new Vector3(2f, 0f, 0f);
        oldBotPos = new Vector3(lights[3].gameObject.transform.position.x, lights[3].gameObject.transform.position.y, lights[3].gameObject.transform.position.z);

        oldHatchPos = new Vector3(GameObject.FindGameObjectWithTag("Hatch").transform.position.x, GameObject.FindGameObjectWithTag("Hatch").transform.position.y, GameObject.FindGameObjectWithTag("Hatch").transform.position.z);
        newHatchPos = new Vector3(GameObject.FindGameObjectWithTag("Hatch").transform.position.x, GameObject.FindGameObjectWithTag("Hatch").transform.position.y, GameObject.FindGameObjectWithTag("Hatch").transform.position.z) + new Vector3(0f, -3f, 0f);
    }

   
    void Update()
    {
        //"Phase 1" of machine where player plays simons says with machine lights
        if (!timer.gameOver&&timer.timerIsRunning)
        {
          

            //activates healthbar when first light is hit
            if(!activeHealth && firstLight)
            {
                activeHealth = true;
                healthBarGameObject.SetActive(true);
            }

            //sets healthbar and texture based on how many times the player gets a correct hammer pattern
            if(correctPats ==0)
            {
                healthBar.value = 100;
            }

            else if (correctPats == 1)
            {
                if (!matChange)
                    StartCoroutine(machineBreakFlashiness(machineMat[1], machineMat[0], 5));
                healthBar.value = 89;
            }
            else if(correctPats == 2)
            {
                matChange = false;
                healthBar.value = 78;
            }

            else if (correctPats == 3)
            {
                if (!matChange)
                    StartCoroutine(machineBreakFlashiness(machineMat[0], machineMat[2], 19));
                healthBar.value = 65;
            }
            else if (correctPats == 4)
            {
                matChange = false;
                healthBar.value = 54;
            }

            else if (correctPats == 5)
            {
                if (!matChange)
                    StartCoroutine(machineBreakFlashiness(machineMat[2], machineMat[1], 13));
                healthBar.value = 43;
            }
            else if (correctPats == 6)
            {
                matChange = false;
                healthBar.value = 32;
            }

            else if (correctPats == 7)
            {
                if (!matChange)
                    StartCoroutine(machineBreakFlashiness(machineMat[2], machineMat[3], 10));
                healthBar.value = 21;
            }
            else if (correctPats == 8)
            {
                matChange = false;
                healthBar.value = 10;
            }

            else if(correctPats == 9)
            {
                if (!matChange)
                    StartCoroutine(machineBreakFlashiness(machineMat[3], machineMat[4], 30));
                timer.gameOver = true;
                healthBar.value = 0;
                if (scoreManager.score == 0)
				{
                    spawnManager.goodEnd = true;
				}
                StopAllCoroutines();
                normalPlayMusic.Stop();
                goodEndingMusic.Play();
            }

            //starts light pattern when one is not actively showing
            if (!activePattern && !timer.gameOver)
            {
                activePattern = true;
                StartCoroutine(patternCoroutine(randomPattern()));
            }
        }
     
    }

    IEnumerator machineBreakFlashiness(Material oldMat, Material newMat, int flashes)
	{
        matChange = true;
        for (int i = 0; i < flashes; i++)
		{
            gameObject.GetComponent<Renderer>().material = newMat;
            yield return new WaitForSeconds(0.05f);
            gameObject.GetComponent<Renderer>().material = oldMat;
            yield return new WaitForSeconds(0.05f);
		}
        gameObject.GetComponent<Renderer>().material = newMat;
    }

    //decides how to activate lights based on passed in light
    //parameter: currentlight - light being activated from an array of lights
    public void lightsActivate(GameObject currentLight)
    {
        if(currentLight.CompareTag("topTriLight"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(169, 0, 0, 0));
            currentLight.transform.position = newTopLightPos;
        }
        else if(currentLight.CompareTag("botTriLight"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(169, 0, 0, 0));
            currentLight.transform.position = newBotLightPos;
        }
        else if(currentLight.CompareTag("TriangleLights"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(169, 0, 0, 0));
            GameObject.FindGameObjectWithTag("Hatch").transform.position = newHatchPos;

        }
        else if(currentLight.CompareTag("CircleLights"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(169, 0, 0, 0));
        }
        
    }

    //decides how to deactivate lights based on passed in light
    //parameter: currentlight - light being deactivated from an array of lights
    public void lightsDeactivate(GameObject currentLight)
    {
        if (currentLight.CompareTag("topTriLight"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(40, 0, 0, 0));
        }
        else if (currentLight.CompareTag("botTriLight"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(40, 0, 0, 0));
        }
        else if (currentLight.CompareTag("TriangleLights"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(40, 0, 0, 0));

        }
        else if(currentLight.CompareTag("CircleLights"))
        {
            currentLight.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color32(40, 0, 0, 0));
        }
    }

    //displays light pattern and determines if player input it correctly
    //parameter: lightOrder - array of machine light indexes that represent the order of the pattern
    IEnumerator patternCoroutine(int[] lightOrder)
    {
        float waitTime = Random.Range(.5f, 1f);
        int timeInBetween = Random.Range(4, 5);

        yield return new WaitForSeconds(Random.Range(10, 15));

        do
        {
            foreach(int light in lightOrder)
            {
                print(light);
            }
            yield return new WaitForSeconds(timeInBetween);

            //disables all lights be default
            for (int i = 0; i < lightOrder.Length; i++)
            {
                lights[i].GetComponent<HitMachine>().canHit = false;
                lightsDeactivate(lights[i]);
            }

            //activates and deactivates lights in order based on parameter
            for(int i =0; i < lightOrder.Length;i ++)
            {
                lightsActivate(lights[lightOrder[i]]);
                yield return new WaitForSeconds(waitTime);
                lightsDeactivate(lights[lightOrder[i]]);
            }
            
            //enables all lights to be hit
            for (int i = 0; i < lightOrder.Length; i++)
            {
                lights[i].GetComponent<HitMachine>().canHit = true;
            }

            float playerInputWindow = waitForSecs(7f);

            yield return new WaitUntil(() => ((lightQueue.Count == 5)||(timer.timeRemaining <= playerInputWindow)));

            //checks if player correctly inputs light order
            int []lightCheck = lightQueue.ToArray();

            //only checks correct pattern if minimum number of lights were hit
            if(lightCheck.Length==5)
            {
                for (int i = 0; i < lightCheck.Length; i++)
                {
                    if (!lightCheck[i].Equals(lightOrder[i]))
                    {
                        correctPattern = false;
                        failedPatternSound.Play();
                        break;
                    }
                    else
                    {
                        correctPattern = true;
                    }
                }
            }
            //only plays in correct sound if more than 1 light is hit to allow player to accidently discover mechanics
            else if (lightCheck.Length >= 1)
            {
                failedPatternSound.Play();
                correctPattern = false;
            }
            else if (firstLight)
            {
                failedPatternSound.Play();
                correctPattern = false;
            }


            lightQueue.Clear();

            for (int i = 0; i < lightOrder.Length; i++)
            {
                lightsDeactivate(lights[lightOrder[i]]);
            }
        }
        while (!correctPattern);
        MachineSFX.PlayOneShot(machineHitSound, 1.0f);
        correctPats++;
        activePattern = false;
    }

    //generates a array with random non repeating values from 0-4
    private int[] randomPattern()
    {

        List<int> nums = new List<int> { 0, 1, 2, 3, 4 };
        List<int> listPat = new List<int> { };

        while(listPat.Count<5)
        {
            int next = Random.Range(0, 5);

            if(nums.Contains(next))
            {
                nums.Remove(next);
                listPat.Add(next);
            }
        }

        int[] pattern = listPat.ToArray();

        return pattern;
        
    }

    private float waitForSecs(float seconds)
    {
    
        float waitForTime = timer.timeRemaining - seconds;

        if (waitForTime > 0)
        {
            print(waitForTime);
            return waitForTime;
        }
        else
            return 0f;
       
    }
}