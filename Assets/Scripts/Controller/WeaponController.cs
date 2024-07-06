using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject WeaponGenerator;
    public int damage;
    public int currentDamage = 0;

    public void Shoot(Vector3 dir) //인자로 3차원 벡터가 입력되고
    { GetComponent<Rigidbody>().AddForce(dir); } // 들어온 입력벡터 만큼 오브젝트에 힘이 가해진다.
    
    void Start() { this.WeaponGenerator = GameObject.Find("WeaponGenerator"); }

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
        GetComponent<Rigidbody>().isKinematic = true; //중력 무시
        GetComponent<ParticleSystem>().Play(); // 파티클 실행

        if (!collision.gameObject.CompareTag("terrain"))
        {
            if (collision.gameObject.CompareTag("Head")) //몬스터의 각 파츠별 충돌 데미지
            { damage = Random.Range(30,50); }
            else if (collision.gameObject.CompareTag("L_Leg"))
            { damage = Random.Range(25,40); }
            else if (collision.gameObject.CompareTag("R_Leg"))
            { damage = Random.Range(25,40); }
            else if (collision.gameObject.CompareTag("Body"))
            { damage = Random.Range(20,35); }
            else if (collision.gameObject.CompareTag("Tail"))
            { damage = Random.Range(10,50); }
            else if (collision.gameObject.CompareTag("Wing"))
            { damage = Random.Range(40,60); }
            damage += currentDamage;
            collision.gameObject.transform.root.GetComponent<MonsterController>().TakeDamage_M(damage);
            collision.gameObject.transform.root.GetComponent<MonsterController>().ShowDamageText(damage, collision.GetContact(0).point);
            Destroy(gameObject, 0.5f);
        }
        else { Destroy(gameObject); }
    }
}