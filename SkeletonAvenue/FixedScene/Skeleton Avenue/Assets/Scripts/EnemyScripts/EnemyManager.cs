using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public float MaxAgression = 10;
    public Transform player;

    [SerializeField] int wave = 1;
    [SerializeField] int enemiesAlive = 0;
    [SerializeField] int enemiesToSpawn = 0;
    [SerializeField]
    [Tooltip("This is the maximum number of enemies that can be active at once. If this is reached and there are still more enemies this round" +
        ", the system will wait until an enemy is defeated to spawn the next one.")]
    int maxEnemiesAlive = 20;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject pumkinBoss;
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
    bool spawnBoss = false;

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
            if (spawnBoss) SpawnEnemy(pumkinBoss);
            else SpawnEnemy(skeleton);
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive == 0)
        {
            WaveComplete?.Invoke();
            wave++;
            StartCoroutine(DelayStartWave());
        }
    }

    private void SpawnEnemy(GameObject enemy)
    {
        GameObject newEnemy = Instantiate(enemy);
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

        if (enemy = pumkinBoss)
        {
            spawnBoss = false;
            AI.agresstion = MaxAgression * (1 - (1 / (wave)));
            BossSpawned?.Invoke();
        }

        enemiesAlive++;
        enemiesToSpawn--;
        StartCoroutine(DelayNextSpawn());
    }

    //calculate the number of enemies that need to spawn this wave
    private void StartWave()
    {
        int seed = wave;
        if (wave % 5 == 0)
        {
            enemiesToSpawn++;
            spawnBoss = true;
            seed = wave / 5;
        }
        CalculateHealthScaler();
        int spawnLogorithm = seed * seed;
        for (int i = 0; i < seed; i++)
        {
            enemiesToSpawn++;
            if (seed < 13)
            {
                while (spawnLogorithm > 0)
                {
                    enemiesToSpawn++;
                    spawnLogorithm /= 2;
                }
            }
        }

        if (seed >= 13)
        {
            enemiesToSpawn += 100;
            while (spawnLogorithm > 0)
            {
                enemiesToSpawn++;
                spawnLogorithm /= 2;
            }
        }
        WaveStart?.Invoke();
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

    #region Delegates
    public delegate void CallBack();
    CallBack WaveStart;
    CallBack WaveComplete;
    CallBack BossSpawned;

    public void SubscribeWaveStart(CallBack aMethod) { WaveStart += aMethod; }
    public void SubscribeWaveComplete(CallBack aMethod) { WaveComplete += aMethod; }
    public void SubscribeBossSpawned(CallBack aMethod) { BossSpawned += aMethod; }

    public void UnsubscribeWaveStart(CallBack aMethod) { WaveStart -= aMethod; }
    public void UnsubscribeWaveComplete(CallBack aMethod) { WaveComplete -= aMethod; }
    public void UnsubscribeBossSpawned(CallBack aMethod) { BossSpawned -= aMethod; }

    #endregion
}
