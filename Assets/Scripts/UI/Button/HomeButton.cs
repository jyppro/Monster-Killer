using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    float F_time = 1.0f;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PauseExit);
        GetComponent<Button>().onClick.AddListener(ReturnHome);
    }
    private void ReturnHome()
    {
        GameManager.Instance.SaveGameData();
        StartCoroutine(FadeOut());
    }
    private void PauseExit() { Time.timeScale = 1.0f; }
    IEnumerator FadeOut()
    {
        float time = 0.0f;
        
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;
        
        while(alpha.a < 1)
        {
            time += Time.unscaledDeltaTime / F_time; // Time.unscaledDeltaTime 사용
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
