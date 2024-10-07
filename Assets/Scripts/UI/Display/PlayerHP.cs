using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText; // 체력바 텍스트 UI
    private Slider playerHealthSlider; // 체력바 UI
    private int playerMaxHealth; // 플레이어 최대 체력
    private int playerCurrentHealth; // 플레이어 현재 체력

    [SerializeField] private Image FadeOutPage; // 페이드 아웃 이미지
    public MouseLook mouseLook; // 마우스 조작 클래스
    [SerializeField] private Image hitEffectImage; // 피격 효과 이미지
    [SerializeField] private Camera playerCamera; // 플레이어 카메라
    public float hitAlpha = 0.3f; // 피격 효과 알파값

    private void Start()
    {
        playerHealthSlider = GetComponent<Slider>();
        playerCurrentHealth = GameManager.Instance.GetCurrentHP();
        playerMaxHealth = GameManager.Instance.GetMaxHP();

        if (StageLoader.Instance.currentModeIndex == 2)
        {
            IncreaseHealth(10);
        }

        if (playerMaxHealth != playerCurrentHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }

        playerCurrentHealth = Mathf.Min(playerCurrentHealth, playerMaxHealth); // 최대 체력 이하로 클램핑
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider() // 체력바 업데이트
    {
        float decreaseHp = (float)playerCurrentHealth / playerMaxHealth;
        playerHealthSlider.value = decreaseHp; // 체력바 값 설정
        PlayerHPText.text = $"{playerCurrentHealth} / {playerMaxHealth}";
    }

    public void TakeDamage_P(int monsterPower) // 플레이어가 받는 데미지
    {
        if (monsterPower <= 0) return;

        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth - monsterPower, 0, playerMaxHealth);
        UpdateHealthSlider();

        if (hitEffectImage != null) StartCoroutine(ShowHitEffect());
        if (playerCamera != null) StartCoroutine(CameraShake(0.4f, 0.05f));

        if (playerCurrentHealth <= 0) HandleGameOver();
    }

    // 체력 증가 메서드
    public void IncreaseHealth(int multiplier)
    {
        playerMaxHealth *= multiplier;
        playerCurrentHealth *= multiplier;
        UpdateHealthSlider();
    }

    private void HandleGameOver()
    {
        StartCoroutine(GameOverAndReturnHome());
    }

    public IEnumerator GameOverAndReturnHome()
    {
        yield return StartCoroutine(FadeOutAndReturnHome());
        SceneManager.LoadScene("MainScene"); // 메인 씬 전환
    }

    private IEnumerator FadeOutAndReturnHome()
    {
        mouseLook.UnlockMouse();
        if (FadeOutPage != null)
        {
            FadeOutPage.gameObject.SetActive(true);
            Color alpha = FadeOutPage.color;
            float time = 0f;
            const float fadeTime = 1.0f; // 페이드 아웃 시간

            while (alpha.a < 1)
            {
                time += Time.deltaTime / fadeTime;
                alpha.a = Mathf.Lerp(0, 1, time);
                FadeOutPage.color = alpha;
                yield return null;
            }
        }
    }

    // 피격 이미지 알파값 조절
    private IEnumerator ShowHitEffect()
    {
        float duration = 1.5f; // 효과 지속 시간
        Color hitColor = hitEffectImage.color;
        hitColor.a = hitAlpha;
        hitEffectImage.color = hitColor;
        hitEffectImage.gameObject.SetActive(true);

        for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += Time.deltaTime)
        {
            hitColor.a = Mathf.Lerp(hitAlpha, 0f, elapsedTime / duration);
            hitEffectImage.color = hitColor;
            yield return null;
        }

        hitEffectImage.gameObject.SetActive(false); // 비활성화
    }

    // 카메라 흔들림 효과
    private IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPosition = playerCamera.transform.localPosition; // 카메라 원래 위치 저장

        for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            playerCamera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            yield return null; // 한 프레임 대기
        }

        playerCamera.transform.localPosition = originalPosition; // 원래 위치로 복구
    }

    public void Heal(int healAmount)
    {
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth + healAmount, 0, playerMaxHealth);
        UpdateHealthSlider();
    }
}
