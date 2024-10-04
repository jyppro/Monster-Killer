using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerHPText; // 체력바 텍스트 UI
    private Slider PlayerhealthSlider; // 체력바 UI
    public int playerMaxHealth; // 플레이어 최대 체력
    public int playerCurrentHealth; // 플레이어 현재 체력
    // public GameObject GameOver; // 게임오버 페이지 삭제
    [SerializeField] private Image FadeOutPage;
    public MouseLook mouseLook;
    [SerializeField] private Image hitEffectImage; // 피격 효과 이미지 추가
    [SerializeField] private Camera playerCamera; // 플레이어 카메라 추가
    public float hitAlpha = 0.3f;

    private void Start()
    {
        PlayerhealthSlider = this.GetComponent<Slider>();
        playerCurrentHealth = GameManager.Instance.GetCurrentHP(); // GameManager에서 현재 체력값 가져오기
        playerMaxHealth = GameManager.Instance.GetMaxHP(); // GameManager에서 최대 체력값 가져오기

        // BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (StageLoader.Instance.currentModeIndex == 2)
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
        if(monsterPower > 0)
        {
            playerCurrentHealth -= monsterPower;
            playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth); // 체력이 음수가 되지 않도록 클램핑
            UpdateHealthSlider();

            // 피격 이미지 활성화 및 알파값 변경
            if (hitEffectImage != null)
            {
                StartCoroutine(ShowHitEffect());
            }

            // 카메라 흔들림 효과 호출
            if (playerCamera != null)
            {
                StartCoroutine(CameraShake(0.4f, 0.05f)); // 지속 시간과 세기를 전달
            }

            if (playerCurrentHealth <= 0)
            {
                HandleGameOver();
            }
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
        StartCoroutine(GameOverAndReturnHome()); // GameManager에 게임오버 및 홈 전환 호출
    }

    public IEnumerator GameOverAndReturnHome()
    {
        // 페이드 아웃 애니메이션
        yield return StartCoroutine(FadeOutAndReturnHome());

        // 씬 전환
        SceneManager.LoadScene("MainScene"); // 메인 씬 이름으로 변경
    }

    private IEnumerator FadeOutAndReturnHome()
    {
        mouseLook.UnlockMouse();
        float F_time = 1.0f; // 페이드 아웃 시

        if (FadeOutPage != null)
        {
            float time = 0.0f;
            FadeOutPage.gameObject.SetActive(true);
            Color alpha = FadeOutPage.color;

            while (alpha.a < 1)
            {
                time += Time.deltaTime / F_time;
                alpha.a = Mathf.Lerp(0, 1, time);
                FadeOutPage.color = alpha;
                yield return null;
            }
        }
    }

    // 피격 이미지 알파값 조절 코루틴
    private IEnumerator ShowHitEffect()
    {
        float duration = 1.5f; // 1초 동안 효과
        Color hitColor = hitEffectImage.color;
        hitColor.a = hitAlpha;
        hitEffectImage.color = hitColor;
        hitEffectImage.gameObject.SetActive(true);

        // 1초 동안 알파값을 서서히 줄이기
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            hitColor.a = Mathf.Lerp(hitAlpha, 0f, elapsedTime / duration); // 알파값을 0으로 줄임
            hitEffectImage.color = hitColor;
            yield return null;
        }

        hitEffectImage.gameObject.SetActive(false); // 알파값이 0이 되면 비활성화
    }

    // 카메라 흔들림 효과 코루틴
    private IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPosition = playerCamera.transform.localPosition; // 카메라 원래 위치 저장
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude; // 랜덤 X값
            float y = Random.Range(-1f, 1f) * magnitude; // 랜덤 Y값

            playerCamera.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z); // 카메라 위치 변화

            elapsed += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }

        playerCamera.transform.localPosition = originalPosition; // 원래 위치로 복구
    }

    public void Heal(int healAmount)
    {
        playerCurrentHealth += healAmount;
        playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0, playerMaxHealth); // 체력을 최대 체력 이하로 유지
        UpdateHealthSlider(); // 체력바 업데이트
        Debug.Log("체력이 회복되었습니다: " + playerCurrentHealth);
    }
}