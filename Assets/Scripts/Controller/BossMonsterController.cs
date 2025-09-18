using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossMonsterController : MonoBehaviour
{
    [Header("UI & Audio")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private AudioClip[] audioClips;

    [Header("Boss Stats")]
    [SerializeField] private int maxHealth = 1000;
    [SerializeField] private int attackPower = 50;
    [SerializeField] private float attackInterval = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2.0f; // 인스펙터에서 조정 가능
    [SerializeField] private float sizeOffset = 1.5f;

    private int currentHealth;
    private bool isAlive = true;

    private AudioSource monsterAudio;
    private Animator animator;
    private Canvas canvas;
    private Transform target;
    private MonsterMovement monsterMovement;
    private MonsterSpawner spawner;
    protected BossMonsterFSM fsm;
    [SerializeField] protected GameObject summonPrefab;  // 외부 접근은 막되 자식에서 사용 가능하도록
    public Transform player;

    private void Start()
    {
        InitializeComponents();

        if (monsterMovement != null)
            monsterMovement.SetStoppingDistance(attackRange);

        currentHealth = maxHealth;
        UpdateHealthSlider();

        fsm = gameObject.AddComponent<BossMonsterFSM>();
        InitializeFSM(); // ← virtual로 오버라이드 가능
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        isAlive = true;

        EnableColliders();
        UpdateHealthSlider();

        InitializeComponents();

        if (fsm == null)
            fsm = gameObject.AddComponent<BossMonsterFSM>();

        InitializeFSM(); // ← 오버라이드 가능

        animator?.SetBool("Death", false);
    }

    /// 상태 머신 초기화: 기본 Idle 상태로 시작. 서브클래스에서 오버라이드 가능.
    protected virtual void InitializeFSM()
    {
        fsm.InitializeFSM(this);
    }


    private void EnableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }


    private void InitializeComponents()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        animator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        canvas = FindObjectOfType<Canvas>();
        healthSlider = FindObjectOfType<Slider>();
        if (healthSlider != null) healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        spawner = FindObjectOfType<MonsterSpawner>();

        if (audioClips.Length > 0 && monsterAudio != null && audioClips[0] != null)
        {
            monsterAudio.clip = audioClips[0]; // 소환 음성
            monsterAudio.Play();
        }

        if (monsterMovement != null && target != null)
            monsterMovement.SetTarget(target); // ★ 타겟 지정
    }

    public void TakeDamage_M(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        PlayAudioClip(2); // 피격 사운드
        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
            fsm.ChangeState(new DieState(fsm, this));
        }
    }

    public void AttackPlayer()
    {
        if (!IsAlive || Target == null) return;

        //float distance = Vector3.Distance(transform.position, Target.position);
        //if (distance > attackRange + sizeOffset) return; // 너무 멀면 무효 공격

        animator.SetTrigger("Attack");
        PlayAudioClip(1); // 공격 사운드

        Debug.Log("Attack Play!");

        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP == null)
        {
            Debug.LogError("Target에 PlayerHP 컴포넌트가 없습니다!");
        }
        else
        {
            Debug.Log($"Attack Damage: {attackPower}");
            playerHP.TakeDamage_P(attackPower);
        }


        // if (playerHP != null)
        // {
        //     playerHP.TakeDamage_P(attackPower);
        //     Debug.Log("Attack Damage Apply!");
        // }
    }


    public void OnDeathFSM()
    {
        animator.SetBool("Death", true);
        PlayAudioClip(3);
        monsterMovement?.StopMoving();
        DisableColliders();
        StartCoroutine(Die());
    }

    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = false;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(5f);
        if (spawner != null)
        {
            spawner.MonsterDied(gameObject);
        }
        Destroy(gameObject);
    }

    public void ShowDamageText(float damage, Vector3 position)
    {
        Vector3 randomPos = position + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 2.0f), 0);
        GameObject damageObj = Instantiate(damageTextPrefab, randomPos, Quaternion.identity, canvas.transform);
        RectTransform rt = damageObj.GetComponent<RectTransform>();
        rt.position = Camera.main.WorldToScreenPoint(randomPos);
        TextMeshProUGUI text = damageObj.GetComponent<TextMeshProUGUI>();
        text.text = "-" + damage.ToString();
        StartCoroutine(AnimateDamageText(text));
    }

    private IEnumerator AnimateDamageText(TextMeshProUGUI text)
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 start = text.transform.position;
        Vector3 end = start + Vector3.up * 50f;
        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < duration)
        {
            if (text == null) yield break;
            float t = elapsed / duration;
            text.transform.position = Vector3.Lerp(start, end, t);
            text.color = Color.Lerp(startColor, endColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null || healthText == null) return;
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void PlayAudioClip(int index)
    {
        if (audioClips.Length > index && audioClips[index] != null && monsterAudio != null)
        {
            monsterAudio.clip = audioClips[index];
            monsterAudio.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 보스의 위치 기준
        Vector3 bossPosition = transform.position;

        // detectionRange: 파란색 원
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(bossPosition, detectionRange);

        // attackRange: 빨간색 원
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bossPosition, attackRange);
    }

    // FSM 접근용 프로퍼티들
    public Animator Animator => animator;
    public Transform Target => target;
    public MonsterMovement MonsterMovement => monsterMovement;
    public GameObject SummonPrefab => summonPrefab;
    public float AttackInterval => attackInterval;
    public bool IsAlive => isAlive;
    public float DetectionRange => detectionRange;
    public int CurrentHealth => currentHealth;
    public float AttackRange => attackRange;
    public float SizeOffset => sizeOffset;
    public int MaxHealth => maxHealth;
}
