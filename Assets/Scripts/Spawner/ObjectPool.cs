using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> poolQueue = new Queue<GameObject>();
    private GameObject prefab;
    private Transform parent;

    public ObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, parent);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;

        // 유효한 오브젝트가 나올 때까지 큐에서 꺼내기
        while (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
            if (obj != null) break;
        }

        // 파괴되었거나 큐가 비어 있었다면 새로 생성
        if (obj == null) obj = GameObject.Instantiate(prefab, parent);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawned();
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null) return;
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnDespawned();
        obj.SetActive(false);

        // 이미 Destroy 되었는지 확인 (비정상 상황 방지)
        if (obj != null) poolQueue.Enqueue(obj);
    }

    public void Clear()
    {
        while (poolQueue.Count > 0)
        {
            var obj = poolQueue.Dequeue();
            if (obj != null)
                GameObject.Destroy(obj);
        }
    }
}
