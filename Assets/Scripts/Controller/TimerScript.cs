using UnityEngine;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float maxTime; // 초기 제한시간
    public float currentTime;
    public bool isTimerRunning = false;
    [SerializeField] private TextMeshProUGUI TimeText;
    GameObject GameOver;
    public GoldController Gold;

    private void Start()
    {
        if(GameOver)
        {
            GameOver = GameObject.Find("GameOverPage");
            GameOver.SetActive(false);
        }
        else
        {
            return;
        }
        
        this.Gold = GameObject.Find("GoldController").GetComponent<GoldController>();

        maxTime = GameManager.Instance.GetTime();
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
            if (currentTime <= 0.0f)
            {
                StopTimer();
                GameManager.Instance.SetGold(GameManager.Instance.GetGold() + this.Gold.currentGold); // 게임 인스턴스의 골드 값에 현재 골드 값을 더한다.
            }
        }
    }

    public void StartTimer() { isTimerRunning = true; }
    public void StopTimer()
    {
        isTimerRunning = false;
        Time.timeScale = 0.0f;
        GameOver.SetActive(true);
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
