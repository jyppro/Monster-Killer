using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private Image fadeOutPage; // FadeOutPage를 직렬화하여 Unity 에디터에서 설정할 수 있게 함
    [SerializeField] private string sceneToLoad; // 로드할 씬 이름
    [SerializeField] private float fadeTime = 1.0f; // 페이드 아웃 시간

    private bool isFading = false; // 현재 페이드 아웃 중인지 여부
    private EventSystem eventSystem; // 현재 EventSystem

    private void Start()
    {
        eventSystem = EventSystem.current; // 현재 EventSystem을 가져옴
        Button button = GetComponent<Button>();
        
        if (button != null) 
        {
            button.onClick.AddListener(ReturnHome); // 버튼 클릭 리스너 등록
        }

        // 페이드 아웃 이미지의 초기 알파 값 설정
        if (fadeOutPage != null)
        {
            Color initialColor = fadeOutPage.color;
            initialColor.a = 0;
            fadeOutPage.color = initialColor; // 알파 값을 0으로 초기화
        }
    }

    private void ReturnHome()
    {
        if (!isFading) // 이미 페이드 아웃 중이 아니라면
        {
            StartCoroutine(FadeOutAndReturnHome()); // 페이드 아웃 코루틴 시작
        }
    }

    private IEnumerator FadeOutAndReturnHome()
    {
        // 스테이지 로더의 모드 및 스테이지 인덱스 초기화
        StageLoader.Instance.currentModeIndex = 0;
        StageLoader.Instance.currentStageIndex = 0;

        // EventSystem 비활성화하여 마우스 입력 막기
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
        }

        Time.timeScale = 0.0f; // 게임 일시 정지

        // 페이드 아웃 시작
        isFading = true;
        float elapsedTime = 0.0f;

        if (fadeOutPage != null)
        {
            fadeOutPage.gameObject.SetActive(true); // 페이드 아웃 페이지 활성화
            Color alpha = fadeOutPage.color;

            while (alpha.a < 1)
            {
                elapsedTime += Time.unscaledDeltaTime; // 스케일링 없이 시간 경과
                alpha.a = Mathf.Lerp(0, 1, elapsedTime / fadeTime); // 알파값 변화
                fadeOutPage.color = alpha; // 알파값 적용
                yield return null; // 다음 프레임까지 대기
            }

            // 페이드 아웃 완료 후 씬 전환
            Time.timeScale = 1.0f; // 씬 전환 전에 Time.timeScale을 복구
            SceneManager.LoadScene(sceneToLoad); // 씬 전환
        }

        isFading = false; // 페이드 아웃 종료
    }
}
