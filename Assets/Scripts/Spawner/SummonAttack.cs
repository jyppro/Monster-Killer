using UnityEngine;

public class SummonAttack : MonoBehaviour
{
    public GameObject monsterPrefab;  // 소환할 몬스터의 프리팹
    public Transform player;          // 플레이어의 위치 참조
    // public float offsetY = 0.3f;      // 플레이어의 발 밑 위치를 계산하기 위한 오프셋

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object with tag 'Player' not found.");
        }
    }

    public void SummonMonsterAtPlayerPosition()
{
    if (player != null && monsterPrefab != null)
    {
        // 플레이어의 발 밑 위치 계산
        Vector3 spawnPosition = new Vector3(player.position.x, player.position.y, player.position.z);
        
        // 프리팹의 기본 회전을 가져와서 사용
        Quaternion spawnRotation = monsterPrefab.transform.rotation;

        // 몬스터 소환
        Instantiate(monsterPrefab, spawnPosition, spawnRotation);
        Debug.Log("Monster Spawn Success at: " + spawnPosition);
    }
    else
    {
        Debug.LogWarning("Player or Monster Prefab is not assigned.");
    }
}
}
