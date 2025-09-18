using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;

// Unity의 JsonUtility는 배열을 바로 변환할 수 없으므로 Wrapper를 사용
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}

/*
    ------------ 
    Data Section 
    ------------ 
*/
[System.Serializable]
public class RankData
{
    public string rank_PlayerID;
    public string rank_Name;
    public int rank_Score;
    public int rank_Rank;
}

[System.Serializable]
public class PlayerData
{
    public string playerID;
    public int rank;
    public int power;
    public int gold;
    public float time;
    public int maxHP;
    public int currentHP;
    public int sumScore;
}

[System.Serializable]
public class StagesClearedData
{
    public int[] stagesClearedData;
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

    [SerializeField] private int[] stagesCleared; // 0: 미클리어, 1: 클리어

    public List<RankData> rankingsList = new List<RankData>();
    public event Action OnRankingsLoaded;

    [SerializeField] private int playerID;
    [SerializeField] private int rank;
    [SerializeField] private int power;
    [SerializeField] private int maxHP;
    [SerializeField] private int currentHP;
    [SerializeField] private int gold;
    [SerializeField] private int sumScore;
    [SerializeField] private float time;

    private Dictionary<int, int> stageHighScores = new Dictionary<int, int>();
    private Dictionary<int, BaseStageData[]> stageDataMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        stageDataMap = new Dictionary<int, BaseStageData[]>();
        if (huntStages != null) stageDataMap[0] = huntStages;
        if (bossStages != null) stageDataMap[1] = bossStages;
        if (guardianStages != null) stageDataMap[2] = guardianStages;

        GetStageClearData();

