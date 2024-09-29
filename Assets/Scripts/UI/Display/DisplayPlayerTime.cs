using UnityEngine;
using TMPro;

public class DisplayPlayerTime : MonoBehaviour
{
    public float PlayerTime;

    [SerializeField] private TextMeshProUGUI TimeText;

    void Start()
    {
        // GameManager.Instance 또는 GetTime()가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        PlayerTime = GameManager.Instance.GetTime(); // 플레이어 시간 정보 가져오기

        // TimeText가 null이 아닌지 확인
        if (TimeText == null)
        {
            Debug.LogError("TimeText is not assigned.");
            return;
        }

        DisplayTime();
    }

    void Update()
    {
        // PlayerTime이 변경된 경우에만 DisplayTime 호출
        float currentTime = GameManager.Instance.GetTime();
        if (PlayerTime != currentTime)
        {
            PlayerTime = currentTime;
            DisplayTime();
        }
    }

    public void DisplayTime()
    {
        if (TimeText != null)
        {
            int minutes = Mathf.FloorToInt(PlayerTime / 60);
            int seconds = Mathf.FloorToInt(PlayerTime % 60);
            TimeText.text = string.Format("TIME : " + "{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateTime(float newTime)
    {
        PlayerTime = newTime;
        GameManager.Instance.SetTime(PlayerTime);
        DisplayTime();
    }
}
