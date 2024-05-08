using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject WeaponGenerator;
    public float damage;
    public float currentDamage = 0f;

    public void Shoot(Vector3 dir) //인자로 3차원 벡터가 입력되고
    { GetComponent<Rigidbody>().AddForce(dir); } // 들어온 입력벡터 만큼 오브젝트에 힘이 가해진다.
    
    void Start() { this.WeaponGenerator = GameObject.Find("WeaponGenerator"); }

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