        if (stagesCleared == null || stagesCleared.Length == 0)
        {
            stagesCleared = new int[30];
        }
    }

    public bool IsStageCleared(int modeIndex, int stageIndex)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        return stagesCleared[index] == 1;
    }

    public void UnlockNextStage(int modeIndex, int currentStageIndex)
    {
        if (currentStageIndex < 9) 
        {
            SetStageCleared(modeIndex, currentStageIndex + 1, true);
        }
    }

    private string ConvertArrayToJson(int[] array)
    {
        return "[" + string.Join(",", array) + "]";
    }

    /*
        ------------ 
        Save Section 
        ------------ 
    */
    public void SaveAllData()
    {
        SaveScoreData();
        SaveGameData();
    }

    public void SaveScoreData()
    {
        PlayerPrefsX.SetIntArray("StagesCleared", stagesCleared);

        foreach (var kvp in stageHighScores)
        {
            PlayerPrefs.SetInt("StageHighScore" + kvp.Key, kvp.Value);
        }

        PlayerPrefs.Save();
    }

    public void SaveStagesClearedData()
    {
        string stagesClearedJSON = ConvertArrayToJson(stagesCleared);
        Debug.Log("stagesClearedJSON:" + stagesClearedJSON);
    }

    public void SaveGameData()
    {
        PlayerPrefs.SetInt("PlayerID", playerID);
        PlayerPrefs.SetInt("Rank", rank);
        PlayerPrefs.SetInt("Power", power);
        PlayerPrefs.SetInt("MaxHP", maxHP);
        PlayerPrefs.SetInt("CurrentHP", currentHP);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("SumScore", sumScore);
        PlayerPrefs.SetFloat("Time", time);
    }

    public void OnSaveSuccess(string info)
    {
        Debug.Log("OnSaveSuccess: " + info);
    }

    public void OnSaveError(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        Debug.LogError(parsedError.message);
    }

    /*
        ------------ 
        Load Section 
        ------------ 
    */
    public void LoadRankingsData()
    {
        FirebaseDatabase.LoadRankingsData(gameObject.name, "OnLoadSuccessRankings", "OnLoadErrorRankings");
    }

    public void OnLoadError(string error)
    {
        Debug.LogError(error);
    }

    public void OnLoadErrorRankings(string error)
    {
        Debug.LogError("Error loading rankings: " + error);
    }

    public void LoadGameData(string playerIDInput)
    {
        playerID = PlayerPrefs.GetInt("PlayerID", 1);
        rank = PlayerPrefs.GetInt("Rank", 1);
        power = PlayerPrefs.GetInt("Power", 10);
        maxHP = PlayerPrefs.GetInt("MaxHP", 100);
        currentHP = PlayerPrefs.GetInt("CurrentHP", 100);
        gold = PlayerPrefs.GetInt("Gold", 0);
        sumScore = PlayerPrefs.GetInt("SumScore", 0);
        time = PlayerPrefs.GetFloat("Time", 60.0f);
    }

    public void OnLoadSuccessRankings(string jsonData)
    {
        Debug.Log("Rankings Loaded: " + jsonData);

        rankingsList.Clear();
        try
        {
            RankData[] rankingsArray = JsonHelper.FromJson<RankData>(jsonData);
            rankingsList.AddRange(rankingsArray);

            foreach (var rank in rankingsList)
            {
                Debug.Log($"PlayerID: {rank.rank_PlayerID}, Name: {rank.rank_Name}, Score: {rank.rank_Score}, Rank: {rank.rank_Rank}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing rankings data: " + ex.Message);
        }

        OnRankingsLoaded?.Invoke();
    }

    public void OnLoadStagesClearedDataSuccess(string jsonStagesClearedData)
    {
        int[] intStagesClearedData = ConvertJsonToIntArray(jsonStagesClearedData);
        Debug.Log("int[ ] 변환 확인: " + string.Join(",", intStagesClearedData));

        if (intStagesClearedData.Length != 30)
            stagesCleared = new int[30];
        else
            stagesCleared = intStagesClearedData;
    }

    public void OnLoadStagesClearedDataError(string error)
    {
        Debug.LogError("Error onLoadStagesClearedData: " + error);
    }

    private int[] ConvertJsonToIntArray(string jsonArray)
    {
        jsonArray = jsonArray.Trim('[', ']');

        if (string.IsNullOrEmpty(jsonArray)) return new int[30];

        return jsonArray.Split(',')
                .Select(s => int.TryParse(s, out var num) ? num : 0)
                .ToArray();
    }

    /*
        ------------ 
        Get Section 
        ------------ 
    */
    public int GetRank() { return rank; }
    public int GetPower() { return power; }
    public int GetMaxHP() { return maxHP; }
    public int GetCurrentHP() { return currentHP; }
    public int GetGold() { return gold; }
    public int GetSumScore() { return sumScore; }
    public float GetTime() { return time; }

    private int GetIndexFromModeAndStage(int modeIndex, int stageIndex)
    {
        return modeIndex * 10 + stageIndex;
    }
    
    public int GetHighScore(int modeIndex, int stageIndex)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        return stageHighScores.ContainsKey(index) ? stageHighScores[index] : 0;
    }

    public void GetStageClearData()
    {
        stagesCleared = PlayerPrefsX.GetIntArray("StagesCleared", new int[30]);

        for (int i = 0; i < 30; i++)
        {
            stageHighScores[i] = PlayerPrefs.GetInt("StageHighScore" + i, 0);
        }
    }

    public BaseStageData GetStageData(int modeIndex, int stageIndex)
    {
        return stageDataMap.TryGetValue(modeIndex, out var stages) ? stages[stageIndex] : null;
    }

    public List<RankData> GetRankDataList()
    {
        return rankingsList;
    }

    /*
        ------------ 
        Set Section 
        ------------ 
    */
    public void SetRank(int value) { rank = value; }
    public void SetPower(int value) { power = value; }
    public void SetMaxHP(int value) { maxHP = value; }
    public void SetCurrentHP(int value) { currentHP = value; }
    public void SetGold(int value) { gold = value; }
    public void SetSumScore(int value) { sumScore = value; }
    public void SetTime(float value) { time = value; }

    public void SetStageCleared(int modeIndex, int stageIndex, bool cleared)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        stagesCleared[index] = cleared ? 1 : 0;
        SaveAllData();
    }

    public void SetHighScore(int modeIndex, int stageIndex, int score)
    {
        int index = GetIndexFromModeAndStage(modeIndex, stageIndex);
        if (stageHighScores.ContainsKey(index))
            stageHighScores[index] = Mathf.Max(stageHighScores[index], score);
        else
            stageHighScores.Add(index, score);

        SaveAllData();
    }
}
