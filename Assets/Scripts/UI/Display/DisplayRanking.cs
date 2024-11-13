using UnityEngine;
using TMPro;

public class DisplayRanking : MonoBehaviour
{
    public int PlayerRank;
    public int PlayerScore;
    public string PlayerPlayerID;

    [SerializeField] private TextMeshProUGUI RankingText;

    void Start()
    {
        // GameManager.Instance 또는 GetPower()가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        PlayerRank = GameManager.Instance.GetRank(); // 플레이어 랭킹 정보 가져오기
        PlayerScore = GameManager.Instance.GetSumScore(); // 플레이어 점수 정보 가져오기
        PlayerPlayerID = GameManager.Instance.GetPlayerID(); // 플레이어 플레이어아이디 정보 가져오기

        // PowerText가 null이 아닌지 확인
        if (RankingText == null)
        {
            Debug.LogError("RankingText is not assigned.");
            return;
        }

        DisplayPlayerRanking();
    }

    void Update()
    {
        // PlayerPower가 변경된 경우에만 DisplayPlayerPower 호출
        int currentRank = GameManager.Instance.GetRank();
        int currentScore = GameManager.Instance.GetSumScore();
        // string currentPlayerID = GameManager.Instance.GetPlayerID();

        if (PlayerRank != currentRank && PlayerScore != currentScore /* && PlayerPlayerID != currentPlayerID */)
        {
            PlayerRank = currentRank;
            PlayerScore = currentScore;
            // PlayerPlayerID = currentPlayerID;
            DisplayPlayerRanking();
        }
    }

    public void DisplayPlayerRanking()
    {
        if (RankingText != null)
        {
            RankingText.text = "RANKING: " + PlayerRank + " NAME: " + PlayerPlayerID + " SCORE: " + PlayerScore;
        }
    }
    
}
