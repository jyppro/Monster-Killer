using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    public string sceneToLoad; // 인스펙터에서 설정할 씬 이름

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(LoginBtn);
    }

    void LoginBtn()
    {
        GameManager.Instance.LoadGameData();
        SceneManager.LoadScene(sceneToLoad);
    }
}