using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    public float F_time = 1.0f;

    private bool isFading = false;

    private void Start()
    {
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
    // Time.timeScale 복구
    Time.timeScale = 1.0f;

    // 페이드 아웃 시작
    isFading = true;
    float time = 0.0f;

    if (FadeOutPage != null)
    {
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;

        while (alpha.a < 1)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }

        // 페이드 아웃 완료 후 씬 전환
        Debug.Log("씬 전환을 시작합니다."); // 디버그 메시지 추가
        SceneManager.LoadScene(sceneToLoad);
    }

    isFading = false;
}

}
