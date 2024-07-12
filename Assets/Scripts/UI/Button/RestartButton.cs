using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    float time = 0.0f;
    float F_time = 1.0f;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(PauseExit);
        GetComponent<Button>().onClick.AddListener(GameStart);
    }
    private void GameStart() { StartCoroutine(FadeOut()); }
    private void PauseExit() { Time.timeScale = 1.0f; }
    IEnumerator FadeOut()
    {
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;
        
        while(alpha.a < 1.0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        // PlayerPrefs.DeleteAll(); // 이전에 획득한 정보만 삭제해야 함
        SceneManager.LoadScene(sceneToLoad);
    }
}
