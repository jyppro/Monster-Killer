using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float maxTime; // 초기 제한시간
    private float currentTime;
    private bool isTimerRunning;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image fadeOutPage;
    [SerializeField] private GameObject clearPage;

    public MouseLook mouseLook;

    private void Start()
    {
        InitializeTimer();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimer();
        }
    }

    private void InitializeTimer()
    {
        maxTime = (StageLoader.Instance.currentModeIndex == 2)
            ? GameManager.Instance.guardianStages[StageLoader.Instance.currentStageIndex].defenseTime
            : GameManager.Instance.GetTime();

        currentTime = maxTime;
        isTimerRunning = true;
        UpdateTimerText();
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.0f)
        {
            currentTime = 0.0f;
            StopTimer();
        }
        UpdateTimerText();
    }

    private void StopTimer()
    {
        isTimerRunning = false;
        HandleGameOver();
    }

    private void HandleGameOver()
    {
        if (StageLoader.Instance.currentModeIndex == 2)
        {
            StageController.Instance.ShowClearPage();
        }
        else
        {
            StartCoroutine(GameOverAndReturnHome());
        }
    }

    private IEnumerator GameOverAndReturnHome()
    {
        yield return FadeOutAndReturnHome();
        SceneManager.LoadScene("MainScene"); // 메인 씬 이름으로 변경
    }

    private IEnumerator FadeOutAndReturnHome()
    {
        mouseLook.UnlockMouse();
        float fadeDuration = 1.0f;

        if (fadeOutPage != null)
        {
            fadeOutPage.gameObject.SetActive(true);
            Color alpha = fadeOutPage.color;

            for (float time = 0; alpha.a < 1; time += Time.deltaTime / fadeDuration)
            {
                alpha.a = Mathf.Lerp(0, 1, time);
                fadeOutPage.color = alpha;
                yield return null;
            }
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
