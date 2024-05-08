using UnityEngine;
using TMPro;

public class GoldController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    public float currentGold = 0f;

    private void Start()
    {
        goldText = GetComponent<TextMeshProUGUI>();
        // 초기화할 때 골드 텍스트 업데이트
        UpdateGoldText();
    }

    // 골드 텍스트 업데이트 메서드
    public void UpdateGoldText() { goldText.text = "Gold: " + currentGold + "G"; }

    // 골드 추가 메서드
    public void GoldSum(float goldReward)
    {
        currentGold += goldReward;
        UpdateGoldText();
    }
}
