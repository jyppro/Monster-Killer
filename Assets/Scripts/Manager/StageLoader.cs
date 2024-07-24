using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance;

    public int currentModeIndex;
    public int currentStageIndex;

    private string[] modeSceneNames = { "HuntStageScene", "BossStageScene", "GuardianStageScene" };

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
        return modeSceneNames[currentModeIndex];
    }
}
