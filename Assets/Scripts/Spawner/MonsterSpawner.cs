using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // 몬스터가 소환될 위치들
    public float spawnInterval = 5f; // 몬스터가 소환되는 주기
    public int numberOfMonsters = 2; // 한 번에 소환할 몬스터의 수

    private GameObject[] monsterPrefabs; // 소환할 몬스터의 프리팹들
    private int maxMonster = 20; // 최대 소환할 몬스터 수
    private int currentMonsterCount = 0; // 현재 소환된 몬스터 수
    public int targetKillCount = 0; // 목표 처치 마리 수 -> 스테이지 데이터에서 가져와 세팅 (ex : 20)
    public int currentKillCount = 0; // 현재 처치 마리 수
    [SerializeField] private GameObject ClearPage;

    void Start()
    {
        if (ClearPage == null)
        {
            Debug.LogError("ClearPage is not assigned.");
            return;
        }
        ClearPage.SetActive(false);

        // 현재 스테이지의 데이터를 받아 설정
        StageData stageData = GameManager.Instance.GetStageData(StageLoader.Instance.currentStageIndex);
        if (stageData != null)
        {
            monsterPrefabs = stageData.monsterPrefabs; // 몬스터 세팅
            targetKillCount = stageData.targetKillCount; // 목표 처치 마리 수 세팅
        }

        StartCoroutine(SpawnMonsters());
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonster) // 소환된 몬스터 수가 최대 마리수보다 적으면 계속 소환
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

    // 몬스터가 죽었을 때 호출
    public void MonsterDied()
    {
        currentMonsterCount--;
        currentKillCount++;

        if(currentKillCount >= maxMonster)
        {
            currentKillCount = maxMonster;
            ShowClearPage();
        }
    }

    private void ShowClearPage()
    {
        if (ClearPage != null)
        {
            ClearPage.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
