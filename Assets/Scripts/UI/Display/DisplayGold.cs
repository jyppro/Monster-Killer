using UnityEngine;
using TMPro;

public class DisplayGold : MonoBehaviour
{
    public int PlayerGold;

    [SerializeField] private TextMeshProUGUI GoldText;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        PlayerGold = GameManager.Instance.GetGold();

        if (GoldText == null)
        {
            Debug.LogError("GoldText is not assigned.");
            return;
        }

        DisplayPlayerGold();
    }

    void Update()
    {
        int currentGold = GameManager.Instance.GetGold();
        if (PlayerGold != currentGold)
        {
            PlayerGold = currentGold;
            DisplayPlayerGold();
        }
    }

    public void DisplayPlayerGold()
    {
        if (GoldText != null)
        {
            GoldText.text = "Gold : " + PlayerGold;
        }
    }

    public void UpdateGold(int newGold)
    {
        PlayerGold = newGold;
        GameManager.Instance.SetPower(PlayerGold);
        DisplayPlayerGold();
    }

    public void GoldSum(int goldReward)
    {
        PlayerGold += goldReward;  // 골드 증가
        GameManager.Instance.SetGold(PlayerGold);  // GameManager에도 골드 업데이트
        UpdateGold(PlayerGold);  // 골드 텍스트 업데이트
    }
}
