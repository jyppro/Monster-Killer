using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator; // 애니메이터

    [SerializeField] private float minMoveDelay = 1f; // 최소 이동 딜레이
    [SerializeField] private float maxMoveDelay = 3f; // 최대 이동 딜레이

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // 애니메이터 연결
        StartCoroutine(MoveCoroutine()); // 처음에 이동을 시작하도록 호출
    }

    private IEnumerator MoveCoroutine()
    {
        while (true)
        {
            // 랜덤한 딜레이
            yield return new WaitForSeconds(Random.Range(minMoveDelay, maxMoveDelay));

            Vector3 targetPosition = GetRandomTargetPosition();
            MoveTo(targetPosition);

            // 몬스터가 목적지에 도착할 때까지 대기
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
            {
                yield return null; // 대기
            }

            StopMoving(); // 도착하면 이동 중지
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        animator.SetBool("Move", true);
        navMeshAgent.SetDestination(targetPosition);
        navMeshAgent.isStopped = false; // 이동 시작
    }

    private Vector3 GetRandomTargetPosition()
    {
        // 랜덤한 위치를 선택하여 반환합니다.
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }

    public void StopMoving()
    {
        // NavMeshAgent가 활성화되어 있고 NavMesh에 연결되어 있을 때만 멈추기
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("Move", false);
        }
    }
}
