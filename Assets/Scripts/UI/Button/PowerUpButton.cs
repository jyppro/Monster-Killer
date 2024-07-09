using UnityEngine;

public class PowerUpButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    [SerializeField] private int IncreaseDamage = 10;
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    public GoldController goldController;
    public DisplayPower displayPower;

    void Start()
    {
        this.displayPower = GameObject.Find("PowerText").GetComponent<DisplayPower>();
        this.goldController = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradePower);
    }

    void UpgradePower()
    { 
        if(goldController != null && this.goldController.PlayerGold >= this.GoldNeeds)
        {
            this.goldController.PlayerGold -= this.GoldNeeds;
            GameManager.Instance.SetGold(this.goldController.PlayerGold);
            this.goldController.UpdateGoldText();

            WeaponController weaponController = WeaponPrefab.GetComponent<WeaponController>();
            if (weaponController != null)
            {
                // int newPower = weaponController.currentDamage + IncreaseDamage; // 데미지 강화값
                weaponController.currentDamage += IncreaseDamage; // 데미지 강화
                this.displayPower.UpdatePower(weaponController.currentDamage); // 새로운 공격력을 설정하고 UI 업데이트
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
