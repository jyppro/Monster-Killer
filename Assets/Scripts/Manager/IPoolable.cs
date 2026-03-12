using UnityEngine;

public interface IPoolable
{
    void OnSpawned();   // 풀에서 꺼낼 때 호출
    void OnDespawned(); // 풀로 반환될 때 호출
}


public class PoolableObject : MonoBehaviour
{
    // 이 오브젝트가 어떤 프리팹으로부터 생성되었는지 저장 (반환 시 사용)
    public GameObject OriginPrefab { get; set; }
    private IPoolable[] _poolables;

    private void Awake()
    {
        // GetComponent를 미리 한 번만 수행하여 캐싱
        _poolables = GetComponents<IPoolable>();
    }

    public void OnSpawned()
    {
        foreach (var p in _poolables) p.OnSpawned();
    }

    public void OnDespawned()
    {
        foreach (var p in _poolables) p.OnDespawned();
    }
}
