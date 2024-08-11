using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public float spawnInterval = 3.0f;
    public int numberOfMonsters = 2;

    private GameObject[] monsterPrefabs;
    private int maxMonster = 20;
    private int currentMonsterCount = 0;
    public int targetKillCount = 0; // 목표 킬 카운트
    public int currentKillCount = 0; // 현재 킬 카운트
    private bool bossSpawned = false; // 보스가 소환되었는지 여부를 추적

    void Start()
    {
        BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (stageData != null)
        {
            if (stageData is HuntStageData huntStageData)
            {
                monsterPrefabs = huntStageData.monsterPrefabs;
                targetKillCount = huntStageData.targetKillCount;
                StartCoroutine(SpawnMonsters());
            }
            else if (stageData is BossStageData bossStageData)
            {
                monsterPrefabs = new GameObject[] { bossStageData.bossPrefab };
                targetKillCount = bossStageData.targetKillCount;
                SpawnBoss(); // 보스 스테이지일 경우 보스를 소환
            }
            else if (stageData is GuardianStageData guardianStageData)
            {
                monsterPrefabs = guardianStageData.guardianPrefabs;
                // defenseTime = guardianStageData.defenseTime; // 가디언 모드의 제한 시간은 주석 처리
                StartCoroutine(SpawnMonsters());
            }
        }
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonster)
            {
                for (int i = 0; i < numberOfMonsters && currentMonsterCount < maxMonster; i++)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
                    Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
                    currentMonsterCount++;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBoss()
    {
        if (!bossSpawned && monsterPrefabs.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(monsterPrefabs[0], spawnPoint.position, spawnPoint.rotation);
            bossSpawned = true; // 보스가 소환되었음을 기록
            currentMonsterCount++; // 현재 몬스터 수를 증가
        }
    }

    public void MonsterDied()
    {
        currentMonsterCount--;
        currentKillCount++;

        // targetKillCount가 0일 때에는 클리어 페이지를 표시하지 않음
        if (targetKillCount > 0 && currentKillCount >= targetKillCount)
        {
            StageController.Instance.ShowClearPage(); // 인스턴스를 통해 메서드 호출
        }
    }
}
