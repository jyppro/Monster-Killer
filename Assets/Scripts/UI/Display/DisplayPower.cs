using UnityEngine;
using TMPro;

public class DisplayPower : MonoBehaviour
{
    public int PlayerPower;

    [SerializeField] private TextMeshProUGUI PowerText;

    void Start()
    {
        // GameManager.Instance 또는 GetPower()가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        PlayerPower = GameManager.Instance.GetPower(); // 플레이어 파워 정보 가져오기

        // PowerText가 null이 아닌지 확인
        if (PowerText == null)
        {
            Debug.LogError("PowerText is not assigned.");
            return;
        }

        DisplayPlayerPower();
    }

    void Update()
    {
        // PlayerPower가 변경된 경우에만 DisplayPlayerPower 호출
        int currentPower = GameManager.Instance.GetPower();
        if (PlayerPower != currentPower)
        {
            PlayerPower = currentPower;
            DisplayPlayerPower();
        }
    }

    public void DisplayPlayerPower()
    {
        if (PowerText != null)
        {
            PowerText.text = "Power : " + PlayerPower;
        }
    }

    public void UpdatePower(int newPower)
    {
        PlayerPower = newPower;
        GameManager.Instance.SetPower(PlayerPower);
        DisplayPlayerPower();
    }
}
