using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*
 * Anna Breuker, Caleb Kahn, Jacob Zydorowicz
 * Project 5 
 * Closes the fight menu when a battle is won or lost.
 */
public class CloseFightMenu : MonoBehaviour
{
    private OverworldAnxietyEffect worldEffect;
    private PlayerStats playerStats;
    private Text description;

    public Enemy finalBoss;
    public GameObject bossAnim;

    private OpenFightMenu openFMScript;
    private HoverDescriptionVisable hoverDesc;

    public GameObject fightMenu;
    public OpenFightMenu fightMenuScript;
    public GameObject gameOverScreen;
    public GameObject youWinScreen;
    public bool gameOver;
    public bool win = false;

    public Button attack1;
    public Button attack2;
    public Button attack3;
    public Button attack4;

    private PlayerMovement player;
    private CheckpointTrigger[] checkpoints;

    // Start is called before the first frame update
    void Start()
    {
        openFMScript = GameObject.FindGameObjectWithTag("Player").GetComponent<OpenFightMenu>();

        hoverDesc = GameObject.FindGameObjectWithTag("Attack 1").GetComponent<HoverDescriptionVisable>();

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        //fightMenu = GameObject.FindGameObjectWithTag("FightMenu");
        fightMenuScript = GameObject.FindGameObjectWithTag("Player").GetComponent<OpenFightMenu>();
        worldEffect = GameObject.FindGameObjectWithTag("AnxietyEffect").GetComponent<OverworldAnxietyEffect>();
        description = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInChildren<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoints = new CheckpointTrigger[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            checkpoints[i] = temp[i].GetComponent<CheckpointTrigger>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.attributes[2].value.BaseValue <= 0 && fightMenuScript.enemyEncountered == finalBoss)
        {
            description.text = "Is this it..? Is it really over? [Press space to continue]";
            StartCoroutine(Win());
        }
        else if (playerStats.attributes[2].value.BaseValue <= 0)
        {
            description.text = "Problem defeated! [Press space to continue]";
            fightMenuScript.encounterNum++;
            StartCoroutine(BattleOver());

        }
        if (playerStats.attributes[0].value.BaseValue > 100 && !gameOver)
        {
            description.text = "You are overwhelmed... [Press space to continue]";
            StartCoroutine(Lose());
        }
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                gameOverScreen.SetActive(false);
                gameOver = false;
                for (int i = 0; i < checkpoints.Length; i++)
                {
                    if (checkpoints[i].currentCheckpoint)
                    {
                        checkpoints[i].Respawn();
                    }
                }
                fightMenu.SetActive(false);
                hoverDesc.mouseOver = false;
                hoverDesc.descriptionCanvas.SetActive(false);

                openFMScript.calmEndMusic.Stop();
                openFMScript.lossMusic.Stop();
                openFMScript.playerAudio.PlayDelayed(0.5f);
            }
        }
        if (win)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//We can add restarting in later
                youWinScreen.SetActive(false);
                win = false;
                fightMenu.SetActive(false);
                hoverDesc.mouseOver = false;
                hoverDesc.descriptionCanvas.SetActive(false);

                openFMScript.calmEndMusic.Stop();
                openFMScript.lossMusic.Stop();
                openFMScript.playerAudio.PlayDelayed(0.5f);
            }
        }
    }

    public void EndFightEarly()
    {
        Debug.Log("Ending Encounter");
        StartCoroutine(BattleOverEarly());
    }

    IEnumerator BattleOver()
    {
        openFMScript.fightMusic.Stop();
        openFMScript.bossMusic.Stop();
        openFMScript.effectSource.PlayOneShot(openFMScript.winBattleSound, .2f);
        openFMScript.playerAudio.PlayDelayed(0.5f);

        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
        attack4.enabled = false;
        while (!Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForFixedUpdate();
        }
        attack1.enabled = true;
        attack1.GetComponentInChildren<Text>().enabled = true;
        attack2.enabled = true;
        attack2.GetComponentInChildren<Text>().enabled = true;
        attack3.enabled = true;
        attack3.GetComponentInChildren<Text>().enabled = true;
        attack4.enabled = true;
        attack4.GetComponentInChildren<Text>().enabled = true;
        worldEffect.resetVariables();
        fightMenu.SetActive(false);
        player.canMove = true;
        playerStats.attributes[2].value.BaseValue = 1;
        if (playerStats.attributes[1].value.BaseValue > 0)
        {
            playerStats.attributes[1].value.BaseValue--;
        }
        hoverDesc.mouseOver = false;
        hoverDesc.descriptionCanvas.SetActive(false);
    }

    IEnumerator Win()
    {
        openFMScript.playerAudio.Stop();
        openFMScript.fightMusic.Stop();
        openFMScript.bossMusic.Stop();

        openFMScript.effectSource.PlayOneShot(openFMScript.winBattleSound, .2f);

        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
        attack4.enabled = false;
        win = true;
        yield return new WaitForSeconds(3);
        youWinScreen.SetActive(true);
        bossAnim.SetActive(false);

        if (!openFMScript.calmEndMusic.isPlaying)
        {
            openFMScript.calmEndMusic.Play();
        }
        hoverDesc.mouseOver = false;
        hoverDesc.descriptionCanvas.SetActive(false);
    }

    IEnumerator Lose()
    {
        openFMScript.playerAudio.Stop();
        openFMScript.fightMusic.Stop();
        openFMScript.bossMusic.Stop();

        openFMScript.effectSource.PlayOneShot(openFMScript.wrongChoice, .2f);

        attack1.enabled = false;
        attack2.enabled = false;
        attack3.enabled = false;
        attack4.enabled = false;
        gameOver = true;
        yield return new WaitForSeconds(3);
        gameOverScreen.SetActive(true);
        bossAnim.SetActive(false);

        if (!openFMScript.lossMusic.isPlaying)
        {
            openFMScript.lossMusic.Play();
        }
        hoverDesc.mouseOver = false;
        hoverDesc.descriptionCanvas.SetActive(false);

    }

    IEnumerator BattleOverEarly()
    {
        while (!Input.GetKey(KeyCode.Space))
        {
            yield return new WaitForFixedUpdate();
        }
        description.text = "You ran away.";
        yield return new WaitForSeconds(2f);
        worldEffect.resetVariables();
        fightMenu.SetActive(false);
        player.canMove = true;
        playerStats.attributes[2].value.BaseValue = 1;

        hoverDesc.mouseOver = false;
        hoverDesc.descriptionCanvas.SetActive(false);

        openFMScript.fightMusic.Stop();
        openFMScript.playerAudio.PlayDelayed(0.5f);
    }
}
