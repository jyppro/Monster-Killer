using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float maxTime = 60f; // 초기 제한시간
    public float currentTime;
    public bool isTimerRunning = false;
    [SerializeField] private TextMeshProUGUI TimeText;
    GameObject GameOver;

    private void Start()
    {
        GameOver = GameObject.Find("GameOverPage");
        GameOver.SetActive(false);
        currentTime = maxTime;
        isTimerRunning = true;
        UpdateTimerText();
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
            if (currentTime <= 0f) { StopTimer(); }
        }
    }

    public void StartTimer() { isTimerRunning = true; }
    public void StopTimer()
    {
        isTimerRunning = false;
        Time.timeScale = 0f;
        GameOver.SetActive(true);
    }

    public void IncreaseMaxTime(float amount)
    {
        maxTime += amount;
        UpdateTimerText();
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
