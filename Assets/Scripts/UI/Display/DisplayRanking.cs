using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayRanking : MonoBehaviour
{
    // [SerializeField] private TextMeshProUGUI RankingText;
    [SerializeField] private GameObject rankingTextPrefab; // 랭킹 텍스트 프리팹
    [SerializeField] private Transform contentTransform;   // Scroll View의 Content 오브젝트

    void Start()
    {
        // RankingText가 null이 아닌지 확인
/*         if (RankingText == null)
        {
            Debug.LogError("RankingText가 할당되지 않았습니다.");
            return;
        } */

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
        // 이전 랭킹 항목 지우기
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        List<RankData> rankList = GameManager.Instance.GetRankDataList();
        if (rankList != null && rankList.Count > 0)
        {
            // 랭킹 데이터 표시
            for (int i = 0; i < rankList.Count; i++)
            {
                // 프리팹을 인스턴스화하여 Content에 추가
                GameObject rankItem = Instantiate(rankingTextPrefab, contentTransform);
                TextMeshProUGUI rankText = rankItem.GetComponent<TextMeshProUGUI>();
                
                // 텍스트 설정 (랭킹, 이름, 점수 표시)
                rankText.text = $"{rankList[i].rank_Rank}. {rankList[i].rank_Name} - {rankList[i].rank_Score} Score";
            }
        }
        else
        {
            // 데이터가 없을 경우
            GameObject rankItem = Instantiate(rankingTextPrefab, contentTransform);
            TextMeshProUGUI rankText = rankItem.GetComponent<TextMeshProUGUI>();
            rankText.text = "랭킹 데이터를 불러올 수 없습니다.";
        }
    }

}
