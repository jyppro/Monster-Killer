using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public static StageLoader Instance;
    public int currentStageIndex;

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

    public void LoadStageData(int stageIndex)
    {
        currentStageIndex = stageIndex;
    }
}
