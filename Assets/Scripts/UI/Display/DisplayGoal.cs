using UnityEngine;
using TMPro;

public class DisplayGoal : MonoBehaviour
{
    public int StageGoal_Kill;
    public int KillCount;
    private MonsterSpawner spawner;

    [SerializeField] private TextMeshProUGUI Goal_KillText;
    [SerializeField] private GameObject ClearPage;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        if (Goal_KillText == null)
        {
            Debug.LogError("Goal_KillText is not assigned.");
            return;
        }

        if (ClearPage == null)
        {
            Debug.LogError("ClearPage is not assigned.");
            return;
        }

        spawner = FindObjectOfType<MonsterSpawner>();

        if(spawner != null)
        {
            KillCount = spawner.currentKillCount;
            StageGoal_Kill = spawner.targetKillCount;
        }
        DisplayKillCount();
    }

    void Update()
    {
        int currentKillCount = spawner.currentKillCount;
        if (KillCount != currentKillCount)
        {
            KillCount = currentKillCount;
            DisplayKillCount();
        }
    }

    public void DisplayKillCount()
    {
        if (Goal_KillText != null)
        {
            Goal_KillText.text = KillCount + " / " + StageGoal_Kill;
        }
    }
}
