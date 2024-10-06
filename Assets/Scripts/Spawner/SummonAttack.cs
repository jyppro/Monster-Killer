using UnityEngine;

public class SummonAttack : MonoBehaviour
{
    [Header("Summon Settings")]
    [SerializeField] private GameObject monsterPrefab; // 소환할 몬스터의 프리팹
    private Transform player; // 플레이어의 위치 참조

    private void Start()
    {
        // 플레이어 오브젝트 찾기
        player = GameObject.Find("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("Player object not found.");
        }
    }

    public void SummonMonsterAtPlayerPosition()
    {
        if (player == null || monsterPrefab == null)
        {
            Debug.LogWarning("Player or Monster Prefab is not assigned.");
            return;
        }

        // 플레이어의 위치를 기반으로 몬스터 소환
        Vector3 spawnPosition = player.position;
        Quaternion spawnRotation = monsterPrefab.transform.rotation;

        // 몬스터 소환
        Instantiate(monsterPrefab, spawnPosition, spawnRotation);
        Debug.Log($"Monster Spawned at: {spawnPosition}");
    }
}
