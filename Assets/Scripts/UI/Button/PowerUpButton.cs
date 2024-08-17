using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseDamage = 10;
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    // public GoldController goldController;
    public DisplayPower displayPower;
    public DisplayGold displayGold;

    void Start()
    {
        this.displayPower = GameObject.Find("PowerText").GetComponent<DisplayPower>();
        //this.goldController = GameObject.Find("GoldController").GetComponent<GoldController>();
        this.displayGold = GameObject.Find("GoldText").GetComponent<DisplayGold>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradePower);
    }

    void UpgradePower()
    { 
        if(displayGold != null && this.displayGold.PlayerGold >= this.GoldNeeds)
        {
            this.displayGold.PlayerGold -= this.GoldNeeds;
            GameManager.Instance.SetGold(this.displayGold.PlayerGold);
            this.displayGold.UpdateGold(this.displayGold.PlayerGold);

            WeaponController weaponController = WeaponPrefab.GetComponent<WeaponController>();
            if (weaponController != null)
            {
                this.displayPower.PlayerPower += IncreaseDamage;
                //int newPower = weaponController.currentDamage + IncreaseDamage; // 데미지 강화값
                //weaponController.currentDamage += IncreaseDamage; // 데미지 강화
                this.displayPower.UpdatePower(this.displayPower.PlayerPower); // 새로운 공격력을 설정하고 UI 업데이트
                weaponController.currentDamage = this.displayPower.PlayerPower;
                GameManager.Instance.SaveGameData();
            }
            else
            {
                Debug.LogError("WeaponPrefab does not have a WeaponController component.");
            }
        }
        else
        {
            return;
        }
    }
}
