/* Caleb Kahn, Jacob Zydorowicz
 * Project 5
 * While the player is in the overworld, this script increases the effects of anxiety over time and applies them
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DigitalRuby.SimpleLUT;

public class OverworldAnxietyEffect : MonoBehaviour
{
    public GameObject[] imagePrefabs;
    public bool inBattle = true;
    public Image darknessEffect;
    public CinemachineVirtualCamera cinemachine;
    public GameObject player;

    private float timer = 0;
    private float randomTimer = 0;
    private float cooldownTimer = 0;
    private float minCooldown = 3f;
    private float pov = 3.5f;
    private int randomChance = 120;
    private float effectTimer = 3f;
    private float alpha = 0f;
    private float minAlpha = 0f;
    private float maxAlpha = .1f;
    private float alphaChangeTime = 2f;
    private bool alphaUp = true;

    public GameObject neuronPrefab;
    public GameObject cloudPrefab;
    public float minCloudSpawnTime = 5f;
    public float maxCloudSpawnTime = 12f;
    private bool skipedFirst = false;

    private float simSpeed = 1f;
    public SimpleLUT cameraLUT;
    public bool isStart = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCloudPrefab());
        StartCoroutine(SpawnNeuronEffect());
    }

    IEnumerator SpawnNeuronEffect()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (!inBattle || isStart)
            {
                GameObject neuron = Instantiate(neuronPrefab, player.transform.position, neuronPrefab.transform.rotation);
                var main = neuron.GetComponent<ParticleSystem>().main;
                main.simulationSpeed = simSpeed;
                yield return new WaitForSeconds(2f);
                Destroy(neuron);
            }
        }
    }

    IEnumerator SpawnCloudPrefab()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minCloudSpawnTime, maxCloudSpawnTime));
            if (!inBattle)
            {
                if (skipedFirst)
                {
                    SpawnClouds();
                }
                else
                {
                    skipedFirst = true;
                }
            }
        }
    }

    private void SpawnClouds()
    {

        Vector2 spawnPos;
        int side = Random.Range(0, 4);

        //side = 0,left side
        //side = 1,right side
        if (side == 0) {//Top Side
            spawnPos =  new Vector2(Random.Range(-7f, 7f) + player.transform.position.x, 4f + player.transform.position.y);
        }
        else if (side == 1)
        {//Right Side
            spawnPos = new Vector2(7f + player.transform.position.x, Random.Range(-4f, 4f) + player.transform.position.y);
        }
        else if (side == 2)
        {//Bottom Side
            spawnPos = new Vector2(Random.Range(-7f, 7f) + player.transform.position.x, -4f + player.transform.position.y);
        }
        else
        {//Left Side
            spawnPos = new Vector2(-7f + player.transform.position.x, Random.Range(-4f, 4f) + player.transform.position.y);
        }

        float size = Random.Range(1.225f, 1.732f);//sqrt 1.5 - sqrt 3
        size *= size;
        Instantiate(cloudPrefab, spawnPos, cloudPrefab.transform.rotation).transform.localScale = new Vector3(size, size, size);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        randomTimer += Time.deltaTime;
        cooldownTimer += Time.deltaTime;
        if (!inBattle)
        {
            if (timer <= 30)
            {
                minCooldown = 3f - (1f * (timer / 30f));
                pov = 3.5f - (.5f * (timer / 30f));
                cinemachine.m_Lens.OrthographicSize = pov;
                randomChance = (int)(120f - (65f * (timer / 30f)));
                effectTimer = 3f + (1.5f * (timer / 30f));
                minAlpha = .3f * (timer / 30f);
                maxAlpha = .1f + (.4f * (timer / 30f));
                minCloudSpawnTime = 5f - (2f * (timer / 30f));
                maxCloudSpawnTime = 12f - (4f * (timer / 30f));
            }
            else if (timer <= 60)
            {
                minCooldown = 2f - (1.5f * ((timer - 30f) / 30f));
                pov = 3f - (.5f * ((timer - 30f) / 30f));
                cinemachine.m_Lens.OrthographicSize = pov;
                randomChance = (int)(65f - (40f * ((timer - 30f) / 30f)));
                effectTimer = 4.5f + (1.5f * ((timer - 30f) / 30f));
                minAlpha = .2f + (.3f * ((timer - 30f) / 30f));
                maxAlpha = .5f + (.5f * ((timer - 30f) / 30f));
                minCloudSpawnTime = 3f - (2f * ((timer - 30f) / 30f));
                maxCloudSpawnTime = 8f - (3f * ((timer - 30f) / 30f));
                simSpeed = 2f;
            }
            else
            {
                effectTimer = 6f + (1.5f * ((timer - 60f) / 30f));
                simSpeed = 4f;
            }
            if (randomTimer >= .1f && cooldownTimer >= minCooldown) {
                randomTimer = 0f;
                if (0 == Random.Range(0, randomChance)) {
                    cooldownTimer = 0f;
                    int num = Random.Range(0, imagePrefabs.Length);
                    GameObject effect = Instantiate(imagePrefabs[num], new Vector2(Random.Range(-7f, 7f) + player.transform.position.x, Random.Range(-4f, 4f) + player.transform.position.y), imagePrefabs[num].transform.rotation);
                    effect.GetComponent<OverworldEffectMovement>().effectTimer = effectTimer;
                }
            }
            if (alphaUp)
            {
                alpha += (maxAlpha - minAlpha) / alphaChangeTime * Time.deltaTime;
                if (alpha > maxAlpha)
                {
                    alpha = maxAlpha;
                    alphaUp = false;
                }
            }
            else
            {
                alpha -= (maxAlpha - minAlpha) / alphaChangeTime * Time.deltaTime;
                if (alpha < minAlpha)
                {
                    alpha = minAlpha;
                    alphaUp = true;
                }
            }
            cameraLUT.Brightness = -alpha;
            //darknessEffect.color = new Color(0f, 0f, 0f, alpha);
        }
    }

    public void resetVariables()
    {
        inBattle = false;
        timer = 0;
        randomTimer = 0;
        cooldownTimer = 0;
        minCooldown = 3f;
        pov = 3.5f;
        randomChance = 120;
        effectTimer = 3f;
        alpha = 0f;
        minAlpha = 0f;
        maxAlpha = .1f;
        alphaChangeTime = 2f;
        minCloudSpawnTime = 5f;
        maxCloudSpawnTime = 12f;
        simSpeed = 1f;
        alphaUp = true;
        skipedFirst = false;
    }
}
