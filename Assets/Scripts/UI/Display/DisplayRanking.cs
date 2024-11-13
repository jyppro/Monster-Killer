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
        // RankingText가 null이 아닌지 확인
        if (RankingText == null)
        {
            Debug.LogError("RankingText가 할당되지 않았습니다.");
            return;
        }

        // 시작 시 한번 데이터 로드
        // RefreshRanking();
    }

    public void RefreshRanking()
    {
        // GameManager.Instance 또는 GetPower()???가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
            return;
        }

        // 최신 데이터를 GameManager에서 불러오기
        PlayerRank = GameManager.Instance.GetRank(); // 플레이어 랭킹 정보 가져오기
        PlayerScore = GameManager.Instance.GetSumScore(); // 플레이어 점수 정보 가져오기
        PlayerPlayerID = GameManager.Instance.GetPlayerID(); // 플레이어 플레이어아이디 정보 가져오기

        // 화면에 갱신된 정보 표시
        DisplayPlayerRanking();
        // Debug.Log("랭킹버튼 호출 확인용2");
    }
    
    private void DisplayPlayerRanking()
    {
        if (RankingText != null)
        {
            RankingText.text = $"RANKING: {PlayerRank} NAME: {PlayerPlayerID} SCORE: {PlayerScore}";
        }
    }

}
