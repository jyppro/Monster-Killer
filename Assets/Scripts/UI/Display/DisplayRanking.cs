using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayRanking : MonoBehaviour
{
    // [SerializeField] private TextMeshProUGUI RankingText;
    [SerializeField] private GameObject rankingTextPrefab; // 랭킹 텍스트 프리팹
    [SerializeField] private Transform contentTransform;   // Scroll View의 Content 오브젝트

    // 오브젝트 풀링을 위한 리스트
    private List<GameObject> objectPool = new List<GameObject>();
    
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
        List<RankData> rankList = GameManager.Instance.GetRankDataList();

        // 이전 랭킹 항목 비활성화 및 풀에 반환
        foreach (Transform child in contentTransform)
        {
            child.gameObject.SetActive(false);
            objectPool.Add(child.gameObject);
        }
        // 강제로 캔버스 업데이트 (레이아웃 갱신)
        Canvas.ForceUpdateCanvases();

        // 새로운 데이터로 UI 갱신
        if (rankList != null && rankList.Count > 0)
        {
            for (int i = 0; i < rankList.Count; i++)
            {
                // 풀에서 오브젝트를 가져오거나, 없으면 새로 생성
                GameObject rankItem = GetPooledObject();
                rankItem.transform.SetParent(contentTransform, false);
                rankItem.SetActive(true);

                // 텍스트 설정
                TextMeshProUGUI rankText = rankItem.GetComponent<TextMeshProUGUI>();
                rankText.text = $"{rankList[i].rank_Rank}. {rankList[i].rank_Name} - {rankList[i].rank_Score} Score";
            }
        }
        else
        {
            GameObject rankItem = GetPooledObject();
            rankItem.transform.SetParent(contentTransform, false);
            rankItem.SetActive(true);

            TextMeshProUGUI rankText = rankItem.GetComponent<TextMeshProUGUI>();
            rankText.text = "랭킹 데이터를 불러올 수 없습니다.";
        }

        // 추가적으로 레이아웃을 강제로 갱신
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentTransform.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
    }

    // 오브젝트 풀에서 사용 가능한 오브젝트를 가져오는 메서드
    private GameObject GetPooledObject()
    {
        // 풀에 사용 가능한 오브젝트가 있는지 확인
        foreach (var obj in objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                objectPool.Remove(obj);
                return obj;
            }
        }

        // 사용 가능한 오브젝트가 없으면 새로 생성
        GameObject newObj = Instantiate(rankingTextPrefab);
        return newObj;
    }
}
