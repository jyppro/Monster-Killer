using UnityEngine;

public class TimeIncreaseButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    public TimerScript Timer;
    public GoldController Gold;
    public float IncreaseTime = 1.0f;

    void Start()
    {
        this.Timer = GameObject.Find("Timer").GetComponent<TimerScript>();
        this.Gold = GameObject.Find("GoldController").GetComponent<GoldController>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeTime);
    }

    void UpgradeTime()
    {
        if(this.Gold.currentGold >= this.GoldNeeds)
        {
            this.Gold.currentGold -= this.GoldNeeds;
            this.Timer.maxTime += IncreaseTime;
            this.Timer.UpdateTimerText();
            this.Gold.UpdateGoldText();

            GameManager.Instance.SetTime(this.Timer.maxTime); // 업그레이드 시 데이터 저장 (최대 시간)
        }
        else
        {
            return;
        }
    }
}
