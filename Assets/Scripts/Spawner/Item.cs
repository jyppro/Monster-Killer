using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public event Action onCollected;

    private void OnDestroy()
    {
        // 아이템이 제거될 때 이벤트 호출
        onCollected?.Invoke();
    }
}
