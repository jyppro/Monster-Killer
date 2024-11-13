using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingButton : MonoBehaviour
{
    [SerializeField] private DisplayRanking displayRanking;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RankingBtn);
    }

    void RankingBtn()
    {
        if (displayRanking != null)
        {
            // 버튼 클릭 시 DisplayRanking의 UpdatePlayerInfo 호출하여 데이터 갱신
            displayRanking.RefreshRanking();
            // Debug.Log("랭킹버튼 호출 확인용");
        }
        else
        {
            Debug.LogError("DisplayRanking 스크립트가 할당되지 않았습니다.");
        }
    }
}
