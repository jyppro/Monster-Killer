using UnityEngine;
using System.Collections;

public class SummonMonster : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damage = 10;  // 플레이어에게 줄 데미지
    [SerializeField] private float damageInterval = 0.5f;  // 도트 데미지 간격
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;  // 몬스터 이동 속도

    private Coroutine damageCoroutine;  // 도트 데미지를 적용하는 코루틴
    private PlayerHP playerHP;  // 플레이어의 HP 스크립트
    private Transform playerTransform;  // 플레이어의 Transform

    private void Start()
    {
        // 플레이어의 Transform을 찾아서 저장
        GameObject playerObject = GameObject.Find("Player");
        playerTransform = playerObject != null ? playerObject.transform : null;

        if (playerTransform == null)
        {
            Debug.LogWarning("Player object not found.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // 몬스터가 플레이어를 천천히 따라가도록 이동
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 도트 데미지를 시작
            if (playerHP == null)
            {
                playerHP = FindObjectOfType<PlayerHP>();
            }

            if (playerHP != null && damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(ApplyDotDamage());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 콜리전 범위를 벗어나면 도트 데미지 코루틴을 중지
            StopDamageCoroutine();
        }
    }

    private void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    private IEnumerator ApplyDotDamage()
    {
        while (true)  // 무한 루프를 사용하여 도트 데미지를 주기적으로 적용
        {
            if (playerHP != null)
            {
                playerHP.TakeDamage_P(damage);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
