using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Image FadeOutPage;
    float time = 0f;
    float F_time = 1f;

    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PauseExit);
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GameStart);
    }
    private void GameStart() { StartCoroutine(FadeOut()); }
    private void PauseExit() { Time.timeScale = 1f; }
    IEnumerator FadeOut()
    {
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;
        
        while(alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("KillerModeScene");
    }
}
