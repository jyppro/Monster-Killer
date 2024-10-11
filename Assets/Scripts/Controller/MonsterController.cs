using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Slider stageBar; 
    [SerializeField] private TextMeshProUGUI healthText; 
    [SerializeField] private GameObject damageTextPrefab; 
    [SerializeField] private GameObject nextMonsterPrefab; 
    [SerializeField] private AudioClip[] audioClips; // 0: 소환, 1: 공격, 2: 피격, 3: 사망
    [SerializeField] private int goldReward = 10; 

    private List<GameObject> damageTextObjects = new List<GameObject>(); 
    private AudioSource monsterAudio;
    private Animator animator; 
    private Canvas canvas; 
    private bool isAlive = true; 
    private int maxHealth = 100; 
    private int currentHealth = 100; 
    private int attackPower = 5; 
    private int attackInterval = 5;

    private int attackPowerCopy;
    private Slider healthSliderCopy;
    private Slider stageBarCopy;
    private TextMeshProUGUI healthTextCopy;
    private GameObject damageTextPrefabCopy;
    private int goldRewardCopy;

    public int powerIncrease = 2;
    public int rewardIncrease = 2;
    public int healthIncrease = 5;

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

    private void InitializeComponents()
    {
        monsterMovement = GetComponent<MonsterMovement>();
        canvas = FindObjectOfType<Canvas>();
        animator = GetComponent<Animator>();
        monsterAudio = GetComponent<AudioSource>();
        monsterAudio.clip = audioClips[0]; 
        monsterAudio.Play();
    }

    private void InitializeMonsterValues()
    {
        attackPowerCopy = attackPower;
        healthSliderCopy = healthSlider;
        stageBarCopy = stageBar;
        healthTextCopy = healthText;
        damageTextPrefabCopy = damageTextPrefab;
        goldRewardCopy = goldReward;
    }

    private void AssignSpawner()
    {
        spawner = FindObjectOfType<MonsterSpawner>();
    }

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

    public void TakeDamage_M(int damage)
    {
        if (!isAlive) return;
        
        currentHealth -= damage;
        PlayAudioClip(2);

        if (currentHealth <= 0) 
        {
            Die();
        }
        UpdateHealthSlider();
    }

    private void Die()
    {
        isAlive = false;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        animator.SetBool("Death", true);
        PlayAudioClip(3);
        GameObject.Find("GoldText").GetComponent<DisplayGold>().GoldSum(goldReward);

        monsterMovement?.StopMoving();
        DisableColliders();

        // 몬스터가 죽은 후 3초 뒤에 다음 몬스터를 소환합니다.
        StartCoroutine(SpawnNextMonsterAfterDelay(3.0f));
    }

    private void DisableColliders()
    {
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }

    private IEnumerator SpawnNextMonsterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
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

    private void IncreaseStats()
    {
        maxHealth += healthIncrease;
        currentHealth = maxHealth;
        attackPower += powerIncrease;
        goldReward += rewardIncrease;
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        healthSlider.value = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 spawnAreaMin = new Vector2(40f, -20f);
        Vector2 spawnAreaMax = new Vector2(-10f, 30f);

        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomZ = Random.Range(spawnAreaMin.y, spawnAreaMax.y);

        return new Vector3(randomX, transform.position.y, randomZ);
    }

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

    private void ClearDamageTextObjects()
    {
        foreach (GameObject damageTextObject in damageTextObjects)
        {
            Destroy(damageTextObject);
        }
        damageTextObjects.Clear();
    }

    private void PlayAudioClip(int index)
    {
        // AudioSource가 활성화되어 있는지 확인
        if (monsterAudio != null && monsterAudio.isActiveAndEnabled)
        {
            monsterAudio.clip = audioClips[index];
            monsterAudio.Play();
        }
    }
}
