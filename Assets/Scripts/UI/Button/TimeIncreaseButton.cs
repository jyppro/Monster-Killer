using UnityEngine;

public class TimeIncreaseButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    public float IncreaseTime = 1.0f;
    // public GoldController goldController;
    public DisplayPlayerTime DisplayPlayerTime;
    public DisplayGold displayGold;
    

    void Start()
    {
        // PlayerGold = GameManager.Instance.GetGold();
        this.DisplayPlayerTime = GameObject.Find("Timer").GetComponent<DisplayPlayerTime>();
        //this.goldController = GameObject.Find("GoldController").GetComponent<GoldController>();
        this.displayGold = GameObject.Find("GoldText").GetComponent<DisplayGold>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeTime);
    }

    void UpgradeTime()
    {
        if(displayGold != null && this.displayGold.PlayerGold >= this.GoldNeeds)
        {
            this.displayGold.PlayerGold -= this.GoldNeeds;
            GameManager.Instance.SetGold(this.displayGold.PlayerGold);
            this.displayGold.UpdateGold(this.displayGold.PlayerGold);


            float newTime = this.DisplayPlayerTime.PlayerTime + IncreaseTime;
            this.DisplayPlayerTime.UpdateTime(newTime); // 새로운 시간을 설정하고 UI 업데이트

            // GameManager.Instance.SetTime(newTime); // 업그레이드 시 데이터 저장 (최대 시간)
            
            // GameManager.Instance.SaveGameData();
        }
        else
        {
            return;
        }
    }
}
