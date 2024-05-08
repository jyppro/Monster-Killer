using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeIncreaseButton : MonoBehaviour
{
    [SerializeField] private int GoldNeeds = 10;
    public GameObject Timer;
    public GameObject Gold;

    void Start()
    {
        this.Timer = GameObject.Find("Timer");
        this.Gold = GameObject.Find("GoldText");
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UpgradeTime);
    }

    void UpgradeTime()
    {
        if(this.Gold.GetComponent<GoldController>().currentGold >= this.GoldNeeds)
        {
            this.Gold.GetComponent<GoldController>().currentGold -= this.GoldNeeds;
            this.Timer.GetComponent<TimerScript>().IncreaseMaxTime(1f);
            this.Timer.GetComponent<TimerScript>().UpdateTimerText();
            this.Gold.GetComponent<GoldController>().UpdateGoldText();
        }
        else{
            return;
        }
    }
}
