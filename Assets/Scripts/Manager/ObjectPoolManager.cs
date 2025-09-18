using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 살아있게 유지 (선택사항)
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(GameObject prefab, int initialSize, Transform parent = null)
    {
        if (!pools.ContainsKey(prefab))
        {
            ObjectPool pool = new ObjectPool(prefab, initialSize, parent);
            pools.Add(prefab, pool);
        }
    }

    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogWarning("GetFromPool: prefab is null.");
            return null;
        }

        if (pools.TryGetValue(prefab, out var pool)) return pool.Get(position, rotation);
        return Instantiate(prefab, position, rotation);
    }

    public void ReturnToPool(GameObject prefab, GameObject obj)
    {
        if (prefab == null || obj == null) return;

        if (pools.TryGetValue(prefab, out var pool))
            pool.Return(obj);
        else
            Destroy(obj); // 비정상 케이스: 풀에 없음
    }

    // 씬 전환 시 호출하여 모든 풀을 정리
    public void ClearAllPools()
    {
        // ObjectPool에 이 메서드 필요 (아래에 정의함)
        foreach (var pool in pools.Values) pool.Clear();
        pools.Clear();
    }
}
