using DigitalRuby.SimpleLUT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Anna Breuker, Jacob Zydorowicz, Caleb Kahn
 * Project 5 
 * Opens the fight menu when player touches a cloud.
 */

//this is supposed to be attached to the cloud prefab- might recode it to be attached to the player because this "FindGameObjectWithTag" isn't finding the game object with tag.
public class OpenFightMenu : MonoBehaviour
{
    private OverworldAnxietyEffect worldEffect;
    private PlayerStats enemyStats;

    public GameObject fightMenu;
    //public ParticleSystem smokeEffect;

    public Image enemyPortrait;
    public Text enemyNameDisplayed;
    public Enemy[] enemies;
    public Enemy finalBoss;
    public Enemy enemyEncountered;
    public GameObject enemyHealthBar;
    public GameObject bossAnim;

    public Text description;

    public GameObject[] attackButtons;
    public string[] attackNames;

    public AudioSource playerAudio;
    public AudioSource fightMusic;
    public AudioSource bossMusic;
    public AudioSource calmEndMusic;
    public AudioSource lossMusic;
    public AudioSource effectSource;

    public AudioClip winBattleSound;
    public AudioClip wrongChoice;
    public AudioClip encounterSound;
    public AudioClip positiveAction;
    public AudioClip negativeAction;

    public float menuDelayTime = 2f;
    private float timer;
    private PlayerMovement player;
    public Image darknessEffect;
    public bool startingBattle = false;

    public int encounterNum = 0;
    public int enemyChoice = 0;

    public SimpleLUT cameraLUT;

    void Start()
    {
        fightMusic.Stop();
        bossMusic.Stop();
        calmEndMusic.Stop();
        lossMusic.Stop();
        effectSource.Stop();

        enemyStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        //fightMenu = GameObject.FindGameObjectWithTag("FightMenu");
        worldEffect = GameObject.FindGameObjectWithTag("AnxietyEffect").GetComponent<OverworldAnxietyEffect>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        darknessEffect = GameObject.FindGameObjectWithTag("Darkness Effect").GetComponent<Image>();
        //description = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInChildren<Text>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cloud") && !startingBattle)
        {
            Debug.Log("Normal Fight Start");
            StartCoroutine(OpenMenuOnDelay(other.gameObject));
        }
        else if (other.CompareTag("Tutorial Cloud") && !startingBattle)
        {
            Debug.Log("Tutorial Fight Start");
            StartCoroutine(OpenMenuOnDelay(other.gameObject));
        }
        else if (other.CompareTag("Lightbulb") && !startingBattle)
        {
            Debug.Log("Lightbulb Fight Start");
            StartCoroutine(StartLightbulbFight(other.gameObject));
        }
        else if (other.CompareTag("Final Boss Cloud") && !startingBattle)
        {
            Debug.Log("Boss Fight Start");
            StartCoroutine(StartBossFight(other.gameObject));
        }
    }

