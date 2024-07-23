using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
    public float maxTime; // 초기 제한시간
    public float currentTime;
    public bool isTimerRunning = false;
    [SerializeField] private TextMeshProUGUI TimeText;
    GameObject GameOver;

    private void Start()
    {
        maxTime = GameManager.Instance.GetTime();
        Debug.Log("maxTime from GameManager: " + maxTime);
        currentTime = maxTime;
        isTimerRunning = true;
        UpdateTimerText();

        GameOver = GameObject.Find("GameOverPage");
        if (GameOver)
        {
            GameOver.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOverPage not found!");
            return;
        }
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

    public void StartTimer() { isTimerRunning = true; }
    public void StopTimer()
    {
        isTimerRunning = false;
        GameOver.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

        Time.timeScale = 0.0f;
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
