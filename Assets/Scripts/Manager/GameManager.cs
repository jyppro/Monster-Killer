using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;

//Unity의 JsonUtility는 배열을 바로 변환할 수 없으므로, JSON 데이터를 배열로 파싱하기 위해 JsonHelper 클래스를 추가합니다.
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


[System.Serializable]
public class RankData
{
    public string rank_PlayerID;
    public string rank_Name;
    public int rank_Score;
    public int rank_Rank;
}

[System.Serializable]
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
public class StagesClearedData{
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

    [SerializeField] private int[] stagesCleared; // 스테이지 클리어 상태 저장 (0은 클리어하지 않음, 1은 클리어함)

    //랭킹 전용 데이터
    public List<RankData> rankingsList = new List<RankData>();
    public event Action OnRankingsLoaded;

    // 개인 데이터
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
        Debug.Log("[1]게임 매니져 Awake 실행 순서 확인용");
        // 파이어베이스에서 관리가 될려면은 이 부분은 실행이 안되는 것이 좋으나
        // 그냥 두고 덮어씌우는 방식으로 진행
        StageClear();
        
        // 이부분은 로드을 했을 때 실행이 되어야 하지만 (초기 데이터 설정)
        // Awake가 먼저 실행이 되므로 그냥 두고 (로컬 저장)
        // TODO: 초기에 로드를 했을 때 초기 데이터 저장을 스테이지 클리어 데이터도 함깨저장 밑에 코드 활용
        // TODO: 데이터를 저장을 할때 스테이지 클리어도 저장 배열 주의
        // 로컬저장과 데이터베이스에 같이 저장이 되어 있지만 덮어씌으면 데이터베이스에서 관리 되는 것 처럼
        if (stagesCleared == null || stagesCleared.Length == 0)
        {
            stagesCleared = new int[30]; // 30개의 스테이지 클리어 상태 초기화
        }
        Debug.Log("[2]게임 매니져 Awake 실행 순서 확인용");
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
        SaveStagesClearedData();

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
            time = 30;
            maxHP = 100;
            currentHP = 100;
            sumScore = 0;
            LoadStagesClearedData();
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
            LoadStagesClearedData();
            Debug.Log("로드 완료");
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

    public void LoadRankingsData()
    {
        FirebaseDatabase.LoadRankingsData(gameObject.name, "OnLoadSuccessRankings", "OnLoadErrorRankings");
    }

    public void OnLoadSuccessRankings(string jsonData)
    {
        Debug.Log("Rankings Loaded: " + jsonData);
        
        // 기존 리스트 초기화
        rankingsList.Clear();
        // JSON 데이터를 RankData 리스트로 변환
        try
        {
            RankData[] rankingsArray = JsonHelper.FromJson<RankData>(jsonData);
            rankingsList.AddRange(rankingsArray);

            // 데이터 확인 (디버그 출력)
            foreach (var rank in rankingsList)
            {
                Debug.Log($"PlayerID: {rank.rank_PlayerID}, Name: {rank.rank_Name}, Score: {rank.rank_Score}, Rank: {rank.rank_Rank}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error parsing rankings data: " + ex.Message);
        }
        // 데이터 로딩이 완료되면 이벤트 호출
        OnRankingsLoaded?.Invoke();
    }

    public void OnLoadErrorRankings(string error)
    {
        Debug.LogError("Error loading rankings: " + error);
    }

    public List<RankData> GetRankDataList()
    {
        return rankingsList;
    }

    public void SaveStagesClearedData()
    {
        // string stagesClearedJSON = JsonUtility.ToJson(new { stages = stagesCleared });
        string stagesClearedJSON = ConvertArrayToJson(stagesCleared);
        Debug.Log("stagesClearedJSON:" + stagesClearedJSON);
        FirebaseDatabase.SaveStagesClearedData(playerID , stagesClearedJSON, gameObject.name, "OnSaveStagesClearedDataSuccess", "OnSaveStagesClearedDataError");
    }

    // 배열을 단순 JSON 배열로 변환하는 함수
    private string ConvertArrayToJson(int[] array)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i]);
            if (i < array.Length - 1) sb.Append(",");
        }
        sb.Append("]");
        return sb.ToString();
    }

    public void OnSaveStagesClearedDataSuccess(string info)
    {
        Debug.Log("onSaveStagesClearedDataSuccess: " + info);
    }

    public void OnSaveStagesClearedDataError(string error)
    {
        Debug.LogError("Error onSaveStagesClearedData: " + error);
    }

    public void LoadStagesClearedData()
    {
        FirebaseDatabase.LoadStagesClearedData(playerID, gameObject.name, "OnLoadStagesClearedDataSuccess", "OnLoadStagesClearedDataError");
    }

    public void OnLoadStagesClearedDataSuccess(string jsonStagesClearedData)
    {
        // jsonStagesClearedData는 "[1,0,0,1,...]" 형식의 순수 배열 JSON 문자열 이어야 함
        int[] intStagesClearedData = ConvertJsonToIntArray(jsonStagesClearedData);
        Debug.Log("int[ ]로 변환 확인용 : " + string.Join(",", intStagesClearedData));
        // 데이터의 길이가 올바르지 않으면 새로운 배열을 초기화
        if(intStagesClearedData.Length != 30){
            stagesCleared = new int[30];
        }else{
            stagesCleared = intStagesClearedData;
        }
        // ScoreCal();
    }

    public void OnLoadStagesClearedDataError(string error)
    {
        Debug.LogError("Error onLoadStagesClearedData: " + error);
    }

    // JSON 문자열 배열을 int[] 배열로 변환하는 함수
    private int[] ConvertJsonToIntArray(string jsonArray)
    {
        // JSON 배열 문자열의 양쪽 대괄호 제거
        jsonArray = jsonArray.Trim('[', ']');

        // 빈 문자열이면 빈 배열 반환
        if (string.IsNullOrEmpty(jsonArray)) return new int[30];

        // 문자열을 콤마로 분리하고 int로 변환
        return jsonArray.Split(',').Select(int.Parse).ToArray();
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
