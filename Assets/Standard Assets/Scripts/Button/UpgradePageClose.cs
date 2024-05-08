using UnityEngine;

public class UpgradePageClose : MonoBehaviour
{
    public GameObject UpgradePage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenUpgrade); }

    public void OpenUpgrade() { UpgradePage.SetActive(false); }
}
