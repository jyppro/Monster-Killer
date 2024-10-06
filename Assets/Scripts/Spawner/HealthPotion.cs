using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] // 인스펙터에서 설정 가능하도록 [SerializeField] 속성을 추가
    private int healAmount = 20; // 회복할 체력 양
    private PlayerHP playerHP;

    private void Awake()
    {
        playerHP = FindObjectOfType<PlayerHP>(); // PlayerHP 컴포넌트 캐시
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealPlayer(); // 플레이어 체력 회복 메서드 호출
        }
    }

    private void HealPlayer()
    {
        if (playerHP != null)
        {
            playerHP.Heal(healAmount); // 체력 회복
            Destroy(gameObject); // 포션 제거
        }
        else
        {
            Debug.LogWarning("PlayerHP component not found!");
        }
    }
}
