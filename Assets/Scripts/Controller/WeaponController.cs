using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject WeaponGenerator;
    public int baseDamage = 10; // 기본 데미지, 10으로 데이터 설정
    public int currentDamage = 0; // 강화 추가 데미지
    public int partsDamage = 0; // 부위 별 추가 데미지
    public AudioSource[] weaponAudios; // 하위 오브젝트들의 오디오 소스를 배열로 저장
    public ParticleSystem weaponParticleSystem;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        WeaponGenerator = GameObject.Find("WeaponGenerator");

        // 프리팹의 하위에 있는 모든 AudioSource를 가져온다
        weaponAudios = GetComponentsInChildren<AudioSource>();

        // 파티클 시스템 초기화
        weaponParticleSystem = GetComponent<ParticleSystem>();
    }

    public void Shoot(Vector3 dir)
    {
        rb.AddForce(dir); // 들어온 입력벡터만큼 오브젝트에 힘이 가해진다.
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayAudio();

        rb.isKinematic = true; // 충돌 시 중력 무시

        // 파티클 실행
        if (weaponParticleSystem != null)
        {
            weaponParticleSystem.Play();
        }

        if (!collision.gameObject.CompareTag("terrain"))
        {
            CalculatePartsDamage(collision);

            int totalDamage = baseDamage + currentDamage + partsDamage; // 총 데미지 계산
            ApplyDamageToSpecificMonster(collision, totalDamage);

            Destroy(gameObject, 0.5f); // 오브젝트 파괴
        }
        else
        {
            Destroy(gameObject); // 지형 충돌 시 즉시 파괴
        }
    }

    private void PlayAudio()
    {
        if (weaponAudios != null)
        {
            foreach (AudioSource audioSource in weaponAudios)
            {
                if (audioSource != null && audioSource.enabled)
                {
                    audioSource.Play(); // 오디오 재생
                }
            }
        }
    }

    private void CalculatePartsDamage(Collision collision)
    {
        // 충돌한 부위에 따라 추가 데미지를 계산
        switch (collision.gameObject.tag)
        {
            case "Head":
                partsDamage += Random.Range(30, 50);
                break;
            case "L_Leg":
            case "R_Leg":
                partsDamage += Random.Range(25, 40);
                break;
            case "Body":
                partsDamage += Random.Range(20, 35);
                break;
            case "Tail":
                partsDamage += Random.Range(10, 50);
                break;
            case "Wing":
                partsDamage += Random.Range(40, 60);
                break;
            case "Neck":
                partsDamage += Random.Range(55, 60);
                break;
        }
    }

    private void ApplyDamageToSpecificMonster(Collision collision, int totalDamage)
    {
        Component[] monsterControllers = collision.gameObject.GetComponentsInParent<Component>();

        foreach (var comp in monsterControllers)
        {
            if (comp is HuntMonsterController hunt)
            {
                hunt.TakeDamage_M(totalDamage);
                hunt.ShowDamageText(totalDamage, collision.GetContact(0).point);
                return;
            }
            else if (comp is MonsterController monster)
            {
                monster.TakeDamage_M(totalDamage);
                monster.ShowDamageText(totalDamage, collision.GetContact(0).point);
                return;
            }
            else if (comp is DefenseMonsterController defense)
            {
                defense.TakeDamage_M(totalDamage);
                defense.ShowDamageText(totalDamage, collision.GetContact(0).point);
                return;
            }
            else if (comp is BossMonsterController boss)
            {
                boss.TakeDamage_M(totalDamage);
                boss.ShowDamageText(totalDamage, collision.GetContact(0).point);
                return;
            }
        }

        Debug.LogWarning("충돌 대상에서 몬스터 컨트롤러를 찾지 못했습니다: " + collision.gameObject.name);
    }

}
