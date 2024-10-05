using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenseMonsterController : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider healthSlider; // 몬스터 체력바 UI
    [SerializeField] private TextMeshProUGUI healthText; // 체력바 텍스트 UI
    [SerializeField] private GameObject damageTextPrefab; // 데미지 텍스트 UI

    [Header("Monster Attributes")]
    [SerializeField] private int maxHealth = 100; // 몬스터 최대 체력
    [SerializeField] private int currentHealth; // 몬스터 현재 체력
    [SerializeField] private int attackPower = 5; // 몬스터 공격력

    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] audioClips; // 사용할 오디오 클립의 배열 <0 : 소환, 1 : 공격, 2 : 피격, 3 : 사망>

    private AudioSource monsterAudio; // 몬스터 사운드
    private Animator animator; // 애니메이터
    private Canvas canvas; // 캔버스
    private MonsterSpawner spawner;

    private bool isAlive = true; // 몬스터 생사 여부
    private bool canAttack = true; // 공격 가능 여부
    private Transform target; // 현재 타겟
    [SerializeField] private float attackDistance = 1.5f; // 공격할 거리
    [SerializeField] private float attackCooldown = 2f; // 공격 쿨다운 시간

    private DefenseMonsterMovement defenseMonsterMovement;
    private List<GameObject> damageTextObjects = new List<GameObject>(); // 데미지 텍스트 리스트

    void Start()
    {
        InitializeComponents();
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
        PlayAudioClip(0); // 몬스터 소환 효과음
        UpdateHealthSlider();
        AssignSpawner();
    }

    void Update()
    {
        if (target != null && isAlive)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackDistance && canAttack)
            {
                StartCoroutine(HandleAttack());
            }
        }
    }

    public void SetTarget(Transform newTarget) => target = newTarget;

    private void InitializeComponents()
    {
        healthSlider = FindObjectOfType<Slider>(); // 씬의 존재하는 슬라이더 찾아서 넣어주기
        defenseMonsterMovement = GetComponent<DefenseMonsterMovement>();
        canvas = FindObjectOfType<Canvas>(); // Canvas 연결
        animator = GetComponent<Animator>(); // 애니메이터 연결
        monsterAudio = GetComponent<AudioSource>(); // 오디오 소스 연결

        if (healthSlider != null)
        {
            healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>(); // 체력바 하위의 TextMeshProUGUI 찾기
        }
    }

    private IEnumerator HandleAttack()
    {
        if (canAttack)
        {
            MonsterAttack();
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void MonsterAttack()
    {
        canAttack = false; // 공격 불가 상태로 설정
        animator.SetTrigger("Attack");
        PlayAudioClip(1); // 몬스터 공격 효과음

        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP != null)
        {
            playerHP.TakeDamage_P(attackPower); // 타겟에 데미지 적용
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

        currentHealth -= damage;
        PlayAudioClip(2); // 몬스터 피격 효과음

        if (currentHealth <= 0)
        {
            OnDeath();
        }
        UpdateHealthSlider();
    }

    private void OnDeath()
    {
        isAlive = false;
        currentHealth = 0;
        animator.SetBool("Death", true);
        PlayAudioClip(3); // 몬스터 사망 효과음

        if (defenseMonsterMovement != null)
        {
            defenseMonsterMovement.StopMoving(); // 움직임 중지
        }

        DisableColliders();
        StartCoroutine(Die()); // 사망 후 처리
    }

    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    private void UpdateHealthSlider() // 체력바 업데이트
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void ShowDamageText(float damage, Vector3 position) // 몬스터가 받는 데미지 보여주기
    {
        Vector3 randomPosition = GenerateRandomOffsetPosition(position);
        GameObject damageTextObject = Instantiate(damageTextPrefab, randomPosition, Quaternion.identity, canvas.transform);
        SetDamageTextPosition(damageTextObject, randomPosition);
        SetDamageTextValue(damageTextObject, damage);

        damageTextObjects.Add(damageTextObject); // 리스트에 추가
        StartCoroutine(AnimateDamageText(damageTextObject.GetComponent<TextMeshProUGUI>()));
    }

    private Vector3 GenerateRandomOffsetPosition(Vector3 basePosition)
    {
        float randomOffsetX = Random.Range(-2.0f, 2.0f);
        float randomOffsetY = Random.Range(1.0f, 2.0f);
        return basePosition + new Vector3(randomOffsetX, randomOffsetY, 0);
    }

    private void SetDamageTextPosition(GameObject damageTextObject, Vector3 position)
    {
        RectTransform rt = damageTextObject.GetComponent<RectTransform>();
        rt.position = Camera.main.WorldToScreenPoint(position);
    }

    private void SetDamageTextValue(GameObject damageTextObject, float damage)
    {
        TextMeshProUGUI textComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = "-" + damage.ToString();
    }

    private IEnumerator AnimateDamageText(TextMeshProUGUI textComponent)
    {
        float duration = 1.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = textComponent.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 50.0f;
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < duration)
        {
            if (textComponent == null) yield break;

            float progress = elapsedTime / duration;
            textComponent.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            textComponent.color = Color.Lerp(startColor, endColor, progress);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void PlayAudioClip(int index)
    {
        if (audioClips.Length > index && monsterAudio != null && audioClips[index] != null)
        {
            monsterAudio.clip = audioClips[index];
            monsterAudio.Play();
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(3f); // 3초 후에 제거
        Destroy(gameObject);
        spawner?.MonsterDied(); // 스포너의 MonsterDied 메서드 호출
    }

    private void AssignSpawner()
    {
        spawner = FindObjectOfType<MonsterSpawner>();
    }
}
