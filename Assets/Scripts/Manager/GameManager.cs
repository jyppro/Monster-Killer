using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    // 게임 데이터 변수
    [SerializeField] private int playerID;
    [SerializeField] private int stage;
    [SerializeField] private int rank;
    [SerializeField] private int power;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int gold;
    [SerializeField] private float time;


    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // 게임 데이터 초기 설정
        LoadGameData();
    }

    public void LoadGameData() // 게임 초기 데이터 로드
    {
        playerID = PlayerPrefs.GetInt("playerID", 1);
        stage = PlayerPrefs.GetInt("Stage", 1);
        rank = PlayerPrefs.GetInt("Rank", 1);
        power = PlayerPrefs.GetInt("Power", 10);
        maxHP = PlayerPrefs.GetInt("MaxHP", 100);
        currentHP = PlayerPrefs.GetInt("CurrentHP", 100);
        gold = PlayerPrefs.GetInt("Gold", 0);

        time = PlayerPrefs.GetFloat("Time", 60.0f);
    }

    public void SaveGameData() // 게임 데이터 저장
    {
        PlayerPrefs.SetInt("PlayerID", playerID);
        PlayerPrefs.SetInt("Rank", rank);
        PlayerPrefs.SetInt("Stage", stage);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("MaxHP", maxHP);
        PlayerPrefs.SetInt("CurrentHP", currentHP);
        PlayerPrefs.SetInt("Gold", gold);

        PlayerPrefs.SetFloat("Time", time);
        PlayerPrefs.Save();
    }

    // 게임 데이터 가져오기
    public int GetPlayerID() { return playerID; }
    public int GetStage() { return stage; }
    public int GetRank() { return rank; }
    public int GetPower() { return power; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP; }
    public int GetGold() { return gold; }

    public float GetTime() { return time; }


    // 게임 데이터 설정하기
    public void SetPlayerID(int value) { playerID = value; }
    public void SetStage(int value) { stage = value; }
    public void SetRank(int value) { rank = value; }
    public void SetPower(int value) { power = value; }
    public void SetMaxHP(int value) { maxHP = value; }
    public void SetCurrentHP(int value) { currentHP = value; }
    public void SetGold(int value) { gold = value; }

    public void SetTime(float value) { time = value; }
    
    // ----------------------------------------------------
}
