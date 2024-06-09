using UnityEngine;

public class UpgradePageClose : MonoBehaviour
{
    public GameObject UpgradePage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseUpgrade); }

    public void CloseUpgrade() { UpgradePage.SetActive(false); }
}
