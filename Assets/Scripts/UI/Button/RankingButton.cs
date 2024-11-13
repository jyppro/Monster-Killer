using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingButton : MonoBehaviour
{
    [SerializeField] private DisplayRanking displayRanking;

    void Start()
    {
        // GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RankingBtn);
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(RankingBtn);
        }
        else
        {
            Debug.LogError("Button 컴포넌트가 할당되지 않았습니다.");
        }
    }

    void RankingBtn()
    {
        if (displayRanking != null)
        {
            GameManager.Instance.LoadRankingsData();
            Debug.Log("랭킹 데이터 로드 버튼 클릭됨");
        }
        else
        {
            Debug.LogError("GameManager 인스턴스를 찾을 수 없습니다.");
        }

        /* if (displayRanking != null)
        {
            // 버튼 클릭 시 DisplayRanking의 UpdatePlayerInfo 호출하여 데이터 갱신
            displayRanking.RefreshRanking();
            // Debug.Log("랭킹버튼 호출 확인용");
        }
        else
        {
            Debug.LogError("DisplayRanking 스크립트가 할당되지 않았습니다.");
        } */
    }
}
