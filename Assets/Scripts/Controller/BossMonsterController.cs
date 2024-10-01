using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BossMonsterController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 보스 체력바 UI
    [SerializeField] private TextMeshProUGUI textHP; // 체력바 텍스트 UI
    [SerializeField] private GameObject damageTextPrefab; // 데미지 텍스트 UI
    [SerializeField] private AudioClip[] Clips; // 사용할 오디오 클립의 배열 <0 : 소환, 1 : 공격, 2 : 피격, 3 : 사망>
    [SerializeField] private int MonsterMaxHealth = 1000; // 보스 최대 체력
    [SerializeField] private int MonsterCurrentHealth = 1000; // 보스 현재 체력
    [SerializeField] private int attackInterval = 10; // 공격 간격
    // [SerializeField] private float attackRange = 2.0f; // 공격 범위
    [SerializeField] public int MonsterPower = 50; // 보스 공격력
    [SerializeField] private LayerMask playerLayer; // 플레이어가 있는 레이어
    private AudioSource MonsterAudio; // 보스 사운드
    private Animator animator; // 애니메이터
    private Canvas canvas; // 캔버스
    private bool isAlive = true; // 보스 생사 여부
    private MonsterMovement monsterMovement;
    private MonsterSpawner spawner;
    private List<GameObject> damageTextObjects = new List<GameObject>(); // 데미지 텍스트 리스트

    void Start()
    {
        healthSlider = FindObjectOfType<Slider>(); // 씬의 존재하는 슬라이더 찾아서 넣어주기
        monsterMovement = this.GetComponent<MonsterMovement>();

        if (healthSlider != null)
        {
            textHP = healthSlider.GetComponentInChildren<TextMeshProUGUI>(); // 체력바 하위의 TextMeshProUGUI 찾기
        }
        canvas = FindObjectOfType<Canvas>(); // Canvas 연결
        animator = GetComponent<Animator>(); // 애니메이터 연결

        MonsterAudio = GetComponent<AudioSource>(); // 오디오 소스 연결

        if (Clips.Length > 0 && MonsterAudio != null && Clips[0] != null) // 오디오 클립 존재 여부와 오디오 소스 체크
        {
            MonsterAudio.clip = Clips[0]; // 몬스터 소환 효과음
            MonsterAudio.Play();
        }

        UpdateHealthSlider();
        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);

        spawner = FindObjectOfType<MonsterSpawner>();
    }

    private void MonsterAttack() // 몬스터 공격 애니메이션 실행 및 플레이어에게 데미지 주기
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP && animator != null && isAlive)
        {
            animator.SetTrigger("Attack");
            
            if (Clips.Length > 1 && MonsterAudio != null && Clips[1] != null) // 오디오 클립 존재 여부와 오디오 소스 체크
            {
                MonsterAudio.clip = Clips[1]; // 몬스터 공격 효과음
                MonsterAudio.Play();
            }

            playerHP.TakeDamage_P(MonsterPower);
        }
    }

    public void TakeDamage_M(int damage) // 몬스터에게 데미지를 주는 함수
    {
        if (!isAlive) return; // 몬스터가 죽었다면 함수를 종료
        
        MonsterCurrentHealth -= damage;

        if (Clips.Length > 2 && MonsterAudio != null && Clips[2] != null) // 오디오 클립 존재 여부와 오디오 소스 체크
        {
            MonsterAudio.clip = Clips[2]; // 몬스터 피격 효과음
            MonsterAudio.Play();
        }

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

        if (Clips.Length > 3 && MonsterAudio != null && Clips[3] != null) // 오디오 클립 존재 여부와 오디오 소스 체크
        {
            MonsterAudio.clip = Clips[3]; // 몬스터 사망 효과음
            MonsterAudio.Play();
        }

        if(monsterMovement != null)
        {
            monsterMovement.StopMoving(); // Stop the movement
        }

        // 하위의 모든 콜라이더를 비활성화
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        StartCoroutine(Die());
    }

    // 몬스터 사망 처리
    IEnumerator Die()
    {
        // 5초 후에 Destroy 실행
        yield return new WaitForSeconds(5f);

        // 게임 오브젝트 삭제
        Destroy(gameObject);

        // 스포너의 MonsterDied 메서드 호출
        if (spawner != null)
        {
            spawner.MonsterDied();
        }
    }

    public void SetIsIdle()
    {
        animator.SetBool("IsIdle", true);
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
