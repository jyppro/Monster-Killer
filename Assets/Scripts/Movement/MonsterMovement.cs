using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private Transform target; // 플레이어 위치 참조

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target == null) return;

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);

        // 애니메이션 제어
        if (navMeshAgent.velocity.magnitude > 0.1f)
            animator.SetBool("Move", true);
        else
            animator.SetBool("Move", false);
    }

    public void StopMoving()
    {
        if (navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = true;
            animator.SetBool("Move", false);
        }
    }

    public void SetStoppingDistance(float distance)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.stoppingDistance = distance;
        }
    }
}

