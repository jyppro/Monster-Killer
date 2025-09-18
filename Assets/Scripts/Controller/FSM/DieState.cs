using UnityEngine;

public class DieState : BossState
{
    public DieState(BossMonsterFSM fsm, BossMonsterController controller) : base(fsm, controller) { }

    public override void Enter()
    {
        controller.OnDeathFSM(); // FSM 버전의 사망 처리 메서드
    }
}
