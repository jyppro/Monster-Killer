using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPoolManager : MonoBehaviour
{
    public static HealthBarPoolManager Instance { get; private set; }
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<MonsterHealthBar> pool = new Queue<MonsterHealthBar>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(healthBarPrefab, canvas.transform);
            obj.SetActive(false);
            pool.Enqueue(obj.GetComponent<MonsterHealthBar>());
        }
    }

    public MonsterHealthBar Get()
    {
        if (pool.Count > 0)
        {
            MonsterHealthBar bar = pool.Dequeue();
            bar.gameObject.SetActive(true);
            return bar;
        }

        // 부족하면 새로 생성
        return Instantiate(healthBarPrefab, FindObjectOfType<Canvas>().transform).GetComponent<MonsterHealthBar>();
    }

    public void Return(MonsterHealthBar bar)
    {
        bar.ResetBar();
        bar.ClearTarget();
        bar.gameObject.SetActive(false);
        pool.Enqueue(bar);
    }
}

