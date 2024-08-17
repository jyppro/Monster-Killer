using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(SaveBtn);
    }

    void SaveBtn()
    {
        GameManager.Instance.SaveGameData();
    }
}
