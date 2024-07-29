/*
게임내에서 버튼, 파이어베이스 데이터베이스 연결
*/

using UnityEngine;
using UnityEngine.UI;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;


public class DatabaseHandler : MonoBehaviour
{
    public Text statusText;
    public InputField pathInputField;
    public InputField valueInputField;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
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