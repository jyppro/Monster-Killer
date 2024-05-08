using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image FadeInPage;
    float time = 0f;
    float F_time = 2f;
    
    void Start() { Fade(); } 

    public void Fade() { StartCoroutine(Fade_In()); }

    IEnumerator Fade_In()
    {
        Color alpha = FadeInPage.color;
        while(alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            FadeInPage.color = alpha;
            yield return null;
        }
        FadeInPage.gameObject.SetActive(false);
        yield return null;
    }
}
