using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string targetScene; // 이름으로 이동할 씬을 지정
    float time = 0f;
    float F_time = 1f;

    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ChangeScene);
    }

    private void ChangeScene() { StartCoroutine(FadeOut()); }

    IEnumerator FadeOut()
    {
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene(targetScene); // 지정된 이름의 씬으로 이동
    }
}
