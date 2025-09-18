using UnityEngine;

public class IdleState : BossState
{
    public IdleState(BossMonsterFSM fsm, BossMonsterController controller)
        : base(fsm, controller) { }

    public override void Enter()
    {
        controller.Animator?.SetBool("IsIdle", true);

        // 고정형 보스는 이동하지 않음
        controller.MonsterMovement?.StopMoving();
    }

    public override void Update()
    {
        if (!controller.IsAlive) return;

        if (controller is SummonBossController)
        {
            // Idle 애니메이션이 1회 끝났는지 체크
            AnimatorStateInfo stateInfo = controller.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Idle") && stateInfo.normalizedTime >= 1.0f)
            {
                fsm.ChangeState(new SummonState(fsm, controller as SummonBossController));
            }

            return; // 소환 보스는 추적하지 않음
        }

        // 일반 보스만 추적
        float distance = Vector3.Distance(controller.transform.position, controller.Target.position);
        if (distance < controller.DetectionRange)
        {
            fsm.ChangeState(new ChaseState(fsm, controller));
        }
    }

    public override void Exit()
    {
        controller.Animator?.SetBool("IsIdle", false);
    }
}
