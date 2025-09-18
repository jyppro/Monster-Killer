using UnityEngine;

public class SummonBossController : BossMonsterController
{
    protected override void InitializeFSM()
    {
        fsm.ChangeState(new IdleState(fsm, this));
    }
}



