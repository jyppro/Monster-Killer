using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill2Damage : MonoBehaviour
{
    public int baseDamage = 10;  // 기본 데미지
    public float damageInterval = 0.5f;  // 도트 데미지 간격
    public float duration = 3.0f; // 총 지속 시간
    public float multiplier = 2f; // 지속 데미지 배수

    private Coroutine damageCoroutine;  // 도트 데미지를 적용하는 코루틴
    private float totalDamage; // 총 데미지
    private WeaponController weaponController; // 무기 컨트롤러
    private HashSet<Collider> affectedParts = new HashSet<Collider>(); // 데미지를 입힌 부위 관리

    void Start()
    {
        weaponController = FindObjectOfType<WeaponController>(); // WeaponController에서 현재 무기 공격력을 가져옴
        totalDamage = weaponController.currentDamage * multiplier; // 무기 데미지에 배수 적용
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsMonsterPart(other)) // 몬스터의 부위와 충돌 시
        {
            if (affectedParts.Add(other)) // 부위를 추가하고, 이미 있는 경우는 제외
            {
                if (damageCoroutine == null)
                {
                    damageCoroutine = StartCoroutine(ApplyDotDamage()); // 코루틴 시작
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsMonsterPart(other))
        {
            affectedParts.Remove(other); // 부위를 제거
            // 모든 부위에서 나간 경우에만 코루틴 중지
            if (affectedParts.Count == 0 && damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDotDamage()
    {
        float damagePerTick = totalDamage / (duration / damageInterval); // 틱마다 주는 데미지 계산
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            foreach (var part in affectedParts)
            {
                if (part != null) // 파츠가 여전히 존재하는지 확인
                {
                    ApplyDamageToMonsterPart(part, damagePerTick); // 부위에 데미지 적용
                }
            }
            elapsedTime += damageInterval;
            yield return new WaitForSeconds(damageInterval); // 지정된 간격만큼 대기
        }

        damageCoroutine = null; // 데미지 완료 후 코루틴 해제
    }

    private void ApplyDamageToMonsterPart(Collider monsterPart, float damage)
    {
        int partDamage = CalculateDamage(monsterPart); // 부위에 따라 데미지 계산

        // 몬스터 부위가 속한 몬스터의 컨트롤러를 가져옴
        MonsterController monsterController = monsterPart.transform.root.GetComponent<MonsterController>();
        if (monsterController != null)
        {
            monsterController.TakeDamage_M(partDamage);
            monsterController.ShowDamageText(partDamage, monsterPart.transform.position);
            return; // 이미 몬스터 컨트롤러가 처리되었으므로 나가기
        }

        // 다른 몬스터 컨트롤러도 확인
        HuntMonsterController huntMonsterController = monsterPart.transform.root.GetComponent<HuntMonsterController>();
        if (huntMonsterController != null)
        {
            huntMonsterController.TakeDamage_M(partDamage);
            huntMonsterController.ShowDamageText(partDamage, monsterPart.transform.position);
            return;
        }

        DefenseMonsterController defenseMonsterController = monsterPart.transform.root.GetComponent<DefenseMonsterController>();
        if (defenseMonsterController != null)
        {
            defenseMonsterController.TakeDamage_M(partDamage);
            defenseMonsterController.ShowDamageText(partDamage, monsterPart.transform.position);
            return;
        }

        BossMonsterController bossMonsterController = monsterPart.transform.root.GetComponent<BossMonsterController>();
        if (bossMonsterController != null)
        {
            bossMonsterController.TakeDamage_M(partDamage);
            bossMonsterController.ShowDamageText(partDamage, monsterPart.transform.position);
        }
    }

    private int CalculateDamage(Collider monsterPart)
    {
        int partDamage = (int)(baseDamage + totalDamage);

        // 태그에 따라 추가 데미지를 설정
        switch (monsterPart.tag)
        {
            case "Head":
                partDamage += Random.Range(30, 50);  // 머리 데미지
                break;
            case "L_Leg":
            case "R_Leg":
                partDamage += Random.Range(25, 40);  // 다리 데미지
                break;
            case "Body":
                partDamage += Random.Range(20, 35);  // 몸통 데미지
                break;
            case "Tail":
                partDamage += Random.Range(10, 50);  // 꼬리 데미지
                break;
            case "Wing":
                partDamage += Random.Range(40, 60);  // 날개 데미지
                break;
            case "Neck":
                partDamage += Random.Range(55, 60);  // 목 데미지
                break;
        }

        return partDamage;
    }

    private bool IsMonsterPart(Collider collider)
    {
        // 몬스터의 각 부위 태그를 확인하여 해당 태그가 있으면 true 반환
        return collider.CompareTag("Head") || collider.CompareTag("L_Leg") || collider.CompareTag("R_Leg") ||
               collider.CompareTag("Body") || collider.CompareTag("Tail") || collider.CompareTag("Wing") || collider.CompareTag("Neck");
    }
}
