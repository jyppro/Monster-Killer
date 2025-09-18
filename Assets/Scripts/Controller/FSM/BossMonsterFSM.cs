using UnityEngine;

public class BossMonsterFSM : MonoBehaviour
{
    private BossState currentState;
    public BossState CurrentState => currentState;  // 현재 상태 읽기용

    public virtual void InitializeFSM(BossMonsterController controller)
    {
        ChangeState(new IdleState(this, controller));
    }

    public void ChangeState(BossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.Update();
    }
}

