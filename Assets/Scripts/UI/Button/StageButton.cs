using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageButton : MonoBehaviour
{
    public int stageIndex; // 스테이지 인덱스
    public Image FadeOutPage;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnStageSelected);
    }

    private void OnStageSelected()
    {
        StartCoroutine(FadeOutAndLoad(SceneManager.GetActiveScene().name)); // 페이드 아웃 시작
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;

        float time = 0f;
        float fadeTime = 1f;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / fadeTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }

        // 스테이지 인덱스 설정
        StageLoader.Instance.LoadStageData(stageIndex); // StageLoader에 스테이지 데이터 로드
        SceneManager.LoadScene("HuntStageScene"); // 헌트 스테이지 씬으로 이동
    }
}
