using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject WeaponGenerator;
    public int damage = 10; // 기본 데미지, 10으로 데이터 설정
    public int currentDamage = 0; // 강화 추가 데미지
    public int PartsDamage = 0; // 부위 별 추가 데미지
    public AudioSource[] WeaponAudios; // 하위 오브젝트들의 오디오 소스를 배열로 저장
    public ParticleSystem ParticleSystem;

    public void Shoot(Vector3 dir) //인자로 3차원 벡터가 입력되고
    {
        GetComponent<Rigidbody>().AddForce(dir); // 들어온 입력벡터 만큼 오브젝트에 힘이 가해진다.
    }
    
    void Start()
    {
        this.WeaponGenerator = GameObject.Find("WeaponGenerator");
        if(WeaponAudios != null)
        {
            // 프리팹의 하위에 있는 모든 AudioSource를 가져온다
            WeaponAudios = GetComponentsInChildren<AudioSource>();
        }

        if(ParticleSystem != null)
        {
            ParticleSystem = GetComponent<ParticleSystem>();
        }
    }

    private void OnCollisionEnter(Collision collision) //다른 물체와 충돌하는 순간
    {
        if(WeaponAudios != null)
        {
            // 각각의 하위 오브젝트에 있는 오디오 소스를 재생
            foreach (AudioSource audioSource in WeaponAudios)
            {
                if(audioSource != null && audioSource.enabled)
                {
                    audioSource.Play(); // 오디오 재생
                }
            }
        }
        
        GetComponent<Rigidbody>().isKinematic = true; //중력 무시

        if(ParticleSystem != null)
        {
            ParticleSystem.Play(); // 파티클 실행
        }

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