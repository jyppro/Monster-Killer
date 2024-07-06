using UnityEngine;

public class HPUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseMaxHealth = 10;
    public PlayerHP HP;
    public GoldController Gold;

    void Start()
    {
        this.HP = GameObject.Find("PlayerHPBar").GetComponent<PlayerHP>();
        this.Gold = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeHP);
    }

    void UpgradeHP()
    {
        if(this.Gold.currentGold >= this.GoldNeeds)
        {
            this.Gold.currentGold -= this.GoldNeeds;
            this.HP.playerMaxHealth += IncreaseMaxHealth;
            this.HP.UpdateHealthSlider();
            this.Gold.UpdateGoldText();

            GameManager.Instance.SetMaxHP(this.HP.playerMaxHealth); // 업그레이드 시 데이터 저장 (최대 체력)
        }
        else
        {
            return;
        }
    }
}
