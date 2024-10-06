using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image FadeInPage;
    private float time = 0f;
    private const float FadeDuration = 2f;

    void Start() 
    {
        Fade(); 
    } 

    public void Fade() 
    { 
        StartCoroutine(Fade_In()); 
    }

    private IEnumerator Fade_In()
    {
        Color alpha = FadeInPage.color;

        while (time < FadeDuration)
        {
            time += Time.deltaTime;
            alpha.a = Mathf.Lerp(1, 0, time / FadeDuration);
            FadeInPage.color = alpha;
            yield return null;
        }

        alpha.a = 0; // Ensure alpha is exactly 0
        FadeInPage.color = alpha; // Update color one last time
        FadeInPage.gameObject.SetActive(false);
    }
}
