/*
게임내에서 버튼, 파이어베이스 데이터베이스 연결
*/

using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;

public class PlayerData{
    public int playerID;
    public int rank;
    public int power;
    public int gold;
    public float time;
}

public class DatabaseHandler : MonoBehaviour{
    public Text statusText;
    public Text playerIDText;
    public Text rankText;
    public Text powerText;
    public Text goldText;
    public Text timeText;


    public int playerID;
    public InputField playerID_InputField;
    public int rank;
    public InputField rank_InputField;
    public int power;
    public InputField power_InputField;
    public int gold;
    public InputField gold_InputField;
    public float time;
    public InputField time_InputField;

    public InputField pathInputField;
    public InputField valueInputField;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
    }

    public void SaveJSON() {
        if(int.TryParse(playerID_InputField.text, out playerID) && 
            int.TryParse(rank_InputField.text, out rank) && 
            int.TryParse(power_InputField.text, out power) && 
            int.TryParse(gold_InputField.text, out gold) && 
            float.TryParse(time_InputField.text, out time)){
                FirebaseDatabase.SaveJSON(playerID, rank, power, gold, time,
                gameObject.name,
                "DisplayInfo", "DisplayErrorObject");
        }
    }

    public void LoadJSON() {
        if(int.TryParse(playerID_InputField.text, out playerID)){
            string path = pathInputField.text;
            FirebaseDatabase.LoadJSON(playerID, path, gameObject.name, 
            "onLoadSuccess", "OnLoadError");
        }
    }

    public void PostJSON() => FirebaseDatabase.PostJSON(pathInputField.text, valueInputField.text, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");

    public void GetJSON() => FirebaseDatabase.GetJSON(pathInputField.text, gameObject.name,
            "DisplayData", "DisplayErrorObject");

     public void PushJSON() => FirebaseDatabase.PushJSON(pathInputField.text, valueInputField.text, gameObject.name,
            "DisplayInfo", "DisplayErrorObject");        
            
    public void UpdateJSON() => FirebaseDatabase.UpdateJSON(pathInputField.text, valueInputField.text,
            gameObject.name, "DisplayInfo", "DisplayErrorObject");
    
    public void DeleteJSON() =>
    FirebaseDatabase.DeleteJSON(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void ListenForValueChanged() =>
    FirebaseDatabase.ListenForValueChanged(pathInputField.text, gameObject.name, "DisplayData", "DisplayErrorObject");

    public void StopListeningForValueChanged() =>
    FirebaseDatabase.StopListeningForValueChanged(pathInputField.text, gameObject.name, "DisplayInfo", "DisplayErrorObject");

    public void DisplayData(string data)
    {
        statusText.text = data;
    }

    public void DisplayInfo(string info)
    {
        statusText.text = info;
    }

    public void onLoadSuccess(string jsonData){
        statusText.text = "Data loaded successfully: " + jsonData;
        var data = JsonUtility.FromJson<PlayerData>(jsonData);
        playerID = data.playerID;
        rank = data.rank;
        power = data.power;
        gold = data.gold;
        time = data.time;

        playerIDText.text = playerID.ToString();
        rankText.text = rank.ToString();
        powerText.text = power.ToString();
        goldText.text = gold.ToString();
        timeText.text = time.ToString();
    }

    public void OnLoadError(string error){
        statusText.text = "Error loading data: " + error;
    }

    public void DisplayErrorObject(string error)
    {
        var parsedError = JsonUtility.FromJson<FirebaseError>(error);
        DisplayError(parsedError.message);
    }

    public void DisplayError(string error)
    {
        statusText.text = error;
    }
}