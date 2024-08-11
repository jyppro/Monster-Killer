using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DefenseMonsterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private DefenseMonsterController monsterController;

    [SerializeField] private Transform[] targets; // 몬스터가 추적할 목표 오브젝트 배열
    [SerializeField] private float moveSpeed = 2f; // 이동 속도
    [SerializeField] private float stopDistance = 1.5f; // 목표에 도달한 것으로 간주할 거리

    private Transform currentTarget; // 현재 이동 중인 타겟

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterController = GetComponent<DefenseMonsterController>(); // 몬스터 컨트롤러 참조
        targets = FindTargetsWithTag("Dest"); // "destination" 태그를 가진 오브젝트 찾기

        navMeshAgent.speed = moveSpeed; // 이동 속도 설정
        navMeshAgent.autoTraverseOffMeshLink = true; // Off-Mesh Link 자동 사용 활성화

        SelectRandomTarget();
    }

    private void Update()
    {
        // Off-Mesh Link 처리
        if (navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(HandleOffMeshLink());
        }

        if (currentTarget != null)
        {
            MoveToTarget();
        }
    }

    private Transform[] FindTargetsWithTag(string tag)
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(tag);
        Transform[] targetTransforms = new Transform[targetObjects.Length];
        for (int i = 0; i < targetObjects.Length; i++)
        {
            targetTransforms[i] = targetObjects[i].transform;
        }
        return targetTransforms;
    }

    private void SelectRandomTarget()
    {
        if (targets.Length > 0)
        {
            currentTarget = targets[Random.Range(0, targets.Length)];
            navMeshAgent.SetDestination(currentTarget.position);
            animator.SetBool("Move", true);
        }
    }

    private void MoveToTarget()
    {
        navMeshAgent.SetDestination(currentTarget.position);

        if (navMeshAgent.remainingDistance <= stopDistance)
        {
            //StopMoving();
            animator.SetBool("Move", false);

            // 이동이 완료되면 몬스터 컨트롤러에게 타겟 정보를 전달하고 공격 실행
            if (monsterController != null)
            {
                monsterController.SetTarget(currentTarget);
            }
        }
        else
        {
            animator.SetBool("Move", true);
        }
    }

    private IEnumerator HandleOffMeshLink()
    {
        OffMeshLinkData linkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 endPos = linkData.endPos;

        // 간단하게 AI를 끝점으로 이동시키는 예제
        navMeshAgent.CompleteOffMeshLink();
        navMeshAgent.transform.position = endPos;

        // 필요한 경우 추가 로직 또는 애니메이션 추가
        animator.SetBool("Move", true);

        yield return null;
    }

    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("Move", false);
    }
}
