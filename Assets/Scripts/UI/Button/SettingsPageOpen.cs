using UnityEngine;

public class SettingsPageOpen : MonoBehaviour
{
    public GameObject SettingsPage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenSettings); }

    public void OpenSettings() { SettingsPage.SetActive(true); }
}
