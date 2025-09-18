using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 3.0f;
    [SerializeField] private int numberOfMonsters = 2;

    private GameObject[] monsterPrefabs;
    private const int maxMonster = 20;
    private int currentMonsterCount = 0;
    public int targetKillCount = 0;
    public int currentKillCount = 0;
    private bool bossSpawned = false;
    private bool isSpawning = true;
    private List<GameObject> spawnedMonsters = new List<GameObject>();

    private Coroutine spawnCoroutine;

    private void Awake()
    {
        LoadStageData();
    }

    private void LoadStageData()
    {
        BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (stageData == null) return;

        if (stageData is HuntStageData huntStageData)
        {
            InitializeMonsterData(huntStageData.monsterPrefabs, huntStageData.targetKillCount);
            StartSpawningMonsters();
        }
        else if (stageData is BossStageData bossStageData)
        {
            InitializeMonsterData(new GameObject[] { bossStageData.bossPrefab }, bossStageData.targetKillCount);
            StartCoroutine(SpawnBossAfterInit());
        }
        else if (stageData is GuardianStageData guardianStageData)
        {
            InitializeMonsterData(guardianStageData.guardianPrefabs, 0);
            StartSpawningMonsters();
        }
    }

    private void InitializeMonsterData(GameObject[] prefabs, int killCount)
    {
        monsterPrefabs = prefabs;
        targetKillCount = killCount;

        foreach (var prefab in monsterPrefabs)
        {
            ObjectPoolManager.Instance.CreatePool(prefab, 10, this.transform); // 풀 사이즈 설정
        }
    }

    private void StartSpawningMonsters()
    {
        if (spawnCoroutine == null && isSpawning)
        {
            spawnCoroutine = StartCoroutine(SpawnMonsters());
        }
    }

    private IEnumerator SpawnMonsters()
    {
        while (isSpawning)
        {
            if (currentMonsterCount < maxMonster)
            {
                for (int i = 0; i < numberOfMonsters && currentMonsterCount < maxMonster; i++)
                {
                    SpawnMonster();
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnMonster()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        GameObject monster = ObjectPoolManager.Instance.GetFromPool(prefab, spawnPoint.position, spawnPoint.rotation);

        spawnedMonsters.Add(monster);
        currentMonsterCount++;
    }

    public void SpawnBoss()
    {
        if (bossSpawned || monsterPrefabs.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject boss = ObjectPoolManager.Instance.GetFromPool(monsterPrefabs[0], spawnPoint.position, spawnPoint.rotation);
        bossSpawned = true;
    }

    private IEnumerator SpawnBossAfterInit()
    {
        yield return new WaitForEndOfFrame();
        SpawnBoss();
    }

    public void MonsterDied(GameObject monster)
    {
        currentMonsterCount--;
        currentKillCount++;

        spawnedMonsters.Remove(monster);

        foreach (var prefab in monsterPrefabs)
        {
            if (monster.name.Contains(prefab.name)) // 정확한 방식이 필요하면 Monster 컴포넌트로 추적 가능
            {
                ObjectPoolManager.Instance.ReturnToPool(prefab, monster);
                break;
            }
        }

        if (targetKillCount > 0 && currentKillCount >= targetKillCount)
        {
            isSpawning = false;
            StopSpawningCoroutine();
            currentKillCount = targetKillCount;
            StartCoroutine(ShowClearPageAfterRemovingMonsters());
        }
        else
        {
            if (isSpawning && currentMonsterCount < maxMonster)
            {
                StartSpawningMonsters();
            }
        }
    }

    private void StopSpawningCoroutine()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator ShowClearPageAfterRemovingMonsters()
    {
        RemoveAllMonsters();
        yield return new WaitForSeconds(0.1f);
        StageController.Instance.ShowClearPage();
    }

    private void RemoveAllMonsters()
    {
        for (int i = spawnedMonsters.Count - 1; i >= 0; i--)
        {
            var monster = spawnedMonsters[i];
            if (monster != null)
            {
                foreach (var prefab in monsterPrefabs)
                {
                    if (monster.name.Contains(prefab.name))
                    {
                        ObjectPoolManager.Instance.ReturnToPool(prefab, monster);
                        break;
                    }
                }
            }
            spawnedMonsters.RemoveAt(i);
        }
    }

    private void OnDestroy()
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ClearAllPools();
        }
    }

}
