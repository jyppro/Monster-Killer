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
    public int targetKillCount = 0;
    public int currentKillCount = 0;

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
                // Add boss specific logic
            }
            else if (stageData is GuardianStageData guardianStageData)
            {
                monsterPrefabs = guardianStageData.guardianPrefabs;
                // Add guardian specific logic
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

    public void MonsterDied()
    {
        currentMonsterCount--;
        currentKillCount++;

        if (currentKillCount >= targetKillCount)
        {
            StageController.Instance.ShowClearPage(); // 인스턴스를 통해 메서드 호출
        }
    }
}
