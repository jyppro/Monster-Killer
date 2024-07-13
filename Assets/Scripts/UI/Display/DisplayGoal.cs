using UnityEngine;
using TMPro;

public class DisplayGoal : MonoBehaviour
{
    public int StageGoal_Kill;

    [SerializeField] private TextMeshProUGUI Goal_KillText;

    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is not found.");
            return;
        }

        // PlayerHP = GameManager.Instance.GetMaxHP();

        if (Goal_KillText == null)
        {
            Debug.LogError("TimeText is not assigned.");
            return;
        }

        // DisplayHP();
    }

    // void Update()
    // {
    //     // int currentHP = GameManager.Instance.GetMaxHP();
    //     if (PlayerHP != currentHP)
    //     {
    //         PlayerHP = currentHP;
    //         DisplayHP();
    //     }
    // }

    // public void DisplayHP()
    // {
    //     if (HPText != null)
    //     {
    //         HPText.text = "MaxHP : " + PlayerHP;
    //     }
    // }

    // public void UpdateHP(int newHP)
    // {
    //     PlayerHP = newHP;
    //     GameManager.Instance.SetMaxHP(PlayerHP);
    //     DisplayHP();
    // }
}
