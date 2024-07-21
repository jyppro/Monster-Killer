using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // 몬스터가 소환될 위치들
    public float spawnInterval = 3.0f; // 몬스터가 소환되는 주기
    public int numberOfMonsters = 2; // 한 번에 소환할 몬스터의 수

    private GameObject[] monsterPrefabs; // 소환할 몬스터의 프리팹들
    private int maxMonster = 20; // 최대 소환할 몬스터 수
    private int currentMonsterCount = 0; // 현재 소환된 몬스터 수
    public int targetKillCount = 0; // 목표 처치 마리 수 -> 스테이지 데이터에서 가져와 세팅 (ex : 20)
    public int currentKillCount = 0; // 현재 처치 마리 수

    public float stageTime; // 스테이지 진행시간
    public float CurrentStageTime; // 스테이지 진행시간
    public float threeStarTime; // 별 3개로 클리어할 수 있는 최소시간
    public float twoStarTime; // 별 2개로 클리어할 수 있는 최소시간
    public int score; // 점수
    public int sumScore; // 총 점수

    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject ClearPage;
    [SerializeField] private Sprite[] starImages; // 별 이미지 배열 (3, 2, 1 스타)
    [SerializeField] private Image starResult;

    void Start()
    {
        // 현재 스테이지의 데이터를 받아 설정
        StageData stageData = GameManager.Instance.GetStageData(StageLoader.Instance.currentStageIndex);
        if (stageData != null)
        {
            monsterPrefabs = stageData.monsterPrefabs; // 몬스터 세팅
            targetKillCount = stageData.targetKillCount; // 목표 처치 마리 수 세팅

            stageTime = stageData.stageTime;
            CurrentStageTime = stageData.stageTime;

            threeStarTime = stageData.threeStarTime;
            twoStarTime = stageData.twoStarTime;

            score = stageData.score;
        }

        StartCoroutine(SpawnMonsters());
    }

    private void Update()
    {
        CurrentStageTime -= Time.deltaTime; // 스테이지 진행 시간 감소
    }

    IEnumerator SpawnMonsters()
    {
        while (true)
        {
            if (currentMonsterCount < maxMonster) // 소환된 몬스터 수가 최대 마리수보다 적으면 계속 소환
            {
                for (int i = 0; i < numberOfMonsters && currentMonsterCount < maxMonster; i++)
                {
                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];
                    Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
                    currentMonsterCount++;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 몬스터가 죽었을 때 호출
    public void MonsterDied()
    {
        currentMonsterCount--;
        currentKillCount++;

        if(currentKillCount >= maxMonster)
        {
            currentKillCount = maxMonster;
            ShowClearPage();
        }
    }

    // 클리어 페이지 띄우기
    private void ShowClearPage()
    {
        if (ClearPage != null)
        {
            Sprite starImage = null;

            if(CurrentStageTime >= threeStarTime) // 별 3개 클리어
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime)); // 남은 시간 기준으로 점수 계산
                starImage = starImages[0];
            }
            else if(CurrentStageTime >= twoStarTime) // 별 2개 클리어
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime));
                starImage = starImages[1];
            }
            else // 별 1개 클리어
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime));
                starImage = starImages[2];
            }
            starResult.sprite = starImage; // Image의 sprite 속성 설정

            if(score <= 20) // 기본점수 20점
            {
                score = 20;
            }
            DisplayScoreText();

            sumScore = GameManager.Instance.GetSumScore(); // 총 점수 가져오기
            sumScore += score; // 점수 합하기

            ClearPage.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    public void DisplayScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score : " + score;
        }
    }
}
