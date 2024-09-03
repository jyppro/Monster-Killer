using UnityEngine;
using System.Collections;

public class SummonMonster : MonoBehaviour
{
    public int damage = 10;  // 플레이어에게 줄 데미지
    public float damageInterval = 0.5f;  // 도트 데미지 간격
    public float moveSpeed = 2f;  // 몬스터 이동 속도

    private Coroutine damageCoroutine;  // 도트 데미지를 적용하는 코루틴을 저장할 변수
    private PlayerHP playerHP;  // 플레이어의 HP 스크립트를 저장할 변수
    private Transform playerTransform;  // 플레이어의 Transform을 저장할 변수

    void Start()
    {
        // 플레이어의 Transform을 찾아서 저장
        // playerTransform = FindObjectOfType<PlayerHP>().transform;
        playerTransform = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 몬스터가 플레이어를 천천히 따라가도록 이동
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        playerHP = FindObjectOfType<PlayerHP>();
        if (other.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 도트 데미지를 시작
            if (playerHP != null)
            {
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(ApplyDotDamage());
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 콜리전 범위를 벗어나면 도트 데미지 코루틴을 중지
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
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
