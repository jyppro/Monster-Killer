using UnityEngine;

public class PageCloseButton : MonoBehaviour
{
    public GameObject Page;

    void Start() { GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ClosePage); }

    public void ClosePage() { Page.SetActive(false); }
}
