using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText; // 체력바 텍스트 UI
    private Slider PlayerhealthSlider; // 체력바 UI
    public float playerMaxHealth = 100f; // 플레이어 최대 체력
    public float playerCurrentHealth = 100f; // 플레이어 현재 체력
    public GameObject GameOver;

    void Start()
    {
        PlayerhealthSlider = this.GetComponent<Slider>();
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider() // 체력바 업데이트
    {
        float decreaseHp = playerCurrentHealth / playerMaxHealth;
        PlayerhealthSlider.value = decreaseHp; // 체력바의 값을 현재 체력 비율로 설정
        PlayerHPText.text = $"{playerCurrentHealth} / {playerMaxHealth}";
    }

    public void TakeDamage_P(float monsterPower) // 플레이어가 받는 데미지
    {
        playerCurrentHealth -= monsterPower;
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0f, playerMaxHealth); // 체력이 음수가 되지 않도록 클램핑
        UpdateHealthSlider();
        if (playerCurrentHealth <= 0f)
        {
             GameOver.SetActive(true);
             Time.timeScale = 0f;
        }
    }
}
