using UnityEngine;
using UnityEngine.UI;

public class GuidePageController : MonoBehaviour
{
    public GameObject[] guidePages; // 각 가이드 페이지를 배열로 선언합니다.
    private int currentPageIndex = 0; // 현재 페이지 인덱스를 초기화합니다.

    // 버튼을 위한 변수 추가
    public Button leftButton;
    public Button rightButton;

    private void Start()
    {
        ShowPage(currentPageIndex); // 시작 시 첫 번째 페이지를 표시합니다.

        // 버튼 클릭 이벤트에 메서드 연결
        leftButton.onClick.AddListener(OnLeftButtonClick);
        rightButton.onClick.AddListener(OnRightButtonClick);
    }

    // 현재 페이지를 표시하는 함수
    private void ShowPage(int index)
    {
        for (int i = 0; i < guidePages.Length; i++)
        {
            guidePages[i].SetActive(i == index); // 인덱스가 일치하는 페이지만 활성화합니다.
        }
    }

    // 왼쪽 버튼 클릭 시 호출되는 함수
    public void OnLeftButtonClick()
    {
        currentPageIndex--; // 인덱스를 감소시킵니다.
        if (currentPageIndex < 0) // 인덱스가 0보다 작아지면 6으로 루프합니다.
        {
            currentPageIndex = guidePages.Length - 1; // 마지막 페이지로 설정합니다.
        }
        ShowPage(currentPageIndex); // 해당 페이지를 표시합니다.
    }

    // 오른쪽 버튼 클릭 시 호출되는 함수
    public void OnRightButtonClick()
    {
        currentPageIndex++; // 인덱스를 증가시킵니다.
        if (currentPageIndex >= guidePages.Length) // 인덱스가 최대 페이지 수를 초과하면 0으로 루프합니다.
        {
            currentPageIndex = 0; // 첫 번째 페이지로 설정합니다.
        }
        ShowPage(currentPageIndex); // 해당 페이지를 표시합니다.
    }
}
