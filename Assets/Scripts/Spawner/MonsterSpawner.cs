using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 소환할 몬스터의 프리팹
    public Transform[] spawnPoints; // 몬스터가 소환될 위치들
    public float spawnInterval = 5f; // 몬스터가 소환되는 주기
    public int numberOfMonsters = 2; // 한 번에 소환할 몬스터의 수
    public int maxMonster = 20; // 최대 소환할 몬스터 수

    private int currentMonsterCount = 0; // 현재 소환된 몬스터 수

    void Start()
    {
        StartCoroutine(SpawnMonsters());
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
                    Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
                    currentMonsterCount++;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    // 몬스터가 죽었을 때 호출되는 메서드
    public void MonsterDied()
    {
        currentMonsterCount--;
    }
}
