using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneChangeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    public float F_time = 1.0f;
    private EventSystem eventSystem; // EventSystem을 참조

    private void Start()
    {
        eventSystem = EventSystem.current; // 현재 EventSystem을 가져옴
        GetComponent<Button>().onClick.AddListener(SceneChange);
    }

    private void SceneChange()
    {
        PauseExit();  // Time.timeScale을 복구
        StartCoroutine(FadeOut());
    }

    private void PauseExit() 
    { 
        Time.timeScale = 1.0f; 
    }

    IEnumerator FadeOut()
    {
        float time = 0.0f;

        // EventSystem 비활성화하여 입력 막기
        if (eventSystem != null)
        {
            eventSystem.enabled = false; 
        }

        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;

        while (alpha.a < 1)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene(sceneToLoad);
    }
}
