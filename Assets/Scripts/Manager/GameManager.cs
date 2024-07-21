using UnityEngine;

[System.Serializable]
public class StageData
{
    public string stageName;
    public GameObject[] monsterPrefabs; // 여러 몬스터 프리팹을 저장
    public int targetKillCount; // 목표 처치 마리 수
    public float stageTime; // 스테이지 진행시간
    public float threeStarTime; // 별 3개로 클리어할 수 있는 최소시간
    public float twoStarTime; // 별 2개로 클리어할 수 있는 최소시간
    public int score; // 점수
}

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public StageData[] stages;

    // 게임 데이터 변수
    [SerializeField] private int playerID;
    [SerializeField] private int rank;
    [SerializeField] private int power;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int gold;
    [SerializeField] private int sumScore; // 플레이어 스코어 총합
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

    public StageData GetStageData(int stageIndex)
    {
        if (stageIndex >= 0 && stageIndex < stages.Length)
        {
            return stages[stageIndex];
        }
        return null;
    }

    public void LoadGameData() // 게임 초기 데이터 로드
    {
        playerID = PlayerPrefs.GetInt("playerID", 1);
        rank = PlayerPrefs.GetInt("Rank", 1);
        power = PlayerPrefs.GetInt("Power", 10);
        maxHP = PlayerPrefs.GetInt("MaxHP", 100);
        currentHP = PlayerPrefs.GetInt("CurrentHP", 100);
        gold = PlayerPrefs.GetInt("Gold", 0);
        sumScore = PlayerPrefs.GetInt("SumScore", 0);

        time = PlayerPrefs.GetFloat("Time", 60.0f);
    }

    public void SaveGameData() // 게임 데이터 저장
    {
        PlayerPrefs.SetInt("PlayerID", playerID);
        PlayerPrefs.SetInt("Rank", rank);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("MaxHP", maxHP);
        PlayerPrefs.SetInt("CurrentHP", currentHP);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("SumScore", sumScore);

        PlayerPrefs.SetFloat("Time", time);
        PlayerPrefs.Save();
    }

    // 게임 데이터 가져오기
    public int GetPlayerID() { return playerID; }
    public int GetRank() { return rank; }
    public int GetPower() { return power; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP; }
    public int GetGold() { return gold; }
    public int GetSumScore() { return sumScore; }

    public float GetTime() { return time; }


    // 게임 데이터 설정하기
    public void SetPlayerID(int value) { playerID = value; }
    public void SetRank(int value) { rank = value; }
    public void SetPower(int value) { power = value; }
    public void SetMaxHP(int value) { maxHP = value; }
    public void SetCurrentHP(int value) { currentHP = value; }
    public void SetGold(int value) { gold = value; }
    public void SetSumScore(int value) { sumScore = value; }

    public void SetTime(float value) { time = value; }
}
