using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance;

    // 변수명을 다시 원래대로 변경하여 외부에서 참조할 수 있도록 수정
    public int currentModeIndex;
    public int currentStageIndex;

    private static readonly string[] ModeSceneNames = { "HuntStageScene", "BossStageScene", "GuardianStageScene" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentStage(int modeIndex, int stageIndex)
    {
        currentModeIndex = modeIndex;
        currentStageIndex = stageIndex;
    }

    public BaseStageData LoadStageData()
    {
        return GameManager.Instance.GetStageData(currentModeIndex, currentStageIndex);
    }

    public string GetCurrentSceneName()
    {
        if (currentModeIndex >= 0 && currentModeIndex < ModeSceneNames.Length)
        {
            return ModeSceneNames[currentModeIndex];
        }
        Debug.LogError("Invalid mode index: " + currentModeIndex);
        return string.Empty;
    }
}
