using UnityEngine;

public class HPUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseMaxHealth = 10;
    public DisplayPlayerHP displayPlayerHP;
    public DisplayGold displayGold;

    void Start()
    {
        this.displayPlayerHP = GameObject.Find("HPText").GetComponent<DisplayPlayerHP>();
        this.displayGold = GameObject.Find("GoldText").GetComponent<DisplayGold>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeHP);
    }

    void UpgradeHP()
    {
        if(displayGold != null && this.displayGold.PlayerGold >= this.GoldNeeds)
        {
            this.displayGold.PlayerGold -= this.GoldNeeds;
            GameManager.Instance.SetGold(this.displayGold.PlayerGold);
            this.displayGold.UpdateGold(this.displayGold.PlayerGold);

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
