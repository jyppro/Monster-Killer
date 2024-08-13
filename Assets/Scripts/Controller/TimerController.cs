using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float maxTime; // 초기 제한시간
    public float currentTime;
    public bool isTimerRunning = false;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private GameObject ClearPage;

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
            StartCoroutine(GameManager.Instance.GameOverAndReturnHome()); // GameManager에 게임오버 및 홈 전환 호출
        }
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
