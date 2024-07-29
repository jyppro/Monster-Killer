using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageController : MonoBehaviour
{
    public float stageTime;
    public float CurrentStageTime;
    public float threeStarTime;
    public float twoStarTime;
    public int score;
    public int sumScore;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject ClearPage;
    [SerializeField] private Sprite[] starImages;
    [SerializeField] private Image starResult;

    public static StageController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        BaseStageData stageData = StageLoader.Instance.LoadStageData();
        if (stageData != null)
        {
            stageTime = stageData.stageTime;
            CurrentStageTime = stageData.stageTime;

            threeStarTime = stageData.threeStarTime;
            twoStarTime = stageData.twoStarTime;

            score = stageData.score;
        }

        ClearPage.SetActive(false);
    }

    private void Update()
    {
        CurrentStageTime -= Time.deltaTime;
    }

    public void ShowClearPage()
    {
        if (ClearPage != null)
        {
            Sprite starImage = null;

            if (CurrentStageTime >= threeStarTime)
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime));
                starImage = starImages[0];
            }
            else if (CurrentStageTime >= twoStarTime)
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime));
                starImage = starImages[1];
            }
            else
            {
                score = Mathf.RoundToInt(score * (CurrentStageTime / stageTime));
                starImage = starImages[2];
            }
            starResult.sprite = starImage;

            if (score <= 20)
            {
                score = 20;
            }
            DisplayScoreText();

            int modeIndex = StageLoader.Instance.currentModeIndex;
            int stageIndex = StageLoader.Instance.currentStageIndex;
            int previousHighScore = GameManager.Instance.GetHighScore(modeIndex, stageIndex);

            if (score > previousHighScore)
            {
                sumScore = GameManager.Instance.GetSumScore();
                sumScore += (score - previousHighScore);
                GameManager.Instance.SetSumScore(sumScore);
                GameManager.Instance.SetHighScore(modeIndex, stageIndex, score);
            }

            // 스테이지 클리어 상태 업데이트
            GameManager.Instance.UnlockNextStage(modeIndex, stageIndex);

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
