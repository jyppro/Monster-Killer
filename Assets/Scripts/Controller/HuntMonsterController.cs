using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HuntMonsterController : MonoBehaviour, IPoolable, IMonsterDamageable
{
    // UI 및 사운드 관련 변수들
    //[SerializeField] private Slider healthSlider;
    //[SerializeField] private TextMeshProUGUI healthText;
    private MonsterHealthBar healthBarUI;

    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private AudioClip[] audioClips; // 0: 소환, 1: 공격, 2: 피격, 3: 사망
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int attackPower = 5;

    // 내부 상태 및 설정 변수들
    private AudioSource monsterAudio;
    private Animator animator;
    private Canvas canvas;
    private bool isAlive = true;
    private bool isInitialized = false;
    private int attackInterval = 5;
    private MonsterSpawner spawner;
    private MonsterMovement monsterMovement;
    private List<GameObject> damageTextObjects = new List<GameObject>();

    // private void Start()
    // {
    //     UpdateHealthSlider();
    //     InvokeRepeating("MonsterAttack", attackInterval, attackInterval);
    //     AssignSpawner();
    // }

    public void OnSpawned()
    {
        InitializeComponents();
        isAlive = true;
        currentHealth = maxHealth;
        EnableColliders();
        animator?.SetBool("Death", false);

        // 체력바 가져오기
        healthBarUI = HealthBarPoolManager.Instance?.Get();
        if (healthBarUI != null)
        {
            healthBarUI.Setup(this.transform);
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("HealthBar UI를 가져오지 못했습니다.");
        }

        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);
        AssignSpawner();
    }


    private void UpdateHealthUI()
    {
        healthBarUI?.UpdateBar(currentHealth, maxHealth);
    }

    private void OnDeath()
    {
        if (!isAlive) return;

        isAlive = false;
        currentHealth = 0;
        animator.SetBool("Death", true);
        monsterMovement?.StopMoving();
        DisableColliders();

        if (healthBarUI != null)
        {
            HealthBarPoolManager.Instance.Return(healthBarUI);
            healthBarUI = null;
        }

        // Start delayed despawn coroutine (MonsterDied도 포함됨)
        StartCoroutine(DelayedDespawn());
    }

    private IEnumerator DelayedDespawn()
    {
        yield return new WaitForSeconds(3.0f); // 사망 애니메이션 + 데미지 텍스트 표시 시간 확보

        spawner?.MonsterDied(gameObject); // 이 시점에 반환 처리
        gameObject.SetActive(false);      // 풀 방식 사용 시
    }


    public void OnDespawned()
    {
        // 필요에 따라 이펙트 정리, 이동 멈춤 등 처리
        monsterMovement?.StopMoving();
    }

    private void EnableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = true;
    }



    // 컴포넌트 초기화
    private void InitializeComponents()
    {
        if (isInitialized) return;

        monsterMovement = GetComponent<MonsterMovement>();
        // healthSlider = FindObjectOfType<Slider>();
        // if (healthSlider != null) healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();

        // 변경된 초기화 코드
        //healthSlider = GetComponentInChildren<Slider>();
        //healthText = GetComponentInChildren<TextMeshProUGUI>();
        canvas = FindObjectOfType<Canvas>();
        animator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        monsterAudio.clip = audioClips[0]; // 소환 효과음
        monsterAudio.Play();

        AssignSpawner();

        isInitialized = true;
    }

    // 몬스터 공격 로직
    private void MonsterAttack()
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP && animator != null && isAlive)
        {
            animator.SetTrigger("Attack");
            PlayAudioClip(1); // 몬스터 공격 효과음
            playerHP.TakeDamage_P(attackPower);
        }
    }

    // 몬스터가 데미지를 받는 로직
    // public void TakeDamage_M(int damage)
    // {
    //     Debug.LogWarning("몬스터가 피격되었습니다.");
    //     if (!isAlive) return;

    //     currentHealth -= damage;
    //     PlayAudioClip(2); // 몬스터 피격 효과음

    //     if (currentHealth <= 0)
    //     {
    //         OnDeath();
    //         Debug.LogWarning("몬스터가 죽었습니다.");
    //     }
    //     //UpdateHealthSlider();
    //     UpdateHealthUI();      // 머리 위 체력바 (MonsterHealthBar)
    // }

    public void TakeDamage_M(int damage)
    {
        // 실제 데미지 처리
        currentHealth -= damage;
        CheckDeath();
    }
    private void CheckDeath()
    {
        if (!isAlive) return;

        if (currentHealth <= 0)
        {
            PlayAudioClip(3); // 사망 효과음
            OnDeath();         // 사망 처리
        }
        else
        {
            PlayAudioClip(2); // 피격 효과음
            UpdateHealthUI(); // 체력바 갱신
        }
    }

    // private void OnDeath()
    // {
    //     isAlive = false;
    //     currentHealth = 0;
    //     animator.SetBool("Death", true);
    //     PlayAudioClip(3); // 몬스터 사망 효과음

    //     monsterMovement?.StopMoving();
    //     DisableColliders();

    //     Destroy(gameObject, 3.0f); // 3초 후에 몬스터 삭제

    //     if (spawner != null)
    //     {
    //         spawner.MonsterDied(gameObject); // 몬스터 오브젝트 전달
    //     }
    // }


    // 모든 자식 콜라이더 비활성화
    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    // 체력바 업데이트
    // private void UpdateHealthSlider()
    // {
    //     healthSlider.value = (float)currentHealth / maxHealth;
    //     healthText.text = $"{currentHealth} / {maxHealth}";
    // }

    // 데미지 텍스트 생성 및 애니메이션 적용
    // public void ShowDamageText(float damage, Vector3 position)
    // {
    //     Vector3 randomPosition = position + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 2.0f), 0);
    //     GameObject damageTextObject = Instantiate(damageTextPrefab, randomPosition, Quaternion.identity, canvas.transform);
    //     RectTransform rt = damageTextObject.GetComponent<RectTransform>();
    //     rt.position = Camera.main.WorldToScreenPoint(randomPosition);

    //     TextMeshProUGUI textComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
    //     textComponent.text = "-" + damage.ToString();

    //     damageTextObjects.Add(damageTextObject);

    //     StartCoroutine(AnimateDamageText(textComponent));
    // }

    public void ShowDamageText(int damage, Vector3 position)
    {
        // 데미지 텍스트 출력
        // DamageTextSpawner.Instance.Spawn(damage, position);

        Vector3 randomPosition = position + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(1.0f, 2.0f), 0);
        GameObject damageTextObject = Instantiate(damageTextPrefab, randomPosition, Quaternion.identity, canvas.transform);
        RectTransform rt = damageTextObject.GetComponent<RectTransform>();
        rt.position = Camera.main.WorldToScreenPoint(randomPosition);

        TextMeshProUGUI textComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = "-" + damage.ToString();

        damageTextObjects.Add(damageTextObject);

        StartCoroutine(AnimateDamageText(textComponent));
    }

    // 데미지 텍스트 애니메이션 코루틴
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

    // 오디오 클립 재생
    private void PlayAudioClip(int index)
    {
        // AudioSource가 활성화되어 있는지 확인
        if (monsterAudio != null && monsterAudio.isActiveAndEnabled)
        {
            monsterAudio.clip = audioClips[index];
            monsterAudio.Play();
        }
    }

    // 스포너 할당
    private void AssignSpawner()
    {
        spawner = FindObjectOfType<MonsterSpawner>();
    }
}
