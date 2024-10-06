using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    private bool isPaused = false; // 게임이 일시정지 상태인지 여부
    private GameObject gamePauseUI; // 일시정지 UI 오브젝트
    public Sprite newImage; // 변경할 이미지
    public Sprite originalImage; // 원래 이미지
    private Image buttonImage; // 버튼 이미지 컴포넌트

    private void Awake()
    {
        // Awake에서 UI 오브젝트와 버튼 리스너를 초기화하여 Start()보다 먼저 설정
        gamePauseUI = GameObject.Find("PausePage");
        if (gamePauseUI != null)
        {
            gamePauseUI.SetActive(false);
        }
        
        buttonImage = GetComponent<Image>();
        Button button = GetComponent<Button>();
        if (button != null) 
        { 
            button.onClick.AddListener(TogglePause); 
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused; // 게임 상태 토글
        if (isPaused)
        {
            gamePauseUI.SetActive(true);
            buttonImage.sprite = newImage;
            Time.timeScale = 0.0f; // 게임 일시정지
        }
        else
        {
            gamePauseUI.SetActive(false);
            buttonImage.sprite = originalImage;
            Time.timeScale = 1.0f; // 게임 재개
        }
    }
}
