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
        InitializeComponents();
        targets = FindTargetsWithTag("Dest"); // "Dest" 태그를 가진 오브젝트 찾기
        SetNavMeshAgentSettings();
        SelectRandomTarget();
    }

    private void Update()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            StartCoroutine(HandleOffMeshLink());
        }
        else if (currentTarget != null)
        {
            MoveToTarget();
        }
    }

    private void InitializeComponents()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterController = GetComponent<DefenseMonsterController>(); // 몬스터 컨트롤러 참조
    }

    private void SetNavMeshAgentSettings()
    {
        navMeshAgent.speed = moveSpeed; // 이동 속도 설정
        navMeshAgent.autoTraverseOffMeshLink = true; // Off-Mesh Link 자동 사용 활성화
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
        if (navMeshAgent.remainingDistance <= stopDistance)
        {
            OnTargetReached();
        }
        else
        {
            navMeshAgent.SetDestination(currentTarget.position);
            animator.SetBool("Move", true);
        }
    }

    private void OnTargetReached()
    {
        animator.SetBool("Move", false);
        monsterController?.SetTarget(currentTarget); // null 체크를 이용한 안전한 호출
    }

    private IEnumerator HandleOffMeshLink()
    {
        OffMeshLinkData linkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 endPos = linkData.endPos;

        navMeshAgent.CompleteOffMeshLink();
        navMeshAgent.transform.position = endPos;
        animator.SetBool("Move", true);

        yield return null; // 다음 프레임까지 대기
    }

    public void StopMoving()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("Move", false);
    }
}
