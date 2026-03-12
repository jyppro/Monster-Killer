public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }
    // Key를 InstanceID(int)로 사용하여 Dictionary 조회 성능 최적화
    private Dictionary<int, ObjectPool> _pools = new Dictionary<int, ObjectPool>();

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void CreatePool(GameObject prefab, int size, Transform parent = null)
    {
        int id = prefab.GetInstanceID();
        if (!_pools.ContainsKey(id)) _pools.Add(id, new ObjectPool(prefab, size, parent));
    }

    public GameObject GetFromPool(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        int id = prefab.GetInstanceID();
        if (_pools.TryGetValue(id, out var pool)) return pool.Get(pos, rot);
        return Instantiate(prefab, pos, rot);
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj.TryGetComponent<PoolableObject>(out var po) && po.OriginPrefab != null)
        {
            if (_pools.TryGetValue(po.OriginPrefab.GetInstanceID(), out var pool))
            {
                pool.Return(obj);
                return;
            }
        }
        Destroy(obj);
    }
}
