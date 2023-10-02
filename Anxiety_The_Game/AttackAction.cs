using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Noah Trillizio, Anna Breuker, Jacob Zydorowicz
 * Project 5 
 * Controls what happens when an attack button is pressed
 */

public class AttackAction : MonoBehaviour
{
    private PlayerStats playerStats;
    private ClickedAttack clickedAttack;
    private OverworldAnxietyEffect cloudSpwanRate;
    private OpenFightMenu enemy;
    private EnemiesTurn enemyTurn;
    private CloseFightMenu endEncounter;

    private AudioSource MainMusic;

    public int numOfEncounters;
    public bool sameBattle;
    public int numOfDrunkenEncounters;

    private RandomNumGen randomNum;
    private OpenFightMenu openFMScript;

    private Text description;
    private BattleTutorial battleTutorial;

    // Start is called before the first frame update
    void Start()
    {
        randomNum = GameObject.FindGameObjectWithTag("Player").GetComponent<RandomNumGen>();
        openFMScript = GameObject.FindGameObjectWithTag("Player").GetComponent<OpenFightMenu>();
        MainMusic = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        endEncounter = GameObject.FindGameObjectWithTag("FightMenu").GetComponent<CloseFightMenu>();
        enemyTurn = GetComponent<EnemiesTurn>();
        enemy = GameObject.FindGameObjectWithTag("Player").GetComponent<OpenFightMenu>();
        cloudSpwanRate = GameObject.FindGameObjectWithTag("AnxietyEffect").GetComponent<OverworldAnxietyEffect>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        clickedAttack = GetComponent<ClickedAttack>();
        numOfEncounters = 0;
        sameBattle = false;
        numOfDrunkenEncounters = 0;

        description = GameObject.FindGameObjectWithTag("DescriptionBox").GetComponentInChildren<Text>();
        battleTutorial = FindObjectOfType<BattleTutorial>();
    }


    // gets called when attack button is pressed in the fight menu
    public void playerAttacks(GameObject attackButton)
    {
        battleTutorial.phase = 100;
        if (enemy.enemyNameDisplayed.text == "Glass Eye") //Glass-Eye
        {
            if (attackButton.tag == "Attack 1")
            {
                TripleRule();
            }
            if (attackButton.tag == "Attack 2")
            {
                EmotionalSupport();
            }
            if (attackButton.tag == "Attack 3")
            {
                SelfDoubt();
            }
            if (attackButton.tag == "Attack 4")
            {
                TakeOffGlasses();
            }
        }
        else if (enemy.enemyNameDisplayed.text == "Liar Smiler") //Lier Smiler
        {
            if (attackButton.tag == "Attack 1")
            {
                Grounding();
            }
            if (attackButton.tag == "Attack 2")
            {
                GoToSleep();
            }
            if (attackButton.tag == "Attack 3")
            {
                DrinkToForget();
            }
            if (attackButton.tag == "Attack 4")
            {
                Isolation();
            }
        }
        else if (enemy.enemyNameDisplayed.text == "Scramble Sound") //Scrambled Sound
        {
            if (attackButton.tag == "Attack 1")
            {
                BlastMusic();
            }
            if (attackButton.tag == "Attack 2")
            {
                BoxBreath();
            }
            if (attackButton.tag == "Attack 3")
            {
                LeaveTheRoom();
            }
            if (attackButton.tag == "Attack 4")
            {
                PunchAWall();
            }
        }
        else if (enemy.enemyNameDisplayed.text == "Question Air") //Question-Air
        {
            if (attackButton.tag == "Attack 1")
            {
                Hide();
            }
            if (attackButton.tag == "Attack 2")
            {
                ShiftFocus();
            }
            if (attackButton.tag == "Attack 3")
            {
                ShutDown();
            }
            if (attackButton.tag == "Attack 4")
            {
                Visualization();
            }
        }
        else if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety") //Final Boss
        {
            Debug.Log("Random Num in AttackAction = " + randomNum.randomNum);
            if (randomNum.randomNum == 1)
            {
                if (attackButton.tag == "Attack 1")
                {
                    TripleRule();
                }
                if (attackButton.tag == "Attack 2")
                {
                    EmotionalSupport();
                }
                if (attackButton.tag == "Attack 3")
                {
                    SelfDoubt();
                }
                if (attackButton.tag == "Attack 4")
                {
                    TakeOffGlasses();
                }
            }
            else if (randomNum.randomNum == 2)
            {
                if (attackButton.tag == "Attack 1")
                {
                    Hide();
                }
                if (attackButton.tag == "Attack 2")
                {
                    ShiftFocus();
                }
                if (attackButton.tag == "Attack 3")
                {
                    ShutDown();
                }
                if (attackButton.tag == "Attack 4")
                {
                    Visualization();
                }
            }
            else if (randomNum.randomNum == 3)
            {
                if (attackButton.tag == "Attack 1")
                {
                    Grounding();
                }
                if (attackButton.tag == "Attack 2")
                {
                    GoToSleep();
                }
                if (attackButton.tag == "Attack 3")
                {
                    DrinkToForget();
                }
                if (attackButton.tag == "Attack 4")
                {
                    Isolation();
                }
            }
            else
            {
                if (attackButton.tag == "Attack 1")
                {
                    BlastMusic();
                }
                if (attackButton.tag == "Attack 2")
                {
                    BoxBreath();
                }
                if (attackButton.tag == "Attack 3")
                {
                    LeaveTheRoom();
                }
                if (attackButton.tag == "Attack 4")
                {
                    PunchAWall();
                }
            }
        }
        enemyTurn.enemyTurn();
        //clickedAttack.changeAttack = true;
    }