    IEnumerator OpenMenuOnDelay(GameObject cloud)
    {
        playerAudio.Stop();
        effectSource.PlayOneShot(encounterSound, .75f);
        enemyPortrait.color = Color.white;
        if (cloud.GetComponent<CloudMovement>().smoke != null)
        {
            cloud.GetComponent<CloudMovement>().smoke.gameObject.SetActive(true);
        }

        if (!fightMusic.isPlaying)
        {
            fightMusic.PlayDelayed(2.5f);
        }

        player.canMove = false;
        worldEffect.inBattle = true;
        worldEffect.isStart = false;
        startingBattle = true;
        cloud.GetComponent<CloudMovement>().inBattle = true;
        GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");
        GameObject[] effects = GameObject.FindGameObjectsWithTag("PhysicalAnxietyEffect");
        float[] alpha = new float[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].GetComponent<OverworldEffectMovement>().inBattle = true;
            alpha[i] = effects[i].GetComponent<SpriteRenderer>().color.a;
        }
        for (int i = 0; i < clouds.Length; i++)
        {
            clouds[i].GetComponent<CloudMovement>().canDie = false;
        }
        //float darknessAlpha = darknessEffect.color.a;
        float darknessAlpha = -cameraLUT.Brightness;
        if (darknessAlpha == 0)
        {
            darknessAlpha = .01f;
        }
        timer = 0;
        while (timer < menuDelayTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            for (int i = 0; i < clouds.Length; i++)
            {
                if (clouds[i] != cloud)
                {
                    clouds[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (menuDelayTime - timer) / menuDelayTime);
                }
            }
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha[i] * (menuDelayTime - timer) / menuDelayTime);
            }
            cameraLUT.Brightness = -darknessAlpha * (menuDelayTime - timer) / menuDelayTime;
            //darknessEffect.color = new Color(0f, 0f, 0f, darknessAlpha * (menuDelayTime - timer) / menuDelayTime);
        }

        //When the time has been waited
        if (cloud.CompareTag("Tutorial Cloud"))
        {
            cloud.SetActive(false);
        }
        for (int i = 0; i < clouds.Length; i++)
        {
            Destroy(clouds[i]);
        }
        for (int i = 0; i < effects.Length; i++)
        {
            Destroy(effects[i]);
        }
        startingBattle = false;
        fightMenu.SetActive(true);

        //setting up the menu for the specific enemy
        if (encounterNum == 0)
        {
            description.text = "A problem appears! You can see the problem's name and information on the left side of the screen.\nPress[space] to continue.";
        }
        if (encounterNum >= 1)
        {
            description.text = "A problem appears...";
        }
        int enemyNum = Random.Range(0, 2);
        if (enemyChoice > enemies.Length)
        {
            enemyNum = Random.Range(0, enemies.Length);
        }
        else if (enemyNum == 0)//50%
        {
            enemyNum = Random.Range(0, enemyChoice);
        }
        else//50%
        {
            enemyNum = enemyChoice - 1;
        }
        enemyEncountered = enemies[enemyNum];
        enemyPortrait.sprite = enemies[enemyNum].enemySprite;
        enemyNameDisplayed.text = enemies[enemyNum].enemyName;
        enemyStats.attributes[2].value.BaseValue = enemies[enemyNum].health;
        enemyHealthBar.GetComponent<ProgressBar>().maximum = enemies[enemyNum].health;

        for (int i = 0; i < attackButtons.Length; i++)
        {
            //Debug.Log(attackButtons[i].GetComponentInChildren<Text>().text);
            if (enemyNameDisplayed.text == "Glass Eye")
            {
                if (enemyStats.Lightbulb01pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb02pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[i];
                }
            }
            if (enemyNameDisplayed.text == "Liar Smiler")
            {
                if (enemyStats.Lightbulb05pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb06pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[4 + i];
                }
            }
            if (enemyNameDisplayed.text == "Scramble Sound")
            {
                if (enemyStats.Lightbulb07pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb08pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[8 + i];
                }
            }
            if (enemyNameDisplayed.text == "Question Air")
            {
                if (enemyStats.Lightbulb03pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb04pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[12 + i];
                }
            }
        }
    }

    IEnumerator StartLightbulbFight(GameObject lightbulb)
    {
        playerAudio.Stop();
        effectSource.PlayOneShot(encounterSound, .75f);
        player.canMove = false;
        worldEffect.inBattle = true;
        enemyPortrait.color = Color.white;
        startingBattle = true;
        GameObject[] clouds = GameObject.FindGameObjectsWithTag("Cloud");
        GameObject[] effects = GameObject.FindGameObjectsWithTag("PhysicalAnxietyEffect");

        playerAudio.Stop();
        if (!fightMusic.isPlaying)
        {
            fightMusic.PlayDelayed(2.5f);
        }

        float[] alpha = new float[effects.Length];
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i].GetComponent<OverworldEffectMovement>().inBattle = true;
            alpha[i] = effects[i].GetComponent<SpriteRenderer>().color.a;
        }
        for (int i = 0; i < clouds.Length; i++)
        {
            clouds[i].GetComponent<CloudMovement>().canDie = false;
        }
        //float darknessAlpha = darknessEffect.color.a;
        float darknessAlpha = -cameraLUT.Brightness;
        if (darknessAlpha == 0)
        {
            darknessAlpha = .01f;
        }
        timer = 0;
        while (timer < menuDelayTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            for (int i = 0; i < clouds.Length; i++)
            {
                clouds[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (menuDelayTime - timer) / menuDelayTime);
            }
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha[i] * (menuDelayTime - timer) / menuDelayTime);
            }
            cameraLUT.Brightness = -darknessAlpha * (menuDelayTime - timer) / menuDelayTime;
            //darknessEffect.color = new Color(0f, 0f, 0f, darknessAlpha * (menuDelayTime - timer) / menuDelayTime);
        }

        //When the time has been waited
        Destroy(lightbulb);
        for (int i = 0; i < clouds.Length; i++)
        {
            Destroy(clouds[i]);
        }
        for (int i = 0; i < effects.Length; i++)
        {
            Destroy(effects[i]);
        }
        startingBattle = false;
        fightMenu.SetActive(true);

        //setting up the menu for the specific enemy
        description.text = "A problem appears...";
        int enemyNum = (enemyChoice % enemies.Length) - 1;
        if (enemyNum == -1)
        {
            enemyNum = enemies.Length - 1;
        }
        enemyEncountered = enemies[enemyNum];
        enemyPortrait.sprite = enemies[enemyNum].enemySprite;
        enemyNameDisplayed.text = enemies[enemyNum].enemyName;
        enemyStats.attributes[2].value.BaseValue = enemies[enemyNum].health;
        enemyHealthBar.GetComponent<ProgressBar>().maximum = enemies[enemyNum].health;

        for (int i = 0; i < attackButtons.Length; i++)
        {
            //Debug.Log(attackButtons[i].GetComponentInChildren<Text>().text);
            if (enemyNameDisplayed.text == "Glass Eye")
            {
                if (enemyStats.Lightbulb01pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb02pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[i];
                }
            }
            if (enemyNameDisplayed.text == "Liar Smiler")
            {
                if (enemyStats.Lightbulb05pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb06pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[4 + i];
                }
            }
            if (enemyNameDisplayed.text == "Scramble Sound")
            {
                if (enemyStats.Lightbulb07pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb08pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[8 + i];
                }
            }
            if (enemyNameDisplayed.text == "Question Air")
            {
                if (enemyStats.Lightbulb03pickedup == false && i == 0)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else if (enemyStats.Lightbulb04pickedup == false && i == 1)
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = "";
                }
                else
                {
                    attackButtons[i].GetComponentInChildren<Text>().text = attackNames[12 + i];
                }
            }
        }
    }

    IEnumerator StartBossFight(GameObject boss)
    {
        playerAudio.Stop();
        effectSource.PlayOneShot(encounterSound, .75f);

        if (!bossMusic.isPlaying)
        {
            bossMusic.PlayDelayed(2.5f);
        }

      
        player.canMove = false;
        worldEffect.inBattle = true;
        startingBattle = true;
        yield return new WaitForSeconds(menuDelayTime);

        //When the time has been waited
        boss.SetActive(false);
        startingBattle = false;
        fightMenu.SetActive(true);

        //setting up the menu for the specific enemy
        description.text = "You feel a chill run up your spine...";
        enemyEncountered = finalBoss;
        enemyPortrait.sprite = null;
        enemyPortrait.color = Color.grey;
        bossAnim.SetActive(true);
        enemyNameDisplayed.text = finalBoss.enemyName;
        enemyStats.attributes[2].value.BaseValue = finalBoss.health;
        enemyHealthBar.GetComponent<ProgressBar>().maximum = finalBoss.health;
    }

    //enemyPortrait.sprite = enemies[1].enemySprite;
}
