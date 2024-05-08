using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isMoving = false;
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
            if (!isMoving)
            {
                // 이동을 시작하기 전에 랜덤한 딜레이를 줍니다.
                float moveDelay = Random.Range(minMoveDelay, maxMoveDelay);
                yield return new WaitForSeconds(moveDelay);

                // 새로운 목표 지점을 선택하여 이동합니다.
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
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        return randomPosition;
    }

    private void Update()
    {
        if (isMoving && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // 이동이 완료되었을 때 이동 상태를 해제합니다.
            isMoving = false;
            animator.SetBool("Move", false);
        }
    }
}