    public void TripleRule()
    {
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
        description.text = "You focus on 3 things you can see, 3 things you can hear, " +
            "and 3 things you can feel, moving 3 different parts of your body." +
            " Taking a deep breath, you feel lighter.\n<Press SPACE To Continue>";
    }
    public void Grounding()
    {
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
        description.text = "You focus on 5 things you can see, 4 you can touch, " +
            "3 you can hear, 2 you can smell, and 1 you can taste. You feel grounded.\n<Press SPACE To Continue>";
    }

    public void BlastMusic()
    {
        description.text = "You turn the music up. The noise seems fainter.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
        StartCoroutine(MusicChange());
        //m_MyAudioSource.volume = m_MySliderValue;
    }

    public void BoxBreath()
    {
        description.text = "You breathe in for 4, hold for 4, out for 4, hold for 4. As you repeat, the noise seems fainter.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
    }

    public void DrinkToForget()
    {
        description.text = "Maybe drinking will help?\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
    }

    public void EmotionalSupport()
    {
        description.text = "You ask a friend for reassurance. They smile and say you look fine.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
    }

    public void GoToSleep()
    {
        description.text = "There's nothing a good nap can't fix.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
        // skip next two turns
    }

    public void Hide()
    {
        description.text = "It's too much. Everyone's staring. You can't do this.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void Isolation()
    {
        description.text = "They're right, none of my friends actually care.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void LeaveTheRoom()
    {
        description.text = "You know what? This isn't worth it. You leave the room.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void PunchAWall()
    {
        description.text = "You punch a wall out of frustration. ...that kind of hurt.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
    }

    public void SelfDoubt()
    {
        description.text = "Maybe they're right... I do look stupid.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void shiftDoubt()
    {
        changeStats();
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void ShiftFocus()
    {
        description.text = "You shift your focus to something other than the people around you.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
    }

    public void ShutDown()
    {
        description.text = "This isn't worth it- you can't do this. Who ever wanted to actually ask questions anyway?\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        cloudSpwanRate.maxCloudSpawnTime = (cloudSpwanRate.maxCloudSpawnTime) - .5f;//This doesn't work anymore
        // increase cloud spawn rate
        if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
        {
            Debug.Log("Failed to end encounter");
        }
        else
        {
            endEncounter.EndFightEarly();
        }
    }

    public void TakeOffGlasses()
    {
        description.text = "Who needs glasses anyways? Not you, that's for sure.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.negativeAction, .75f);
        changeStats();
        // blindness?
    }

    public void Visualization()
    {
        description.text = "You imagine yourself somewhere else. This is nice.\n<Press SPACE To Continue>";
        openFMScript.effectSource.PlayOneShot(openFMScript.positiveAction, .75f);
        changeStats();
        if (UnityEngine.Random.Range(1, 10) == 1)
        {
            if (enemy.enemyNameDisplayed.text == "You" || enemy.enemyNameDisplayed.text == "Your Anxiety")
            {
                Debug.Log("Failed to end encounter");
            }
            else
            {
                endEncounter.EndFightEarly();
            }
        }
    }

    public void changeStats()
    {
        Debug.Log(clickedAttack.attack.data.Name);
        for (int i = 0; i < playerStats.attributes.Length; i++)
        {
            for (int j = 0; j < clickedAttack.attack.data.buffs.Length; j++)
            {
                if (playerStats.attributes[i].type == clickedAttack.attack.data.buffs[j].attribute)
                {
                    clickedAttack.attack.data.buffs[j].GenerateValue();
                    if (playerStats.attributes[i] == playerStats.attributes[0])
                    {
                        if (playerStats.attributes[0].value.BaseValue <= ((clickedAttack.attack.data.buffs[j].value) * -1))
                        {
                            playerStats.attributes[i].value.BaseValue = 0;
                        }
                        else
                        {
                            playerStats.attributes[i].value.BaseValue = (playerStats.attributes[i].value.BaseValue) + clickedAttack.attack.data.buffs[j].value;
                        }
                        Debug.Log(string.Concat(playerStats.attributes[i].type, " was updated! Value is now ", playerStats.attributes[i].value.ModifiedValue));
                    }
                    else
                    {
                        playerStats.attributes[i].value.BaseValue = (playerStats.attributes[i].value.BaseValue) + clickedAttack.attack.data.buffs[j].value;
                        Debug.Log(string.Concat(playerStats.attributes[i].type, " was updated! Value is now ", playerStats.attributes[i].value.ModifiedValue));
                    }
                }
            }
        }
    }

    IEnumerator MusicChange()
    {
        MainMusic.volume = .9f;
        yield return new WaitForSeconds(3f);
        MainMusic.volume = .35f;
    }
}
