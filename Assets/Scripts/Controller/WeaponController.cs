using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject WeaponGenerator;
    public int damage = 10; // 기본 데미지, 10으로 데이터 설정
    public int currentDamage = 0; // 강화 추가 데미지
    public int PartsDamage = 0; // 부위 별 추가 데미지
    public AudioSource WeaponAudio; // 몬스터 사운드

    public void Shoot(Vector3 dir) //인자로 3차원 벡터가 입력되고
    { GetComponent<Rigidbody>().AddForce(dir); } // 들어온 입력벡터 만큼 오브젝트에 힘이 가해진다.
    
    void Start()
    {
        this.WeaponGenerator = GameObject.Find("WeaponGenerator");
        WeaponAudio = GetComponent<AudioSource>(); // 오디오 소스 연결
        //currentDamage = GameManager.Instance.GetPower();
    }

    // public void ApplyDamageToMonster(float damage) // 플레이어가 몬스터에게 주는 데미지
    // {
    //     MonsterCurrentHealth -= damage;
    //     MonsterAudio.clip = Clips[2]; // 몬스터 피격 효과음
    //     MonsterAudio.Play();

    //     // 체력이 전부 소진되면 다음 몬스터 소환
    //     if (MonsterCurrentHealth <= 0f)
    //     {
    //         isAlive = false;
    //         MonsterCurrentHealth = MonsterMaxHealth;
    //         animator.SetBool("Death", true);
    //         gameObject.GetComponent<MonsterMovement>().enabled = false;
    //         GameObject.Find("GoldText").GetComponent<GoldController>().GoldSum(goldReward); // 수정 필요 -> 골드 데이터 값으로
    //         MonsterAudio.clip = Clips[3]; // 몬스터 사망 효과음
    //         MonsterAudio.Play();
    //         Invoke("SpawnNextMonster", 3f);
    //     }
    //     UpdateHealthSlider();
    // }

    private void OnCollisionEnter(Collision collision) //다른 물체와 충돌하는 순간
    {
        WeaponAudio.Play();
        GetComponent<Rigidbody>().isKinematic = true; //중력 무시
        GetComponent<ParticleSystem>().Play(); // 파티클 실행


        if (!collision.gameObject.CompareTag("terrain"))
        {
            if (collision.gameObject.CompareTag("Head")) //몬스터의 각 파츠별 충돌 데미지
            { PartsDamage += Random.Range(30, 50); }
            else if (collision.gameObject.CompareTag("L_Leg"))
            { PartsDamage += Random.Range(25, 40); }
            else if (collision.gameObject.CompareTag("R_Leg"))
            { PartsDamage += Random.Range(25, 40); }
            else if (collision.gameObject.CompareTag("Body"))
            { PartsDamage += Random.Range(20, 35); }
            else if (collision.gameObject.CompareTag("Tail"))
            { PartsDamage += Random.Range(10, 50); }
            else if (collision.gameObject.CompareTag("Wing"))
            { PartsDamage += Random.Range(40, 60); }
            else if (collision.gameObject.CompareTag("Neck"))
            { PartsDamage += Random.Range(55, 60); }

            damage += currentDamage; // 강화 데미지 합산
            damage += PartsDamage; // 파츠 별 데미지 합산

            MonsterController monsterController = collision.gameObject.transform.root.GetComponent<MonsterController>();
            HuntMonsterController huntMonsterController = collision.gameObject.transform.root.GetComponent<HuntMonsterController>();
            DefenseMonsterController defenseMonsterController = collision.gameObject.transform.root.GetComponent<DefenseMonsterController>();
            BossMonsterController bossMonsterController = collision.gameObject.transform.root.GetComponent<BossMonsterController>();
            if(monsterController)
            {
                monsterController.TakeDamage_M(damage);
                monsterController.ShowDamageText(damage, collision.GetContact(0).point);
            }
            else if(huntMonsterController)
            {
                huntMonsterController.TakeDamage_M(damage);
                huntMonsterController.ShowDamageText(damage, collision.GetContact(0).point);
            }
            else if(defenseMonsterController)
            {
                defenseMonsterController.TakeDamage_M(damage);
                defenseMonsterController.ShowDamageText(damage, collision.GetContact(0).point);
            }
            else if(bossMonsterController)
            {
                bossMonsterController.TakeDamage_M(damage);
                bossMonsterController.ShowDamageText(damage, collision.GetContact(0).point);
            }
            
            
            Destroy(gameObject, 0.5f);
        }
        else { Destroy(gameObject); }
    }
}