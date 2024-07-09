using UnityEngine;

public class HPUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseMaxHealth = 10;
    public DisplayPlayerHP displayPlayerHP;
    public GoldController goldController;

    void Start()
    {
        this.displayPlayerHP = GameObject.Find("HPText").GetComponent<DisplayPlayerHP>();
        this.goldController = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeHP);
    }

    void UpgradeHP()
    {
        if(goldController != null && this.goldController.PlayerGold >= this.GoldNeeds)
        {
            this.goldController.PlayerGold -= this.GoldNeeds;
            GameManager.Instance.SetGold(this.goldController.PlayerGold);
            this.goldController.UpdateGoldText();

            this.displayPlayerHP.PlayerHP += IncreaseMaxHealth;
            this.displayPlayerHP.UpdateHP(this.displayPlayerHP.PlayerHP);

            GameManager.Instance.SaveGameData();
            // GameManager.Instance.SetMaxHP(this.HP.playerMaxHealth); // 업그레이드 시 데이터 저장 (최대 체력)
        }
        else
        {
            return;
        }
    }
}
