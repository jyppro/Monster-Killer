using UnityEngine;

public class SummonState : BossState
{
    private float summonTimer = 0f;
    private float normalDelay = 6f;
    private float fastDelay = 2f;

    public SummonState(BossMonsterFSM fsm, SummonBossController controller)
        : base(fsm, controller) { }

    public override void Enter()
    {
        summonTimer = 0f;
    }

    public override void Update()
    {
        var boss = controller as SummonBossController;

        if (!boss.IsAlive || boss.Target == null || boss.SummonPrefab == null)
            return;

        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0f)
        {
            Summon();
            
            // 체력 50% 이하일 경우 소환 속도 증가
            float delay = boss.CurrentHealth < boss.MaxHealth * 0.5f ? fastDelay : normalDelay;
            summonTimer = delay;
        }
    }

    private void Summon()
    {
        var boss = controller as SummonBossController;

        // 소환 위치는 플레이어 위치로
        Vector3 spawnPos = boss.Target.position;
        Quaternion rot = boss.SummonPrefab.transform.rotation;

        GameObject.Instantiate(boss.SummonPrefab, spawnPos, rot);
        Debug.Log("플레이어 위치에 몬스터 소환");

        // 소환 애니메이션 트리거 실행
        controller.Animator.SetTrigger("Attack");
    }

    public override void Exit()
    {
        Debug.Log("소환 상태 종료");
    }
}
