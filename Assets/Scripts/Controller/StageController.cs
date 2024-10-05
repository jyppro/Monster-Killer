using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageController : MonoBehaviour
{
    public float stageTime; // 전체 스테이지 시간
    public float currentStageTime; // 현재 남은 스테이지 시간
    public float threeStarTime; // 3성 달성 시간
    public float twoStarTime; // 2성 달성 시간
    public int score; // 현재 스코어
    public int sumScore; // 총 스코어

    [SerializeField] private TextMeshProUGUI scoreText; // 스코어 표시 UI
    [SerializeField] private GameObject clearPage; // 클리어 페이지 UI
    [SerializeField] private Sprite[] starImages; // 별 이미지 배열
    [SerializeField] private Image starResult; // 결과 별 UI

    public MouseLook mouseLook; // 마우스 조작

    public static StageController Instance { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 스테이지 데이터 로드
        LoadStageData();
        clearPage.SetActive(false); // 클리어 페이지 비활성화
    }

    private void Update()
    {
        // Guardian 모드인 경우 시간 업데이트 하지 않음
        if (StageLoader.Instance.currentModeIndex != 2) 
        {
            UpdateCurrentStageTime();
        }
    }

    private void LoadStageData()
    {
        BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (stageData != null)
        {
            stageTime = stageData.stageTime;
            currentStageTime = stageData.stageTime;
            threeStarTime = stageData.threeStarTime;
            twoStarTime = stageData.twoStarTime;
            score = stageData.score;
        }
    }

    private void UpdateCurrentStageTime()
    {
        currentStageTime -= Time.deltaTime;
    }

    public void ShowClearPage()
    {
        if (clearPage != null)
        {
            UpdateScoreAndStarImage();
            DisplayScoreText();

            // 하이스코어 업데이트
            UpdateHighScore();

            // 스테이지 클리어 상태 업데이트
            GameManager.Instance.UnlockNextStage(StageLoader.Instance.currentModeIndex, StageLoader.Instance.currentStageIndex);
            mouseLook.UnlockMouse();

            clearPage.SetActive(true);
            Time.timeScale = 0.0f; // 게임 일시 정지
        }
    }

    private void UpdateScoreAndStarImage()
    {
        Sprite starImage = GetStarImage();
        score = Mathf.Max(Mathf.RoundToInt(score * (currentStageTime / stageTime)), 20); // 최소 스코어 20으로 설정
        starResult.sprite = starImage;
    }

    private Sprite GetStarImage()
    {
        if (currentStageTime >= threeStarTime)
        {
            return starImages[0]; // 3성
        }
        else if (currentStageTime >= twoStarTime)
        {
            return starImages[1]; // 2성
        }
        else
        {
            return starImages[2]; // 1성
        }
    }

    private void UpdateHighScore()
    {
        int modeIndex = StageLoader.Instance.currentModeIndex;
        int stageIndex = StageLoader.Instance.currentStageIndex;
        int previousHighScore = GameManager.Instance.GetHighScore(modeIndex, stageIndex);

        if (score > previousHighScore)
        {
            sumScore = GameManager.Instance.GetSumScore() + (score - previousHighScore);
            GameManager.Instance.SetSumScore(sumScore);
            GameManager.Instance.SetHighScore(modeIndex, stageIndex, score);
        }
    }

    public void DisplayScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}"; // 문자열 보간을 사용하여 가독성 향상
        }
    }
}
