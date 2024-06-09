using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    // 게임 데이터 변수
    private int stage;
    private float power;
    private float gold;
    private float maxHP;
    private float currentHP;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // 게임 데이터 초기화
        LoadGameData();
    }

    public void LoadGameData()
    {
        // 게임 데이터 로드
        stage = PlayerPrefs.GetInt("Stage", 1);
        power = PlayerPrefs.GetFloat("Power", 0f);
        gold = PlayerPrefs.GetFloat("Gold", 0f);
        maxHP = PlayerPrefs.GetFloat("MaxHP", 100f);
        currentHP = PlayerPrefs.GetFloat("CurrentHP", 100f);
    }

    public void SaveGameData()
    {
        // 게임 데이터 저장
        PlayerPrefs.SetInt("Stage", stage);
        PlayerPrefs.SetFloat("Power", power);
        PlayerPrefs.SetFloat("Gold", gold);
        PlayerPrefs.SetFloat("MaxHP", maxHP);
        PlayerPrefs.SetFloat("CurrentHP", currentHP);
        PlayerPrefs.Save();
    }

    // 게임 데이터 가져오기
    public int GetStage() { return stage; }
    public float GetPower() { return power; }
    public float GetGold() { return gold; }
    public float GetMaxHP() { return maxHP; }
    public float GetCurrentHP() { return currentHP; }

    // 게임 데이터 설정하기
    public void SetStage(int value) { stage = value; }
    public void SetPower(float value) { power = value; }
    public void SetGold(float value) { gold = value; }
    public void SetMaxHP(float value) { maxHP = value; }
    public void SetCurrentHP(float value) { currentHP = value; }
}
