using UnityEngine;

public class RankingClose : MonoBehaviour
{
    public GameObject RankingPage;
    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseRanking); }

    public void CloseRanking() { RankingPage.SetActive(false); }
}
