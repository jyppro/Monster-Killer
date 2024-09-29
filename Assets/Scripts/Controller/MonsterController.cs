using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider; // 몬스터 체력바 UI
    [SerializeField] private Slider StageBar; // 스테이지 UI
    [SerializeField] private TextMeshProUGUI textHP; // 체력바 텍스트 UI
    [SerializeField] private GameObject damageTextPrefab; // 데미지 텍스트 UI
    [SerializeField] private GameObject nextMonsterPrefab; // 다음 몬스터로 전환할 때 사용할 프리팹
    [SerializeField] private AudioClip[] Clips; // 사용할 오디오 클립의 배열 <0 : 소환, 1 : 공격, 2 : 피격, 3 : 사망>
    [SerializeField] public int goldReward = 10; // 몬스터 잡을 때 획득 가능한 골드량
    private List<GameObject> damageTextObjects = new List<GameObject>(); // 데미지 텍스트 리스트
    private AudioSource MonsterAudio; // 몬스터 사운드
    private Animator animator; // 애니메이터
    private Canvas canvas; // 캔버스
    private bool isAlive = true; // 몬스터 생사 여부
    private int MonsterMaxHealth = 100; // 몬스터 최대 체력
    private int MonsterCurrentHealth = 100; // 몬스터 현재 체력
    private int MonsterPower = 5; // 몬스터 공격력
    private int attackInterval = 5; // 공격 간격

    private int MonsterPowerCopy;
    private Slider healthSliderCopy;
    private Slider StageBarCopy;
    private TextMeshProUGUI textHPCopy;
    private GameObject damageTextPrefabCopy;
    private int goldRewardCopy;
    // public int stage;
    public int IncreasePower = 2;
    public int IncreaseReward = 2;
    public int IncreaseMonsterMaxHealth = 5;
    private MonsterSpawner spawner;
    private MonsterMovement monsterMovement;

    void Start()
    {
        monsterMovement = this.GetComponent<MonsterMovement>();
        canvas = FindObjectOfType<Canvas>(); // Canvas 연결
        animator = GetComponent<Animator>(); // 애니메이터 연결
        MonsterAudio = GetComponent<AudioSource>(); // 오디오 소스 연결
        MonsterAudio.clip = Clips[0]; // 몬스터 소환 효과음
        MonsterAudio.Play();

        // 복사본 저장
        MonsterPowerCopy = this.MonsterPower;
        healthSliderCopy = this.healthSlider;
        StageBarCopy = this.StageBar;
        textHPCopy = this.textHP;
        damageTextPrefabCopy = this.damageTextPrefab;
        goldRewardCopy = this.goldReward;

        UpdateHealthSlider();
        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);

        if(spawner != null)
        {
            spawner = FindObjectOfType<MonsterSpawner>();
        }
        else
        {
            return;
        }
    }

    private void MonsterAttack() // 몬스터 공격 애니메이션 실행 및 플레이어에게 데미지 주기
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP && animator != null && isAlive == true)
        {
            animator.SetTrigger("Attack");
            MonsterAudio.clip = Clips[1]; // 몬스터 공격 효과음
            MonsterAudio.Play();
            // ApplyDamageToPlayer(MonsterPower);
            playerHP.TakeDamage_P(MonsterPower);
        }
    }

    // public void ApplyDamageToPlayer(float monsterPower) // 몬스터가 플레이어에게 주는 데미지
    // {
    //     playerCurrentHealth -= monsterPower;
    //     playerCurrentHealth = Mathf.Clamp(playerCurrentHealth, 0f, playerMaxHealth); // 체력이 음수가 되지 않도록 클램핑
    //     UpdateHealthSlider();
    //     if (playerCurrentHealth <= 0.0f)
    //     {
    //          GameOver.SetActive(true);
    //          Time.timeScale = 0.0f;
    //     }
    // }

    public void TakeDamage_M(int damage) // 몬스터에게 데미지를 주는 함수
    {
        if (!isAlive) return; // 몬스터가 죽었다면 함수를 종료
        
        MonsterCurrentHealth -= damage;
        MonsterAudio.clip = Clips[2]; // 몬스터 피격 효과음
        MonsterAudio.Play();

        // 체력이 전부 소진되면 다음 몬스터 소환
        if (MonsterCurrentHealth <= 0)
        {
            OnDeath();
            Invoke("SpawnNextMonster", 2.0f);
        }
        UpdateHealthSlider();
    }

    private void OnDeath()
    {
        isAlive = false;
        if(MonsterCurrentHealth <= 0)
        {
            MonsterCurrentHealth = 0;
        }
        animator.SetBool("Death", true);

        MonsterAudio.clip = Clips[3]; // 몬스터 사망 효과음
        MonsterAudio.Play();

        // GameObject.Find("GoldController").GetComponent<GoldController>().GoldSum(goldReward);
        GameObject.Find("GoldText").GetComponent<DisplayGold>().GoldSum(goldReward);
        // GameManager.Instance.SaveGameData(); // 게임 데이터 저장

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

        // 3초 뒤에 몬스터 제거
        Destroy(gameObject, 3.0f);
    }

    private void SpawnNextMonster() // 다음 몬스터를 소환하는 함수
    {
        // 씬에 현재 존재하는 몬스터의 수를 카운트
        MonsterController[] existingMonsters = FindObjectsOfType<MonsterController>();
        int monsterCount = existingMonsters.Length;

        // 몬스터가 1마리 이하로 남아있으면 새로운 몬스터를 소환
        if (monsterCount <= 1)
        {
            if (nextMonsterPrefab != null) // 다음 몬스터 프리팹이 세팅되어 있다면
            {
                ResetMonster(); // 넘겨줄 몬스터 데이터 세팅
            }
        }

        // 이전 몬스터의 데미지 텍스트 UI 오브젝트 제거
        foreach (GameObject damageTextObject in damageTextObjects)
        {
            Destroy(damageTextObject);
        }
        damageTextObjects.Clear(); // 리스트 초기화
        StageBar.value += 0.33f;
        // GameManager.Instance.SaveGameData();
        GameManager.Instance.ScoreCal();

        if (StageBarCopy.value >= 1.0f)
        {
            StageBarCopy.value = 0.0f;
        }
        Destroy(gameObject);
    }

    private void ResetMonster() // 몬스터 초기화
    {
        GameObject nextMonster = Instantiate(nextMonsterPrefab, transform.position, transform.rotation);
        MonsterController nextMonsterController = nextMonster.GetComponent<MonsterController>();
        nextMonsterController.MonsterPower = MonsterPowerCopy;
        nextMonsterController.healthSlider = healthSliderCopy;
        nextMonsterController.StageBar = StageBarCopy;
        nextMonsterController.textHP = textHPCopy;
        nextMonsterController.damageTextPrefab = damageTextPrefabCopy;
        nextMonsterController.goldReward = goldRewardCopy;
        nextMonsterController.MonsterMaxHealth = MonsterMaxHealth;
        nextMonsterController.MonsterCurrentHealth = MonsterCurrentHealth;
        nextMonsterController.UpdateHealthSlider();
        nextMonsterController.IncreaseHealth();
        nextMonsterController.MonsterPower += IncreasePower;
        nextMonsterController.goldReward += IncreaseReward;
    }

    private void IncreaseHealth() // 다음몬스터 소환 시 체력 증가
    {
        MonsterMaxHealth += IncreaseMonsterMaxHealth;
        if (MonsterCurrentHealth != MonsterMaxHealth) { MonsterCurrentHealth = MonsterMaxHealth; }
        UpdateHealthSlider();
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