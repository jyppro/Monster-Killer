using UnityEngine;

public class RankingPageOpen : MonoBehaviour
{
    public GameObject RankingPage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenRanking); }

    public void OpenRanking() {RankingPage.SetActive(true); }
}
