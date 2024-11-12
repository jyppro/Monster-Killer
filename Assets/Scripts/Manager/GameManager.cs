using System.Collections.Generic;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;

public class PlayerData{
    public string playerID;
    // public int playerID;
    public int rank;
    public int power;
    public int gold;
    public float time;
    public int maxHP;
    public int currentHP;
    public int sumScore;
}

[System.Serializable]
public class BaseStageData
{
    public float stageTime;
    public float threeStarTime;
    public float twoStarTime;
    public int score;
}

[System.Serializable]
public class HuntStageData : BaseStageData
{
    public GameObject[] monsterPrefabs;
    public int targetKillCount;
}

[System.Serializable]
public class BossStageData : BaseStageData
{
    public GameObject bossPrefab;
    public int targetKillCount;
}

[System.Serializable]
public class GuardianStageData : BaseStageData
{
    public GameObject[] guardianPrefabs;
    public float defenseTime;
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public HuntStageData[] huntStages;
    public BossStageData[] bossStages;
    public GuardianStageData[] guardianStages;

    [SerializeField] private int[] stagesCleared; // 스테이지 클리어 상태 저장 (0은 클리어하지 않음, 1은 클리어함)
    [SerializeField] private string playerID;
    // [SerializeField] private int playerID;

    [SerializeField] private int rank;
    [SerializeField] private int power;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int gold;
    [SerializeField] private int sumScore;
    [SerializeField] private float time;
    //플레이어가 웹에서 로그인 확인 체크용
    // private bool loginCheck = false;
    private Dictionary<int, int> stageHighScores = new Dictionary<int, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
            Destroy(gameObject);
        // if(!loginCheck){
        //     LoadGameData();
        // }
        // LoadGameData("100");
        StageClear();
        
