using UnityEngine;

public class AttackState : BossState
{
    private float attackTimer = 0f;
    private float bufferDistance = 0.5f; // 공격 거리 여유

    public AttackState(BossMonsterFSM fsm, BossMonsterController controller) : base(fsm, controller) { }
    public override void Enter()
    {
        controller.MonsterMovement.StopMoving();
        attackTimer = 0f;
    }

    public override void Update()
    {
        if (!controller.IsAlive) return;
        float distance = Vector3.Distance(controller.transform.position, controller.Target.position);
        float maxAttackDistance = controller.AttackRange + controller.SizeOffset + bufferDistance;
        if (distance > maxAttackDistance)
        {
            fsm.ChangeState(new ChaseState(fsm, controller));
            return;
        }
        controller.transform.LookAt(controller.Target);
        attackTimer += Time.deltaTime;
        if (attackTimer >= controller.AttackInterval)
        {
            controller.AttackPlayer();
            attackTimer = 0f;
        }
    }
}
