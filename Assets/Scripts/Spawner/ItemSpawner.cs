using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // 소환할 아이템의 프리팹
    public GameObject itemPrefab;

    // 소환 주기 (초 단위)
    public float spawnInterval = 5.0f;

    // 소환 범위 (X, Z 좌표의 최소 및 최대값)
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;

    // Y축 고정 값 (아이템이 소환될 평면의 높이)
    public float fixedYPosition = 0.0f;

    // 소환 범위의 회전 각도 (Y축 기준 회전)
    public float spawnAreaRotationAngle = 0f;

    // 소환 가능한 최대 아이템 수
    public int maxItems = 3;

    // 현재 필드에 존재하는 아이템 수를 추적할 리스트
    private List<GameObject> spawnedItems = new List<GameObject>();

    // 시작할 때 코루틴 실행
    private void Start()
    {
        StartCoroutine(SpawnItemRoutine());
    }

    // 일정 시간마다 아이템을 소환하는 코루틴
    private IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            // 현재 소환된 아이템이 maxItems보다 적은지 확인
            if (spawnedItems.Count < maxItems)
            {
                // 랜덤 위치 생성 (X, Y 고정, Z)
                Vector3 localPosition = new Vector3(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    fixedYPosition, // Y는 고정
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y) // Z는 랜덤
                );

                // 회전 각도를 적용하여 월드 좌표로 변환
                Quaternion rotation = Quaternion.Euler(0, spawnAreaRotationAngle, 0);
                Vector3 rotatedPosition = rotation * localPosition;

                // 월드 좌표에서 아이템 소환
                GameObject spawnedItem = Instantiate(itemPrefab, rotatedPosition + transform.position, Quaternion.identity);

                // 소환된 아이템을 리스트에 추가
                spawnedItems.Add(spawnedItem);

                // 아이템이 사라졌을 때 리스트에서 제거하는 콜백 설정
                spawnedItem.GetComponent<Item>().onCollected += () => spawnedItems.Remove(spawnedItem);
            }

            // 소환 주기 동안 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 디버그용으로 소환 범위를 시각화하는 함수
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Quaternion rotation = Quaternion.Euler(0, spawnAreaRotationAngle, 0);

        Vector3 center = new Vector3(
            (spawnAreaMin.x + spawnAreaMax.x) / 2,
            fixedYPosition,
            (spawnAreaMin.y + spawnAreaMax.y) / 2
        );

        // 회전 적용
        Vector3 rotatedCenter = rotation * center;
        Vector3 size = new Vector3(
            spawnAreaMax.x - spawnAreaMin.x,
            0, // 평면이므로 Y축 크기는 0
            spawnAreaMax.y - spawnAreaMin.y
        );

        Gizmos.matrix = Matrix4x4.TRS(transform.position, rotation, Vector3.one);
        Gizmos.DrawWireCube(center, size);
    }
}
