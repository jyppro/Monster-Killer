using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints; // 소환 포인트
    [SerializeField] private float spawnInterval = 3.0f; // 소환 주기
    [SerializeField] private int numberOfMonsters = 2; // 한 번에 소환할 몬스터 수

    private GameObject[] monsterPrefabs; // 몬스터 프리팹 배열
    private const int maxMonster = 20; // 최대 몬스터 수
    private int currentMonsterCount = 0; // 현재 몬스터 수
    public int targetKillCount = 0; // 목표 킬 카운트
    public int currentKillCount = 0; // 현재 킬 카운트
    private bool bossSpawned = false; // 보스 소환 여부

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
            StartCoroutine(SpawnMonsters());
        }
        else if (stageData is BossStageData bossStageData)
        {
            InitializeMonsterData(new GameObject[] { bossStageData.bossPrefab }, bossStageData.targetKillCount);
            SpawnBoss(); // 보스 소환
        }
        else if (stageData is GuardianStageData guardianStageData)
        {
            InitializeMonsterData(guardianStageData.guardianPrefabs, 0);
            StartCoroutine(SpawnMonsters());
        }
    }

    private void InitializeMonsterData(GameObject[] prefabs, int killCount)
    {
        monsterPrefabs = prefabs;
        targetKillCount = killCount;
    }

    private IEnumerator SpawnMonsters()
    {
        while (true)
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
        GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
        Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        currentMonsterCount++;
    }

    public void SpawnBoss()
    {
        if (!bossSpawned && monsterPrefabs.Length > 0)
        {
            SpawnMonster();
            bossSpawned = true; // 보스 소환 기록
        }
    }

    public void MonsterDied()
    {
        currentMonsterCount--;
        currentKillCount++;

        // 목표 킬 카운트 달성 시 클리어 페이지 표시
        if (targetKillCount > 0 && currentKillCount >= targetKillCount)
        {
            StageController.Instance.ShowClearPage();
        }
    }
}
