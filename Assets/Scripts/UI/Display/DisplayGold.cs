using UnityEngine;
using TMPro;

public class DisplayGold : MonoBehaviour
{
    public int PlayerGold;

    [SerializeField] private TextMeshProUGUI goldText;

    void Start()
    {
       PlayerGold = GameManager.Instance.GetGold();
    }

    void Update()
    {
        DisplayPlayerGold();
    }

    public void DisplayPlayerGold()
    {
        if (goldText != null)
        {
            goldText.text = "Gold : " + PlayerGold + "G";  // 골드 텍스트 업데이트
        }
    }
}
