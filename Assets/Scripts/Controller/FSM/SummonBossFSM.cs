public class SummonBossFSM : BossMonsterFSM
{
    public override void InitializeFSM(BossMonsterController controller)
    {
        ChangeState(new SummonState(this, (SummonBossController)controller));
    }
}
