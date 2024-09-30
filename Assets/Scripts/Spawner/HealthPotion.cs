using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    // 회복할 체력 양
    public int healAmount = 20;
    private PlayerHP playerHP;

    void Start()
    {
        playerHP = FindObjectOfType<PlayerHP>();
    }

    // 플레이어와 충돌했을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 플레이어의 PlayerHP 컴포넌트 찾기
        if (playerHP != null)
        {
            // 체력 회복
            playerHP.Heal(healAmount);

            // 포션 제거
            Destroy(gameObject);
        }
    }
}
