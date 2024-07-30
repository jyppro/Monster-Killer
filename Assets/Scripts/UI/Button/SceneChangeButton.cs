using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string targetScene; // 이름으로 이동할 씬을 지정
    float F_time = 1f;
    bool useUnscaledTime = false;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangeScene);
        // 초기 색상 및 알파 값 설정
        if (FadeOutPage != null)
        {
            FadeOutPage.color = new Color(FadeOutPage.color.r, FadeOutPage.color.g, FadeOutPage.color.b, 0);
            FadeOutPage.gameObject.SetActive(false);
        }
    }

    public void ChangeScene()
    {
        // 게임의 현재 상태에 따라 useUnscaledTime 설정
        if (Time.timeScale == 0)
        {
            useUnscaledTime = true;
        }
        else
        {
            useUnscaledTime = false;
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float time = 0f;

        if (FadeOutPage != null)
        {
            // 코루틴 시작 시 알파 값을 0으로 초기화
            FadeOutPage.color = new Color(FadeOutPage.color.r, FadeOutPage.color.g, FadeOutPage.color.b, 0);
            FadeOutPage.gameObject.SetActive(true);
            Color alpha = FadeOutPage.color;

            // 디버그 메시지로 초기 알파 값 확인
            // Debug.Log("Initial Alpha: " + alpha.a);

            while (alpha.a < 1)
            {
                if (useUnscaledTime)
                {
                    time += Time.unscaledDeltaTime / F_time; // unscaledDeltaTime 사용
                }
                else
                {
                    time += Time.deltaTime / F_time; // deltaTime 사용
                }

                alpha.a = Mathf.Lerp(0, 1, time);
                FadeOutPage.color = new Color(FadeOutPage.color.r, FadeOutPage.color.g, FadeOutPage.color.b, alpha.a);

                // 디버그 메시지로 현재 알파 값 확인
                // Debug.Log("Current Alpha: " + alpha.a);

                yield return null;
            }

            // Debug.Log("Scene is changing to: " + targetScene); // 디버그 메시지 추가
            SceneManager.LoadScene(targetScene); // 지정된 이름의 씬으로 이동
        }
        // else
        // {
        //     Debug.LogError("FadeOutPage is not assigned.");
        // }
    }
}
