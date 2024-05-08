using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public float fadeDuration = 1f; // 페이드 인 시간
    private Image[] uiImages; // UI 이미지 배열

    private void Start()
    {
        uiImages = GetComponentsInChildren<Image>(); // 자식 오브젝트의 이미지 컴포넌트를 가져옴
        // 초기 상태에서 UI를 투명하게 설정
        foreach (var image in uiImages) { image.color = new Color(image.color.r, image.color.g, image.color.b, 0f); }
        // 페이드 인 코루틴 시작
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // 페이드 인 시간 동안 알파 값을 증가시켜 페이드 인 효과 생성
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            foreach (var image in uiImages) { image.color = new Color(image.color.r, image.color.g, image.color.b, alpha); }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
