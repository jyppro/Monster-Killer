using UnityEngine;

public class UpgradePageOpen : MonoBehaviour
{
    public GameObject UpgradePage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenUpgrade); }

    public void OpenUpgrade() {UpgradePage.SetActive(true); }
}
