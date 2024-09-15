using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject GamePauseUI;
     public Sprite newImage; // 변경할 이미지
     public Sprite orignalImage; // 기존 이미지
    private Image buttonImage;

    private void Start()
    {
        GamePauseUI = GameObject.Find("PausePage");
        GamePauseUI.SetActive(false);
        buttonImage = GetComponent<Image>();
        if(GetComponent<Button>() != null) { GetComponent<Button>().onClick.AddListener(TogglePause); }
    }
    // private void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { TogglePause(); } }
    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            GamePauseUI.SetActive(true);
            buttonImage.sprite = newImage;
            Time.timeScale = 0.0f;
        }
        else
        {
            GamePauseUI.SetActive(false);
            buttonImage.sprite = orignalImage;
            Time.timeScale = 1.0f;
        }
    }
}
