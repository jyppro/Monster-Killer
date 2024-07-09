using UnityEngine;

public class HealingButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    public PlayerHP HP;
    public GoldController Gold;

    void Start()
    {
        this.HP = GameObject.Find("PlayerHPBar").GetComponent<PlayerHP>();
        this.Gold = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(HealHP);
    }

    void HealHP()
    {
        if(this.Gold.PlayerGold >= this.GoldNeeds)
        {
            this.Gold.PlayerGold -= this.GoldNeeds;
            this.HP.playerCurrentHealth = this.HP.playerMaxHealth;
            this.HP.UpdateHealthSlider();
            this.Gold.UpdateGoldText();
        }
        else
        {
            return;
        }
    }
}
