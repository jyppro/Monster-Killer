using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BaseHPText; // 체력바 텍스트 UI
    private Slider BasehealthSlider; // 체력바 UI
    public int BaseMaxHealth; // 플레이어 최대 체력
    public int BaseCurrentHealth; // 플레이어 현재 체력
    public GameObject GameOver;

    void Start()
    {
        BasehealthSlider = this.GetComponent<Slider>();
        BaseCurrentHealth = GameManager.Instance.GetCurrentHP(); // GameManager에서 현재 체력값 가져오기
        BaseMaxHealth = GameManager.Instance.GetMaxHP(); // GameManager에서 최대 체력값 가져오기

        if(BaseMaxHealth != BaseCurrentHealth)
        {
            BaseCurrentHealth = BaseMaxHealth;
        }
        
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider() // 체력바 업데이트
    {
        // 정수 나눗셈 대신 부동 소수점 나눗셈으로 변경
        float decreaseHp = (float)BaseCurrentHealth / BaseMaxHealth;
        BasehealthSlider.value = decreaseHp; // 체력바의 값을 현재 체력 비율로 설정
        BaseHPText.text = $"{BaseCurrentHealth} / {BaseMaxHealth}";
    }

    public void TakeDamage_P(int monsterPower) // 플레이어가 받는 데미지
    {
        BaseCurrentHealth -= monsterPower;
        BaseCurrentHealth = Mathf.Clamp(BaseCurrentHealth, 0, BaseMaxHealth); // 체력이 음수가 되지 않도록 클램핑
        UpdateHealthSlider();
        if (BaseCurrentHealth <= 0)
        {
            GameOver.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
