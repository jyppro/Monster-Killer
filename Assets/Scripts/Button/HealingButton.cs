using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    public GameObject HP;
    public GameObject Gold;

    void Start()
    {
        this.HP = GameObject.Find("PlayerHP");
        this.Gold = GameObject.Find("GoldText");
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(HealHP);
    }

    void HealHP()
    {
        if(this.Gold.GetComponent<GoldController>().currentGold >= this.GoldNeeds)
        {
            this.Gold.GetComponent<GoldController>().currentGold -= this.GoldNeeds;
            this.HP.GetComponent<PlayerHP>().playerCurrentHealth = this.HP.GetComponent<PlayerHP>().playerMaxHealth;
            this.HP.GetComponent<PlayerHP>().UpdateHealthSlider();
            this.Gold.GetComponent<GoldController>().UpdateGoldText();
        } else { return; }
    }
}
