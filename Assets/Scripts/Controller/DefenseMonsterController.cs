using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DefenseMonsterController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 몬스터 체력바 UI
    [SerializeField] private TextMeshProUGUI textHP; // 체력바 텍스트 UI
    [SerializeField] private GameObject damageTextPrefab; // 데미지 텍스트 UI
    [SerializeField] private AudioClip[] Clips; // 사용할 오디오 클립의 배열 <0 : 소환, 1 : 공격, 2 : 피격, 3 : 사망>
    [SerializeField] private int MonsterMaxHealth = 100; // 몬스터 최대 체력
    [SerializeField] private int MonsterCurrentHealth = 100; // 몬스터 현재 체력
    [SerializeField] private int MonsterPower = 5; // 몬스터 공격력
    private AudioSource MonsterAudio; // 몬스터 사운드
    private Animator animator; // 애니메이터
    private Canvas canvas; // 캔버스
    private bool isAlive = true; // 몬스터 생사 여부
    private MonsterSpawner spawner;
    private bool canAttack = true; // 공격 가능 여부

    [SerializeField] private Transform target; // 현재 타겟
    [SerializeField] private float attackDistance = 1.5f; // 공격할 거리
    [SerializeField] private float attackCooldown = 2f; // 공격 쿨다운 시간
    private DefenseMonsterMovement defenseMonsterMovement;
    private List<GameObject> damageTextObjects = new List<GameObject>(); // 데미지 텍스트 리스트

    void Start()
    {
        healthSlider = FindObjectOfType<Slider>(); // 씬의 존재하는 슬라이더 찾아서 넣어주기
        defenseMonsterMovement = this.GetComponent<DefenseMonsterMovement>();

        if (healthSlider != null)
        {
            textHP = healthSlider.GetComponentInChildren<TextMeshProUGUI>(); // 체력바 하위의 TextMeshProUGUI 찾기
        }
        canvas = FindObjectOfType<Canvas>(); // Canvas 연결
        animator = GetComponent<Animator>(); // 애니메이터 연결
        MonsterAudio = GetComponent<AudioSource>(); // 오디오 소스 연결
        MonsterAudio.clip = Clips[0]; // 몬스터 소환 효과음
        MonsterAudio.Play();

        UpdateHealthSlider();

        spawner = FindObjectOfType<MonsterSpawner>();
    }

    void Update()
    {
        if (target != null && isAlive)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackDistance && canAttack)
            {
                StartCoroutine(HandleAttack()); // 공격을 처리하는 코루틴 호출
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private IEnumerator HandleAttack()
    {
        if (canAttack) // 공격 가능 여부 확인
        {
            MonsterAttack(); // 공격 실행
            canAttack = false; // 공격 불가 상태로 설정

            // 공격 쿨다운 처리
            yield return new WaitForSeconds(attackCooldown);

            canAttack = true; // 쿨다운 후 다시 공격 가능
        }
    }

    private void MonsterAttack()
    {
        canAttack = false; // 공격 불가 상태로 설정
        animator.SetTrigger("Attack");
        MonsterAudio.clip = Clips[1]; // 몬스터 공격 효과음
        MonsterAudio.Play();

        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.TakeDamage_P(MonsterPower); // 타겟에 데미지 적용
        }

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; // 쿨다운 후 다시 공격 가능
    }

    public void TakeDamage_M(int damage) // 몬스터에게 데미지를 주는 함수
    {
        if (!isAlive) return; // 몬스터가 죽었다면 함수를 종료
        
        MonsterCurrentHealth -= damage;
        MonsterAudio.clip = Clips[2]; // 몬스터 피격 효과음
        MonsterAudio.Play();

        // 체력이 전부 소진되면 몬스터 사망 처리
        if (MonsterCurrentHealth <= 0)
        {
            OnDeath(); // 몬스터 사망 처리 함수 호출
        }
        UpdateHealthSlider();
    }

    private void OnDeath()
    {
        isAlive = false;
        MonsterCurrentHealth = 0;
        animator.SetBool("Death", true);

        MonsterAudio.clip = Clips[3]; // 몬스터 사망 효과음
        MonsterAudio.Play();

        if(defenseMonsterMovement != null)
        {
            defenseMonsterMovement.StopMoving(); // Stop the movement
        }

        // 하위의 모든 콜라이더를 비활성화
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        // 3초 뒤에 몬스터 제거
        Destroy(gameObject, 3f);

        // 스포너의 MonsterDied 메서드 호출
        if (spawner != null)
        {
            spawner.MonsterDied();
        }
    }

    private void UpdateHealthSlider() // 체력바 업데이트
    {
        // 체력바 슬라이더의 값을 현재 체력 비율로 설정
        float decreaseHp = (float)MonsterCurrentHealth / MonsterMaxHealth;
        healthSlider.value = decreaseHp;
        textHP.text = $"{MonsterCurrentHealth} / {MonsterMaxHealth}";
    }

    public void ShowDamageText(float damage, Vector3 position) // 몬스터가 받는 데미지 보여주기
    {
        // 겹침을 방지하기 위해 랜덤 오프셋 생성
        float randomOffsetX = Random.Range(-2.0f, 2.0f); // X 방향 랜덤 오프셋 (범위 확대)
        float randomOffsetY = Random.Range(1.0f, 2.0f); // Y 방향 랜덤 오프셋 (범위 확대)

        // 랜덤 오프셋을 적용한 새로운 월드 위치 생성
        Vector3 randomPosition = position + new Vector3(randomOffsetX, randomOffsetY, 0);

        // 랜덤 위치에서 데미지 텍스트 오브젝트 생성
        GameObject damageTextObject = Instantiate(damageTextPrefab, randomPosition, Quaternion.identity);
        
        // 위치를 스크린 공간으로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(randomPosition);
        
        // RectTransform의 부모 설정 및 위치 지정
        RectTransform rt = damageTextObject.GetComponent<RectTransform>();
        rt.SetParent(canvas.transform, false); // 캔버스의 자식으로 설정
        rt.position = screenPosition; // 스크린 위치로 설정

        // 데미지 텍스트 설정
        TextMeshProUGUI textComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = "-" + damage.ToString();
        
        // 데미지 텍스트 오브젝트를 리스트에 추가
        damageTextObjects.Add(damageTextObject); // 리스트에 추가

        StartCoroutine(AnimateDamageText(textComponent));
    }

    private IEnumerator AnimateDamageText(TextMeshProUGUI textComponent) // 데미지에 애니메이션 적용
    {
        float duration = 1.5f; // 애니메이션 지속 시간
        float elapsedTime = 0f;

        Vector3 startPosition = textComponent.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 50.0f; // 위로 올라갈 위치
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 투명도 조정

        while (elapsedTime < duration)
        {
            // 텍스트 컴포넌트가 여전히 존재하는지 확인
            if (textComponent == null)
            {
                yield break; // 텍스트가 파괴되었으면 코루틴 종료
            }

            float progress = elapsedTime / duration;
            textComponent.transform.position = Vector3.Lerp(startPosition, endPosition, progress); // 위치 이동
            textComponent.color = Color.Lerp(startColor, endColor, progress); // 투명도 조정
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (textComponent != null) // 애니메이션 종료 후 텍스트 오브젝트가 존재하면 제거
        {
            Destroy(textComponent.gameObject);
        }
    }
}