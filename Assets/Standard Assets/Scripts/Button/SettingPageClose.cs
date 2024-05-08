using UnityEngine;

public class SettingPageClose : MonoBehaviour
{
    public GameObject SettingsPage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseSettings); }

    public void CloseSettings() {SettingsPage.SetActive(false); }
}
