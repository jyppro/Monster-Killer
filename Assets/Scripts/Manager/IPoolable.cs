public interface IPoolable
{
    void OnSpawned();   // 풀에서 꺼낼 때 호출
    void OnDespawned(); // 풀로 반환될 때 호출
}
