using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class StageButton : MonoBehaviour
{
    public int modeIndex; // 모드 인덱스
    public int stageIndex; // 스테이지 인덱스
    public Image FadeOutPage;
    public Button button;
    public Sprite unlockedSprite;
    public Sprite lockedSprite;
    public TextMeshProUGUI buttonText;
    public float F_time = 1.0f;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnStageSelected);
        UpdateButtonState(); // 스테이지 상태에 따라 버튼 상태 업데이트
    }

    private void OnStageSelected()
    {
        if (GameManager.Instance.IsStageCleared(modeIndex, stageIndex) || stageIndex == 0) // 첫 번째 스테이지는 항상 접근 가능
        {
            StartCoroutine(FadeOutAndLoad());
        }
    }

    private IEnumerator FadeOutAndLoad()
    {
        float time = 0.0f;

        FadeOutPage.gameObject.SetActive(true);
        Color alpha = FadeOutPage.color;

        while (alpha.a < 1)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            FadeOutPage.color = alpha;
            yield return null;
        }

        // 스테이지 인덱스와 모드 인덱스 설정
        StageLoader.Instance.SetCurrentStage(modeIndex, stageIndex);
        string sceneName = StageLoader.Instance.GetCurrentSceneName();
        SceneManager.LoadScene(sceneName); // 해당 씬으로 이동
    }

    private void UpdateButtonState()
    {
        bool isCleared = GameManager.Instance.IsStageCleared(modeIndex, stageIndex);
        if (stageIndex == 0 || isCleared) // 첫 번째 스테이지는 항상 활성화
        {
            button.interactable = true;
            GetComponent<Image>().sprite = unlockedSprite; // 활성화된 버튼 이미지
            if (buttonText != null) buttonText.enabled = true; // 텍스트 보이기
        }
        else
        {
            button.interactable = false;
            GetComponent<Image>().sprite = lockedSprite; // 비활성화된 버튼 이미지
            if (buttonText != null) buttonText.enabled = false; // 텍스트 숨기기
        }
    }
}
