using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public Image FadeOutPage;
    float time = 0.0f;
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
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;
        
        while(alpha.a < 1.0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0.0f, 1.0f, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene("MainScene");
    }
}
