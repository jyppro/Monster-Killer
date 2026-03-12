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
    
    [HideInInspector] public int targetKillCount = 0;
    [HideInInspector] public int currentKillCount = 0;
    
    private bool bossSpawned = false;
    private bool isSpawning = true;
    
    // 리스트 타입을 GameObject에서 PoolableObject로 관리하면 반환 시 유리합니다.
    private List<GameObject> spawnedMonsters = new List<GameObject>(maxMonster);

    private Coroutine spawnCoroutine;
    
    // 최적화: 가비지 컬렉션을 방지하기 위한 캐싱
    private WaitForSeconds _waitSpawnInterval;
    private WaitForEndOfFrame _waitForEndOfFrame;
    private WaitForSeconds _waitClearPage = new WaitForSeconds(0.1f);

    private void Awake()
    {
        _waitSpawnInterval = new WaitForSeconds(spawnInterval);
        _waitForEndOfFrame = new WaitForEndOfFrame();
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
            // 각 프리팹별로 풀 생성 (초기 사이즈 10)
            ObjectPoolManager.Instance.CreatePool(prefab, 10, this.transform);
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
            yield return _waitSpawnInterval; // 캐싱된 객체 사용으로 GC 방지
        }
    }

    private void SpawnMonster()
    {
        if (spawnPoints.Length == 0 || monsterPrefabs.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        
        // 풀에서 가져오기
        GameObject monster = ObjectPoolManager.Instance.GetFromPool(prefab, spawnPoint.position, spawnPoint.rotation);

        if (monster != null)
        {
            spawnedMonsters.Add(monster);
            currentMonsterCount++;
        }
    }

    public void SpawnBoss()
    {
        if (bossSpawned || monsterPrefabs.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        ObjectPoolManager.Instance.GetFromPool(monsterPrefabs[0], spawnPoint.position, spawnPoint.rotation);
        bossSpawned = true;
    }

    private IEnumerator SpawnBossAfterInit()
    {
        yield return _waitForEndOfFrame;
        SpawnBoss();
    }

    /// <summary>
    /// 몬스터 사망 시 외부(Monster 스크립트 등)에서 호출
    /// </summary>
    public void MonsterDied(GameObject monster)
    {
        currentMonsterCount--;
        currentKillCount++;

        spawnedMonsters.Remove(monster);

        // [최적화 핵심] 복잡한 반복문과 문자열 비교(Contains) 제거!
        // PoolableObject가 자기의 태생 프리팹을 알고 있으므로 바로 반환 가능합니다.
        ObjectPoolManager.Instance.ReturnToPool(monster);

        if (targetKillCount > 0 && currentKillCount >= targetKillCount)
        {
            isSpawning = false;
            StopSpawningCoroutine();
            currentKillCount = targetKillCount;
            StartCoroutine(ShowClearPageAfterRemovingMonsters());
        }
        else if (isSpawning && currentMonsterCount < maxMonster)
        {
            StartSpawningMonsters();
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
        yield return _waitClearPage;
        StageController.Instance.ShowClearPage();
    }

    private void RemoveAllMonsters()
    {
        // 역순 순회로 리스트에서 안전하게 제거 및 풀 반환
        for (int i = spawnedMonsters.Count - 1; i >= 0; i--)
        {
            GameObject monster = spawnedMonsters[i];
            if (monster != null)
            {
                // 마찬가지로 여기서도 복잡한 조건문 없이 즉시 반환
                ObjectPoolManager.Instance.ReturnToPool(monster);
            }
        }
        spawnedMonsters.Clear();
    }

    private void OnDestroy()
    {
        // 씬 전환 시 싱글톤 매니저가 살아있다면 풀 정리
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ClearAllPools();
        }
    }
}
