using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HuntMonsterController : MonoBehaviour
{
    // UI 및 사운드 관련 변수들
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
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
    private int attackInterval = 5;
    private MonsterSpawner spawner;
    private MonsterMovement monsterMovement;
    private List<GameObject> damageTextObjects = new List<GameObject>();

    private void Start()
    {
        InitializeComponents();
        UpdateHealthSlider();
        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);
        AssignSpawner();
    }

    // 컴포넌트 초기화
    private void InitializeComponents()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        healthSlider = FindObjectOfType<Slider>();
        if (healthSlider != null) healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>(); // 체력바 하위의 TextMeshProUGUI 찾기
        canvas = FindObjectOfType<Canvas>();
        animator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        monsterAudio.clip = audioClips[0]; // 몬스터 소환 효과음
        monsterAudio.Play();
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
    public void TakeDamage_M(int damage)
    {
        if (!isAlive) return;

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

        monsterMovement?.StopMoving();
        DisableColliders();

        Destroy(gameObject, 3.0f); // 3초 후에 몬스터 삭제

        if (spawner != null)
        {
            spawner.MonsterDied(gameObject); // 몬스터 오브젝트 전달
        }
    }


    // 모든 자식 콜라이더 비활성화
    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    // 체력바 업데이트
    private void UpdateHealthSlider()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // 데미지 텍스트 생성 및 애니메이션 적용
    public void ShowDamageText(float damage, Vector3 position)
    {
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
