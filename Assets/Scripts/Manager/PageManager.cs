using UnityEngine;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages; // 모든 페이지를 관리하기 위한 배열

    // 페이지를 켤 때 다른 페이지는 자동으로 꺼지도록 설정
    public void OpenPage(GameObject pageToOpen)
    {
        foreach (GameObject page in pages)
        {
            page.SetActive(false); // 모든 페이지를 먼저 비활성화
        }

        pageToOpen.SetActive(true); // 선택한 페이지만 활성화
    }
}
