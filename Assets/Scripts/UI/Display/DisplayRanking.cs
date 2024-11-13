using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayRanking : MonoBehaviour
{
    // 대충 정렬된 랭킹을 받는 변수

    [SerializeField] private TextMeshProUGUI RankingText;

    void Start()
    {
        // RankingText가 null이 아닌지 확인
        if (RankingText == null)
        {
            Debug.LogError("RankingText가 할당되지 않았습니다.");
            return;
        }

        // GameManager의 데이터 로드 완료 이벤트를 구독
        GameManager.Instance.OnRankingsLoaded += DisplayPlayerRanking;

        // 시작 시 한번 데이터 로드
        // RefreshRanking();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnRankingsLoaded -= DisplayPlayerRanking;
        }
    }

/*     public void RefreshRanking()
    {
        // GameManager.Instance 또는 GetPower()???가 null일 경우 예외 처리
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
            return;
        }

        // 대충 정렬 랭킹 데이터 받아오기
        GameManager.Instance.LoadRankingsData();
        // 데이터 로딩이 완료될 때까지 잠시 대기 후 화면에 표시
        // Invoke("DisplayPlayerRanking", 2f); // 데이터 로드 지연을 고려하여 2초 대기

        // 화면에 갱신된 정보 표시
        // DisplayPlayerRanking();
        // Debug.Log("랭킹버튼 호출 확인용2");
    } */
    
    private void DisplayPlayerRanking()
    {
        List<RankData> rankList = GameManager.Instance.GetRankDataList();
        if (rankList != null && rankList.Count > 0)
        {
            string rankText = "";
            for (int i = 0; i < Mathf.Min(10, rankList.Count); i++) // 상위 10명만 표시
            {
                rankText += $"{rankList[i].rank_Rank}. {rankList[i].rank_Name} - {rankList[i].rank_Score} 점\n";
            }
            RankingText.text = rankText;
        }
        else
        {
            RankingText.text = "랭킹 데이터를 불러올 수 없습니다.";
        }
    }

}
