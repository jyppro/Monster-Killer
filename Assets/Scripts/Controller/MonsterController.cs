using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterController : MonoBehaviour
{
    // UI 및 사운드 관련 변수들
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Slider stageBar; 
    [SerializeField] private TextMeshProUGUI healthText; 
    [SerializeField] private GameObject damageTextPrefab; 
    [SerializeField] private GameObject nextMonsterPrefab; 
    [SerializeField] private AudioClip[] audioClips; // 0: 소환, 1: 공격, 2: 피격, 3: 사망
    [SerializeField] private int goldReward = 10; 

    // 내부 상태 및 설정 변수들
    private List<GameObject> damageTextObjects = new List<GameObject>(); 
    private AudioSource monsterAudio;
    private Animator animator; 
    private Canvas canvas; 
    private bool isAlive = true; 
    private int maxHealth = 100; 
    private int currentHealth = 100; 
    private int attackPower = 5; 
    private int attackInterval = 5;

    // 상태 복사본 저장 변수들
    private int attackPowerCopy;
    private Slider healthSliderCopy;
    private Slider stageBarCopy;
    private TextMeshProUGUI healthTextCopy;
    private GameObject damageTextPrefabCopy;
    private int goldRewardCopy;

    // 능력치 증가 설정
    public int powerIncrease = 2;
    public int rewardIncrease = 2;
    public int healthIncrease = 5;

    // 기타 참조 변수
    private MonsterSpawner spawner;
    private MonsterMovement monsterMovement;

    private void Start()
    {
        InitializeComponents();
        InitializeMonsterValues();
        UpdateHealthSlider();
        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);
        AssignSpawner();
    }

    // 컴포넌트 초기화
    private void InitializeComponents()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        canvas = FindObjectOfType<Canvas>();
        animator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        monsterAudio.clip = audioClips[0]; 
        monsterAudio.Play();
    }

    // 몬스터 능력치 및 복사본 초기화
    private void InitializeMonsterValues()
    {
        attackPowerCopy = attackPower;
        healthSliderCopy = healthSlider;
        stageBarCopy = stageBar;
        healthTextCopy = healthText;
        damageTextPrefabCopy = damageTextPrefab;
        goldRewardCopy = goldReward;
    }

    // 스포너 할당
    private void AssignSpawner()
    {
        spawner = FindObjectOfType<MonsterSpawner>();
    }

    // 몬스터 공격 로직
    private void MonsterAttack()
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP && animator != null && isAlive)
        {
            animator.SetTrigger("Attack");
            PlayAudioClip(1);
            playerHP.TakeDamage_P(attackPower);
        }
    }

    // 몬스터가 데미지를 받는 로직
    public void TakeDamage_M(int damage)
    {
        if (!isAlive) return;
        
        currentHealth -= damage;
        PlayAudioClip(2);

        if (currentHealth <= 0) 
        {
            Die();
            Invoke("SpawnNextMonster", 2.0f);
        }
        UpdateHealthSlider();
    }

    // 몬스터 사망 로직
    private void Die()
    {
        isAlive = false;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        animator.SetBool("Death", true);
        PlayAudioClip(3);
        GameObject.Find("GoldText").GetComponent<DisplayGold>().GoldSum(goldReward);

        monsterMovement?.StopMoving();
        DisableColliders();

        Destroy(gameObject, 3.0f); 
    }

    // 모든 자식 콜라이더 비활성화
    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    // 다음 몬스터 소환
    private void SpawnNextMonster()
    {
        if (FindObjectsOfType<MonsterController>().Length <= 1 && nextMonsterPrefab != null)
        {
            GameObject nextMonster = Instantiate(nextMonsterPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            MonsterController nextMonsterController = nextMonster.GetComponent<MonsterController>();
            SetNextMonsterValues(nextMonsterController);
        }

        ClearDamageTextObjects();
        stageBar.value += 0.33f;

        GameManager.Instance.ScoreCal();

        if (stageBarCopy.value >= 1.0f)
        {
            stageBarCopy.value = 0.0f;
        }
        Destroy(gameObject);
    }

    // 다음 몬스터 능력치 설정
    private void SetNextMonsterValues(MonsterController nextMonster)
    {
        nextMonster.attackPower = attackPowerCopy;
        nextMonster.healthSlider = healthSliderCopy;
        nextMonster.stageBar = stageBarCopy;
        nextMonster.healthText = healthTextCopy;
        nextMonster.damageTextPrefab = damageTextPrefabCopy;
        nextMonster.goldReward = goldRewardCopy;
        nextMonster.maxHealth = maxHealth;
        nextMonster.currentHealth = currentHealth;
        nextMonster.UpdateHealthSlider();
        nextMonster.IncreaseStats();
    }

    // 스탯 증가
    private void IncreaseStats()
    {
        maxHealth += healthIncrease;
        currentHealth = maxHealth;
        attackPower += powerIncrease;
        goldReward += rewardIncrease;
        UpdateHealthSlider();
    }

    // 체력바 업데이트
    private void UpdateHealthSlider()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    // 랜덤한 스폰 위치 계산
    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 spawnAreaMin = new Vector2(-80f, -75f);
        Vector2 spawnAreaMax = new Vector2(90f, 35f);

        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomZ = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        return new Vector3(randomX, transform.position.y, randomZ);
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

        Destroy(textComponent.gameObject);
    }

    // 데미지 텍스트 오브젝트 제거
    private void ClearDamageTextObjects()
    {
        foreach (GameObject damageTextObject in damageTextObjects)
        {
            Destroy(damageTextObject);
        }
        damageTextObjects.Clear();
    }

    // 오디오 클립 재생
    private void PlayAudioClip(int index)
    {
        monsterAudio.clip = audioClips[index];
        monsterAudio.Play();
    }
}
