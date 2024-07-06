using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseDamage = 10;
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    public GoldController Gold;

    void Start()
    {
        this.Gold = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradePower);
    }

    void UpgradePower()
    { 
        if(this.Gold.currentGold >= this.GoldNeeds)
        {
            this.Gold.currentGold -= this.GoldNeeds;
            WeaponPrefab.GetComponent<WeaponController>().currentDamage += IncreaseDamage;
            this.Gold.UpdateGoldText();

            GameManager.Instance.SetPower(WeaponPrefab.GetComponent<WeaponController>().damage); // 업그레이드 시 데이터 저장 (공격력)
        }
        else
        {
            return;
        }
    }
}
