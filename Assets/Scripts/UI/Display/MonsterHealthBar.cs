using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI text;
    private Transform target;

    public void Setup(Transform monster)
    {
        target = monster;
    }

    public void UpdateBar(int current, int max)
    {
        Debug.Log($"UpdateBar 호출됨: current = {current}, max = {max}");
        if (slider == null || text == null)
        {
            Debug.LogWarning("슬라이더 또는 텍스트가 연결되지 않았습니다!");
            return;
        }

        slider.value = (float)current / max;
        text.text = $"{current} / {max}";
    }

    public void ResetBar()
    {
        slider.value = 1f;
        text.text = "";
    }

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 4f);
    }

    public void ClearTarget()
    {
        target = null;
    }
}

