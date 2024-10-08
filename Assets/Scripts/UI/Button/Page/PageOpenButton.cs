using UnityEngine;

public class PageOpenButton : MonoBehaviour
{
    public GameObject Page;
    public PageManager pageManager;

    void Start()
    {
        pageManager = GameObject.Find("PageManager").GetComponent<PageManager>();
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpeningPage);
    }

    public void OpeningPage()
    {
        pageManager.OpenPage(Page);
    }
}
