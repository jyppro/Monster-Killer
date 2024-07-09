using UnityEngine;
using TMPro;

public class DisplayPlayerHP : MonoBehaviour
{
    public int PlayerHP;

    [SerializeField] private TextMeshProUGUI HPText;

    void Start()
    {
        // GameManager.Instance 또는 GetTime()가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        PlayerHP = GameManager.Instance.GetMaxHP(); // 플레이어 시간 정보 가져오기

        // TimeText가 null이 아닌지 확인
        if (HPText == null)
        {
            Debug.LogError("TimeText is not assigned.");
            return;
        }

        DisplayHP();
    }

    void Update()
    {
        // PlayerTime이 변경된 경우에만 DisplayTime 호출
        int currentHP = GameManager.Instance.GetMaxHP();
        if (PlayerHP != currentHP)
        {
            PlayerHP = currentHP;
            DisplayHP();
        }
    }

    public void DisplayHP()
    {
        if (HPText != null)
        {
            HPText.text = "MaxHP : " + PlayerHP;
        }
    }

    public void UpdateHP(int newHP)
    {
        PlayerHP = newHP;
        GameManager.Instance.SetMaxHP(PlayerHP);
        DisplayHP();
    }
}
