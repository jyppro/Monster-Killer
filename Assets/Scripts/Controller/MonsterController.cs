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
    [SerializeField] public float goldReward = 10f; // 몬스터 잡을 때 획득 가능한 골드량
    private List<GameObject> damageTextObjects = new List<GameObject>(); // 데미지 텍스트 리스트
    private AudioSource MonsterAudio; // 몬스터 사운드
    private Animator animator; // 애니메이터
    private Canvas canvas; // 캔버스
    private bool isAlive = true; // 몬스터 생사 여부
    private float MonsterMaxHealth = 100f; // 몬스터 최대 체력
    private float MonsterCurrentHealth = 100f; // 몬스터 현재 체력
    private float MonsterPower = 5f; // 몬스터 공격력
    private float attackInterval = 5f; // 공격 간격

    private float MonsterPowerCopy;
    private Slider healthSliderCopy;
    private Slider StageBarCopy;
    private TextMeshProUGUI textHPCopy;
    private GameObject damageTextPrefabCopy;
    public float goldRewardCopy;
    public int stage;

    void Start()
    {
        canvas = FindObjectOfType<Canvas>(); // Canvas 연결
        animator = GetComponent<Animator>(); // 애니메이터 연결
        MonsterAudio = GetComponent<AudioSource>(); // 오디오 소스 연결
        MonsterAudio.clip = Clips[0]; // 몬스터 소환 효과음
        MonsterAudio.Play();
        stage = GameManager.Instance.GetStage();

        // 복사본 저장
        MonsterPowerCopy = this.MonsterPower;
        healthSliderCopy = this.healthSlider;
        StageBarCopy = this.StageBar;
        textHPCopy = this.textHP;
        damageTextPrefabCopy = this.damageTextPrefab;
        goldRewardCopy = this.goldReward;

        UpdateHealthSlider();
        InvokeRepeating("MonsterAttack", attackInterval, attackInterval);
    }

    private void MonsterAttack() // 몬스터 공격 애니메이션 실행 및 플레이어에게 데미지 주기
    {
        PlayerHP playerHP = FindObjectOfType<PlayerHP>();
        if (playerHP && animator != null && isAlive == true)
        {
            animator.SetTrigger("Attack");
            MonsterAudio.clip = Clips[1]; // 몬스터 공격 효과음
            MonsterAudio.Play();
            playerHP.TakeDamage_P(MonsterPower);
        }
    }

    public void TakeDamage_M(float damage) // 몬스터에게 데미지를 주는 함수
    {
        MonsterCurrentHealth -= damage;
        MonsterAudio.clip = Clips[2]; // 몬스터 피격 효과음
        MonsterAudio.Play();

        // 체력이 전부 소진되면 다음 몬스터 소환
        if (MonsterCurrentHealth <= 0f)
        {
            isAlive = false;
            MonsterCurrentHealth = MonsterMaxHealth;
            animator.SetBool("Death", true);
            gameObject.GetComponent<MonsterMovement>().enabled = false;
            GameObject.Find("GoldText").GetComponent<GoldController>().GoldSum(goldReward); // 수정 필요 -> 골드 데이터 값으로
            MonsterAudio.clip = Clips[3]; // 몬스터 사망 효과음
            MonsterAudio.Play();
            Invoke("SpawnNextMonster", 3f);
        }
        UpdateHealthSlider();
    }

    private void SpawnNextMonster() // 다음 몬스터를 소환하는 함수
    {
        if (nextMonsterPrefab != null)
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
            nextMonsterController.MonsterPower += 2f;
            nextMonsterController.goldReward += 2;
        }
         // 이전 몬스터의 데미지 텍스트 UI 오브젝트 제거
        foreach (GameObject damageTextObject in damageTextObjects) { Destroy(damageTextObject); }
        damageTextObjects.Clear(); // 리스트 초기화
        StageBar.value += 0.33f;
        GameManager.Instance.SetStage(stage + 1);
        GameManager.Instance.SaveGameData();
        if(StageBarCopy.value >= 1f) { StageBarCopy.value = 0f; }
        Destroy(gameObject);
    }

    private void UpdateHealthSlider() // 체력바 업데이트
    {
        // 체력바 슬라이더의 값을 현재 체력 비율로 설정
        float decreaseHp = MonsterCurrentHealth / MonsterMaxHealth;
        healthSlider.value = decreaseHp;
        textHP.text = $"{MonsterCurrentHealth} / {MonsterMaxHealth}";
    }

    private void IncreaseHealth() // 다음몬스터 소환 시 체력 증가
    {
        MonsterMaxHealth = (MonsterMaxHealth + 5f); // 임의로 설정한 수치
        if (MonsterCurrentHealth != MonsterMaxHealth) { MonsterCurrentHealth = MonsterMaxHealth; }
        UpdateHealthSlider();
    }

    public void ShowDamageText(float damage, Vector3 position) // 몬스터가 받는 데미지 보여주기
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, position, Quaternion.identity);
        TextMeshProUGUI textComponent = damageTextObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = "-" + damage.ToString();
        damageTextObject.transform.SetParent(canvas.transform, false);
        damageTextObjects.Add(damageTextObject); // 리스트에 추가
        StartCoroutine(AnimateDamageText(textComponent));
    }


    private IEnumerator AnimateDamageText(TextMeshProUGUI textComponent) // 데미지에 애니메이션 적용
    {
        float duration = 1.5f; // 애니메이션 지속 시간
        float elapsedTime = 0f;

        Vector3 startPosition = textComponent.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 50f; // 위로 올라갈 위치
        Color startColor = textComponent.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f); // 투명도 조정

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            textComponent.transform.position = Vector3.Lerp(startPosition, endPosition, progress); // 위치 이동
            textComponent.color = Color.Lerp(startColor, endColor, progress); // 투명도 조정
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(textComponent.gameObject); // 애니메이션 종료 후 텍스트 오브젝트 제거
    }
}
