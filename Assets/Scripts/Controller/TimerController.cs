using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float maxTime; // 초기 제한시간
    public float currentTime;
    public bool isTimerRunning = false;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private Image FadeOutPage;
    [SerializeField] private GameObject ClearPage;

    public MouseLook mouseLook;

    private void Start()
    {
        InitializeTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            if (currentTime <= 0.0f)
            {
                StopTimer();
            }
        }
    }

    public void InitializeTimer()
    {
        if (StageLoader.Instance.currentModeIndex == 2) // Guardian 모드인 경우
        {
            int stageIndex = StageLoader.Instance.currentStageIndex;
            GuardianStageData guardianStageData = GameManager.Instance.guardianStages[stageIndex];
            maxTime = guardianStageData.defenseTime;
        }
        else
        {
            maxTime = GameManager.Instance.GetTime(); // 기본 시간 사용
        }

        currentTime = maxTime;
        isTimerRunning = true;
        UpdateTimerText();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        if (StageLoader.Instance.currentModeIndex == 2) // Guardian 모드인 경우
        {
            StageController.Instance.ShowClearPage();
        }
        else
        {
            StartCoroutine(GameOverAndReturnHome()); // GameManager에 게임오버 및 홈 전환 호출
        }
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


    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
