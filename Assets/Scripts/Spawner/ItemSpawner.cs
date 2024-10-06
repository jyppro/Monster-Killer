using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private GameObject itemPrefab; // 소환할 아이템의 프리팹
    [SerializeField] private float spawnInterval = 5.0f; // 소환 주기 (초 단위)
    [SerializeField] private Vector2 spawnAreaMin; // 소환 범위 최소 (X, Z)
    [SerializeField] private Vector2 spawnAreaMax; // 소환 범위 최대 (X, Z)
    [SerializeField] private float fixedYPosition = 0.0f; // 아이템 Y축 고정 위치
    [SerializeField] private float spawnAreaRotationAngle = 0f; // 소환 범위 회전 각도
    [SerializeField] private int maxItems = 3; // 최대 소환 아이템 수

    private List<GameObject> spawnedItems = new List<GameObject>(); // 현재 소환된 아이템 리스트
    private Queue<Vector3> pooledPositions = new Queue<Vector3>(); // 재사용할 위치 큐

    private void Start()
    {
        StartCoroutine(SpawnItemRoutine());
    }

    private IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            if (spawnedItems.Count < maxItems)
            {
                Vector3 position = GetSpawnPosition();
                GameObject spawnedItem = Instantiate(itemPrefab, position, Quaternion.identity);
                spawnedItems.Add(spawnedItem);

                // 아이템이 수집될 때 리스트에서 제거
                spawnedItem.GetComponent<Item>().onCollected += () => spawnedItems.Remove(spawnedItem);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // 위치를 계산하고 회전 적용
        Vector3 localPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            fixedYPosition,
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        Quaternion rotation = Quaternion.Euler(0, spawnAreaRotationAngle, 0);
        return rotation * localPosition + transform.position; // 월드 좌표로 변환
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, spawnAreaRotationAngle, 0), Vector3.one);
        
        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2,
            fixedYPosition,
            (spawnAreaMin.y + spawnAreaMax.y) / 2
        );

        Vector3 size = new Vector3(
            spawnAreaMax.x - spawnAreaMin.x,
            0, // 평면이므로 Y축 크기는 0
            spawnAreaMax.y - spawnAreaMin.y
        );

        Gizmos.DrawWireCube(center, size);
    }
}
