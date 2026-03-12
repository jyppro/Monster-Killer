using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly Stack<PoolableObject> _poolStack = new Stack<PoolableObject>();
    private readonly GameObject _prefab;
    private readonly Transform _parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            _poolStack.Push(CreateNew());
        }
    }

    private PoolableObject CreateNew()
    {
        GameObject obj = Object.Instantiate(_prefab, _parent);
        var po = obj.GetOrAddComponent<PoolableObject>(); // 확장 메서드 사용 권장
        po.OriginPrefab = _prefab;
        obj.SetActive(false);
        return po;
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        PoolableObject po = _poolStack.Count > 0 ? _poolStack.Pop() : CreateNew();
        
        // 큐 내부에 파괴된 오브젝트가 있을 경우 대비
        if (po == null) return Get(position, rotation);

        Transform t = po.transform;
        t.SetPositionAndRotation(position, rotation);
        po.gameObject.SetActive(true);
        po.OnSpawned();
        
        return po.gameObject;
    }

    public void Return(GameObject obj)
    {
        if (obj.TryGetComponent<PoolableObject>(out var po))
        {
            po.OnDespawned();
            po.gameObject.SetActive(false);
            _poolStack.Push(po);
        }
    }

    public void Clear()
    {
        while (_poolStack.Count > 0)
        {
            var po = _poolStack.Pop();
            if (po != null) Object.Destroy(po.gameObject);
        }
    }
}

// 유틸리티 확장 메서드
public static class GameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        return component != null ? component : obj.AddComponent<T>();
    }
}
