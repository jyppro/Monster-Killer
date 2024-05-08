using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    public GameObject Gold;

    void Start()
    {
        this.Gold = GameObject.Find("GoldText");
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradePower);
    }

    void UpgradePower()
    {
        if(this.Gold.GetComponent<GoldController>().currentGold >= this.GoldNeeds)
        {
            this.Gold.GetComponent<GoldController>().currentGold -= this.GoldNeeds;
            WeaponPrefab.GetComponent<WeaponController>().currentDamage += 10;
            this.Gold.GetComponent<GoldController>().UpdateGoldText();
        }
        else{
            return;
        }
    }
}
