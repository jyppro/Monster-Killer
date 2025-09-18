using UnityEngine;

public class ChaseState : BossState
{
    public ChaseState(BossMonsterFSM fsm, BossMonsterController controller) : base(fsm, controller) { }

    public override void Enter()
    {
        controller.Animator.SetBool("Move", true);
        controller.MonsterMovement.SetTarget(controller.Target);
    }

    public override void Update()
    {
        if (!controller.IsAlive) return;

        float distance = Vector3.Distance(controller.transform.position, controller.Target.position);
        float effectiveAttackDistance = controller.AttackRange + controller.SizeOffset;

        if (distance < effectiveAttackDistance)
        {
            fsm.ChangeState(new AttackState(fsm, controller));
            return;
        }

        controller.MonsterMovement.SetTarget(controller.Target); // 계속 추적 보장
        controller.transform.LookAt(controller.Target);
    }

    public override void Exit()
    {
        controller.MonsterMovement.StopMoving();
        controller.Animator.SetBool("Move", false);
    }
}
