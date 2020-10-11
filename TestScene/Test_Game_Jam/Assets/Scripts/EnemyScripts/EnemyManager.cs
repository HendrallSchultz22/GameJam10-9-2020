using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public float MaxAgression = 10;
    public Transform player;

    int wave = 1;
    int enemiesAlive = 0;
    int enemiesToSpawn = 0;
    [SerializeField]
    [Tooltip("This is the maximum number of enemies that can be active at once. If this is reached and there are still more enemies this round" +
        ", the system will wait until an enemy is defeated to spawn the next one.")]
    int maxEnemiesAlive = 20;
    [SerializeField] GameObject skeleton;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField]
    [Tooltip("How long it takes for enemies to start spawning")]
    float waveStartDelay = 1;
    [SerializeField]
    [Tooltip("The minimum amount of time that must pass between spawns")]
    float spawnDelay = 0.5f;
    bool spawnReady = true;
    [SerializeField]
    [Tooltip("How likly a skeleton that spawns will be melee only. 1 = 100%")]
    [Range(0, 1)]
    float meleeChance = 0.5f;

    float healthScaler = 1;

    private void Start()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        if (spawnPoints.Length == 0) Debug.Log("EnemyManager has no spawn points");
        StartCoroutine(DelayStartWave());
    }

    private void Update()
    {
        if (spawnReady) SpawnLogic();
    }

    private void SpawnLogic()
    {
        if (enemiesToSpawn > 0 && enemiesAlive < maxEnemiesAlive)
        {
            SpawnSkeleton();
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            Debug.Log("Finshed wave " + wave);
            wave++;
            StartCoroutine(DelayStartWave());
        }
    }

    private void SpawnSkeleton()
    {
        GameObject newEnemy = Instantiate(skeleton);
        int index = Random.Range(0, spawnPoints.Length);
        newEnemy.transform.position = spawnPoints[index].position;
        SkeletonAI AI = newEnemy.GetComponent<SkeletonAI>();
        EnemyStats stats = newEnemy.GetComponent<EnemyStats>();
        AI.target = player;

        //determin the range of the skelton (stop them from just standing in a circle)
        AI.range = Random.Range(AI.meleeRange, AI.range);
        if (Random.value <= meleeChance) AI.range = AI.meleeRange;

        stats.health = stats.health * healthScaler;

        AI.agresstion = MaxAgression * (1 - (5 / (wave + 4)));

        enemiesAlive++;
        enemiesToSpawn--;
        StartCoroutine(DelayNextSpawn());
    }

    private void StartWave()
    {
        CalculateHealthScaler();
        int spawnLogorithm = wave * wave;
        for (int i = 0; i < wave; i++)
        {
            enemiesToSpawn++;
            if (wave < 13)
            {
                while (spawnLogorithm > 0)
                {
                    enemiesToSpawn++;
                    spawnLogorithm /= 2;
                }
            }
        }

        if (wave >= 13)
        {
            enemiesToSpawn += 100;
            while (spawnLogorithm > 0)
            {
                enemiesToSpawn++;
                spawnLogorithm /= 2;
            }
        }

    }

    //the enemies get tougher as the in higher rounds
    private void CalculateHealthScaler()
    {
        if (wave == 1) healthScaler = 1;
        else healthScaler = 2 * Mathf.Sqrt(wave - 1);
    }

    IEnumerator DelayStartWave()
    {
        yield return new WaitForSeconds(waveStartDelay);
        StartWave();
    }

    IEnumerator DelayNextSpawn()
    {
        spawnReady = false;
        yield return new WaitForSeconds(spawnDelay);
        spawnReady = true;
    }

    public int GetWave()
    {
        return wave;
    }
}
