using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText; // 체력바 텍스트 UI
    private Slider PlayerhealthSlider; // 체력바 UI
    public int playerMaxHealth; // 플레이어 최대 체력
    public int playerCurrentHealth; // 플레이어 현재 체력
    public GameObject GameOver; // 게임오버 페이지 삭제

    private void Start()
    {
        PlayerhealthSlider = this.GetComponent<Slider>();
        playerCurrentHealth = GameManager.Instance.GetCurrentHP(); // GameManager에서 현재 체력값 가져오기
        playerMaxHealth = GameManager.Instance.GetMaxHP(); // GameManager에서 최대 체력값 가져오기

        BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (stageData is GuardianStageData guardianStageData)
        {
            IncreaseHealth(10);
        }
        if (playerMaxHealth != playerCurrentHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }

        UpdateHealthSlider();
    }

    public void UpdateHealthSlider() // 체력바 업데이트
    {
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
            HandleGameOver();
        }
    }

    // 체력 증가 메서드 추가
    public void IncreaseHealth(int multiplier)
    {
        playerMaxHealth *= multiplier;
        playerCurrentHealth *= multiplier;
        UpdateHealthSlider();
    }

    private void HandleGameOver()
    {
        StartCoroutine(GameManager.Instance.GameOverAndReturnHome()); // GameManager에 게임오버 및 홈 전환 호출
    }
}
