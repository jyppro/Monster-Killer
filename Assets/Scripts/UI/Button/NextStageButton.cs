using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    public Image FadeOutPage;
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름
    float F_time = 1.0f;
    private Button nextStageButton;

    private void Start()
    {
        nextStageButton = GetComponent<Button>();

        // 마지막 스테이지인 경우 버튼 비활성화
        if (StageLoader.Instance.currentStageIndex == 9)
        {
            nextStageButton.interactable = false; // 버튼 비활성화
        }
        else
        {
            nextStageButton.onClick.AddListener(PauseExit);
            nextStageButton.onClick.AddListener(GameStart);
        }
    }
    private void GameStart() { StartCoroutine(FadeOut()); }
    private void PauseExit() { Time.timeScale = 1.0f; }

    IEnumerator FadeOut()
    {
        StageLoader.Instance.currentStageIndex++;
        float time = 0.0f;
        
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;
        
        while(alpha.a < 1)
        {
            time += Time.deltaTime / F_time; // Time.deltaTime 사용
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
