using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HomeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad;
    public float F_time = 1.0f;

    private bool isFading = false;
    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current; // 현재 EventSystem을 가져옴
        GetComponent<Button>().onClick.AddListener(ReturnHome);
        // 페이드 아웃 이미지의 초기 알파 값 설정
        if (FadeOutPage != null)
        {
            Color initialColor = FadeOutPage.color;
            initialColor.a = 0;
            FadeOutPage.color = initialColor;
        }
    }

    private void ReturnHome()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOutAndReturnHome());
        }
    }

    private IEnumerator FadeOutAndReturnHome()
    {
        StageLoader.Instance.currentModeIndex = 0;
        StageLoader.Instance.currentStageIndex = 0;

        // EventSystem 비활성화하여 마우스 입력 막기
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
        }

        Time.timeScale = 0.0f;

        // 페이드 아웃 시작
        isFading = true;
        float time = 0.0f;

        if (FadeOutPage != null)
        {
            FadeOutPage.gameObject.SetActive(true);
            Color alpha = FadeOutPage.color;

            while (alpha.a < 1)
            {
                time += Time.unscaledDeltaTime / F_time; // 시간 스케일이 0이므로 unscaledDeltaTime 사용
                alpha.a = Mathf.Lerp(0, 1, time);
                FadeOutPage.color = alpha;
                yield return null;
            }

            // 페이드 아웃 완료 후 씬 전환
            Time.timeScale = 1.0f; // 씬 전환 전에 Time.timeScale을 복구
            SceneManager.LoadScene(sceneToLoad);
        }

        isFading = false;
    }
}