        if (stagesCleared == null || stagesCleared.Length == 0)
        {
            stagesCleared = new int[30]; // 30개의 스테이지 클리어 상태 초기화
        }
    }

    public bool IsStageCleared(int modeIndex, int stageIndex)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        return stagesCleared[index] == 1;
    }

    public void SetStageCleared(int modeIndex, int stageIndex, bool cleared)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        stagesCleared[index] = cleared ? 1 : 0;
        ScoreCal();
        SaveGameData();
    }

    public void UnlockNextStage(int modeIndex, int currentStageIndex)
    {
        if (currentStageIndex < 9) // 스테이지 인덱스 0~9까지
        {
            SetStageCleared(modeIndex, currentStageIndex + 1, true);
        }
    }

    private int GetIndexFromModeAndStage(int modeIndex, int stageIndex)
    {
        return modeIndex * 10 + stageIndex; // 모드별로 스테이지 인덱스를 조합
    }

    public BaseStageData GetStageData(int modeIndex, int stageIndex)
    {
        switch (modeIndex)
        {
            case 0:
                return huntStages[stageIndex];
            case 1:
                return bossStages[stageIndex];
            case 2:
                return guardianStages[stageIndex];
            default:
                return null;
        }
    }

    public int GetHighScore(int modeIndex, int stageIndex)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        if (stageHighScores.ContainsKey(index))
        {
            return stageHighScores[index];
        }
        return 0;
    }

    public void SetHighScore(int modeIndex, int stageIndex, int score)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        if (stageHighScores.ContainsKey(index))
        {
            stageHighScores[index] = Mathf.Max(stageHighScores[index], score);
        }
        else
        {
            stageHighScores.Add(index, score);
        }
        ScoreCal();
        SaveGameData();
    }

    public void LoadGameData(string playerIDInput)
    {
        // playerID = PlayerPrefs.GetInt("playerID", 1);
        // rank = PlayerPrefs.GetInt("Rank", 1);
        // power = PlayerPrefs.GetInt("Power", 10);
        // maxHP = PlayerPrefs.GetInt("MaxHP", 100);
        // currentHP = PlayerPrefs.GetInt("CurrentHP", 100);
        // gold = PlayerPrefs.GetInt("Gold", 0);
        // sumScore = PlayerPrefs.GetInt("SumScore", 0);
        // time = PlayerPrefs.GetFloat("Time", 60.0f);

        // int playerIDInputTemp; //로그인 안될 시 기본적인 로드

        // playerIDInputTemp = "playerID_100";
        // playerIDInputTemp = 100;
        Debug.Log("로드게임:" + playerIDInput);
        FirebaseDatabase.LoadGameData(playerIDInput, gameObject.name, "onLoadSuccess", "OnLoadError");

        // stagesCleared = PlayerPrefsX.GetIntArray("StagesCleared", new int[30]);

        // // Load stage high scores
        // for (int i = 0; i < 30; i++)
        // {
        //     stageHighScores[i] = PlayerPrefs.GetInt("StageHighScore" + i, 0);
        // }
    }

    // public void LoadGameData(string playerID){
    //     //플레이어가 웹에서 로그인 확인 체크용
    //     // loginCheck = true;
    //     Debug.Log("플레이어아이디: " + playerID);
    //     FirebaseDatabase.LoadGameData(playerID, gameObject.name, "onLoadSuccess", "OnLoadError");
    // }

    public void StageClear()
    {
        stagesCleared = PlayerPrefsX.GetIntArray("StagesCleared", new int[30]);

        // Load stage high scores
        for (int i = 0; i < 30; i++)
        {
            stageHighScores[i] = PlayerPrefs.GetInt("StageHighScore" + i, 0);
        }
    }

    public void SaveGameData()
    {
        // PlayerPrefs.SetInt("PlayerID", playerID);
        // PlayerPrefs.SetInt("Rank", rank);
        // PlayerPrefs.SetInt("Power", power);
        // PlayerPrefs.SetInt("MaxHP", maxHP);
        // PlayerPrefs.SetInt("CurrentHP", currentHP);
        // PlayerPrefs.SetInt("Gold", gold);
        // PlayerPrefs.SetInt("SumScore", sumScore);
        // PlayerPrefs.SetFloat("Time", time);

        // playerID = "100";
        FirebaseDatabase.SaveGameData(playerID, 
        rank, 
        power, 
        maxHP, 
        currentHP, 
        gold, 
        sumScore,
        time, 
        gameObject.name, "onSaveSuccess", "OnSaveError");

        // PlayerPrefsX.SetIntArray("StagesCleared", stagesCleared);

        // // Save stage high scores
        // foreach (var kvp in stageHighScores)
        // {
        //     PlayerPrefs.SetInt("StageHighScore" + kvp.Key, kvp.Value);
        // }

        // PlayerPrefs.Save();
    }

    public void ScoreCal()
    {
        PlayerPrefsX.SetIntArray("StagesCleared", stagesCleared);

        // Save stage high scores
        foreach (var kvp in stageHighScores)
        {
            PlayerPrefs.SetInt("StageHighScore" + kvp.Key, kvp.Value);
        }

        PlayerPrefs.Save();
    }

    public void onLoadSuccess(string jsonData){
        //statusText.text = "Data loaded successfully: " + jsonData;
        var data = JsonUtility.FromJson<PlayerData>(jsonData);
        if(data.power <= 0 && data.gold <= 0 && data.time <= 0 && data.maxHP <= 0 && data.currentHP <= 0){
            playerID = data.playerID;
            rank = 0;
            power = 10;
            gold = 100;
            time = 20;
            maxHP = 100;
            currentHP = 100;
            sumScore = 0;
            Debug.Log("저장함수 실행");
            SaveGameData();
        }else{
            playerID = data.playerID;
            rank = data.rank;
            power = data.power;
            gold = data.gold;
            time = data.time;
            maxHP = data.maxHP;
            currentHP = data.currentHP;
            sumScore = data.sumScore;
        }



        // playerIDText.text = playerID.ToString();
        // rankText.text = rank.ToString();
        // powerText.text = power.ToString();
        // goldText.text = gold.ToString();
        // timeText.text = time.ToString();
    }

    public void onSaveSuccess(string info)
    {
        //statusText.text = info;
        Debug.Log("onSaveSuccess: " + info);
    }

    public void OnLoadError(string error){
        //statusText.text = "Error loading data: " + error;
        Debug.LogError(error);
    }

    public void OnSaveError(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        Debug.LogError(parsedError.message);
    }

    // public int GetPlayerID() { return playerID; }
    public string GetPlayerID() { return playerID; }
    public int GetRank() { return rank; }
    public int GetPower() { return power; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP; }
    public int GetGold() { return gold; }
    public int GetSumScore() { return sumScore; }
    public float GetTime() { return time; }

    // public void SetPlayerID(int value) { playerID = value; }
    public void SetPlayerID(string value) { playerID = value; }
    
    public void SetRank(int value) { rank = value; }
    public void SetPower(int value) { power = value; }
    public void SetMaxHP(int value) { maxHP = value; }
    public void SetCurrentHP(int value) { currentHP = value; }
    public void SetGold(int value) { gold = value; }
    public void SetSumScore(int value) { sumScore = value; }
    public void SetTime(float value) { time = value; }
}
