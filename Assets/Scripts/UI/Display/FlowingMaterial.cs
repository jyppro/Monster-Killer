using UnityEngine;

public class FlowingMaterial : MonoBehaviour
{
    public Material material;  // 적용할 머티리얼
    public float scrollSpeedX = 0.5f;  // X축 흐르는 속도
    public float scrollSpeedY = 0.5f;  // Y축 흐르는 속도
    private float offsetX;
    private float offsetY;

    void Start()
    {
        offsetX = 0.0f;
        offsetY = 0.0f;
    }
    
    void Update()
    {
        // 시간에 따라 오프셋 값을 증가시켜 텍스처가 X축, Y축 모두에서 흐르는 효과를 줌
        offsetX = Time.time * scrollSpeedX;
        offsetY = Time.time * scrollSpeedY;
        material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));  // X, Y 방향 모두 흐르게 설정
    }
}
