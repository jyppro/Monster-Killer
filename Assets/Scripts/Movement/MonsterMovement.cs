using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator; // 애니메이터

    [SerializeField] private float minMoveDelay = 1f; // 최소 이동 딜레이
    [SerializeField] private float maxMoveDelay = 3f; // 최대 이동 딜레이

    private bool isMoving;

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
            if (!isMoving)
            {
                yield return new WaitForSeconds(Random.Range(minMoveDelay, maxMoveDelay)); // 랜덤한 딜레이

                Vector3 targetPosition = GetRandomTargetPosition();
                MoveTo(targetPosition);
            }
            yield return null;
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        isMoving = true;
        animator.SetBool("Move", true);
        navMeshAgent.SetDestination(targetPosition);
    }

    private Vector3 GetRandomTargetPosition()
    {
        // 랜덤한 위치를 선택하여 반환합니다.
        return new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }

    private void Update()
    {
        if (isMoving && IsReachedDestination())
        {
            StopMoving(); // 도착하면 이동 중지
        }
    }

    private bool IsReachedDestination()
    {
        return navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;
    }

    public void StopMoving()
    {
        // Stop the movement and set the isMoving flag to false
        isMoving = false;
        navMeshAgent.isStopped = true;
        animator.SetBool("Move", false);
    }
}
