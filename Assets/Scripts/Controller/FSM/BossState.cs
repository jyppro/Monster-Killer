public abstract class BossState
{
    protected BossMonsterFSM fsm;
    protected BossMonsterController controller;

    public BossState(BossMonsterFSM fsm, BossMonsterController controller)
    {
        this.fsm = fsm;
        this.controller = controller;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
