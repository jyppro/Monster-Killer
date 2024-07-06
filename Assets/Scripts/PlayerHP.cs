using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText; // 체력바 텍스트 UI
    private Slider PlayerhealthSlider; // 체력바 UI
    public int playerMaxHealth; // 플레이어 최대 체력
    public int playerCurrentHealth; // 플레이어 현재 체력
    public GameObject GameOver;

    void Start()
    {
        PlayerhealthSlider = this.GetComponent<Slider>();
        playerCurrentHealth = GameManager.Instance.GetCurrentHP(); // GameManager에서 현재 체력값 가져오기
        playerMaxHealth = GameManager.Instance.GetMaxHP(); // GameManager에서 최대 체력값 가져오기
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider() // 체력바 업데이트
    {
        // 정수 나눗셈 대신 부동 소수점 나눗셈으로 변경
        float decreaseHp = (float)playerCurrentHealth / playerMaxHealth;
        PlayerhealthSlider.value = decreaseHp; // 체력바의 값을 현재 체력 비율로 설정
        PlayerHPText.text = $"{playerCurrentHealth} / {playerMaxHealth}";
    }

    public void TakeDamage_P(int monsterPower) // 플레이어가 받는 데미지
    {
        playerCurrentHealth -= monsterPower;
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth); // 체력이 음수가 되지 않도록 클램핑
        UpdateHealthSlider();
        if (playerCurrentHealth <= 0)
        {
             GameOver.SetActive(true);
             Time.timeScale = 0.0f;
        }
    }
}
