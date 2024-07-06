using UnityEngine;
using TMPro;
using System.Linq;

public class GoldController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] goldTexts;  // 모든 골드 텍스트를 담을 배열
    public int currentGold = 0;

    private void Start()
    {
        // 씬에서 골드 텍스트를 찾아 배열에 저장
        goldTexts = FindObjectsOfType<TextMeshProUGUI>()
                        .Where(text => text.gameObject.name == "GoldText")
                        .ToArray();

        UpdateGoldText();  // 골드 텍스트 업데이트
    }

    public void UpdateGoldText()
    {
        // 골드 텍스트 배열이 비어있지 않으면 업데이트 실행
        if (goldTexts != null && goldTexts.Length > 0)
        {
            foreach (var goldText in goldTexts)
            {
                if (goldText != null)
                {
                    goldText.text = "Gold : " + currentGold + "G";  // 골드 텍스트 업데이트
                }
            }
        }
        else
        {
            Debug.LogError("골드 텍스트가 할당되지 않았습니다.");
        }
    }

    public void GoldSum(int goldReward)
    {
        currentGold += goldReward;  // 현재 골드 증가
        UpdateGoldText();  // 골드 텍스트 업데이트
    }
}
