using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // EventSystem을 사용하기 위해 추가

public class HomeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    public float F_time = 1.0f;

    private bool isFading = false;
    private EventSystem eventSystem; // EventSystem을 참조

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

    private void Update()
    {
        // ESC 키를 눌러도 아무 동작이 없도록 로그만 남김 (차단 효과)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC 입력이 차단되었습니다.");
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
        // EventSystem 비활성화하여 마우스 입력 막기
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
        }

        // Time.timeScale을 0으로 설정하여 게임을 일시정지
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
            Debug.Log("씬 전환을 시작합니다.");
            Time.timeScale = 1.0f; // 씬 전환 전에 Time.timeScale을 복구
            SceneManager.LoadScene(sceneToLoad);
        }

        isFading = false;
    }
}
