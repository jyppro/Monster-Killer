using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GoldController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] goldTexts;  // 모든 골드 텍스트를 담을 배열
    public int PlayerGold;

    // 싱글톤 인스턴스
    public static GoldController Instance;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject);
        }

        FindAllGoldTexts();

        // 씬 전환 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayerGold = GameManager.Instance.GetGold();
        UpdateGoldText();  // 골드 텍스트 업데이트
    }

    private void OnDestroy()
    {
        // 씬 전환 이벤트 구독 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때 골드 텍스트 업데이트
        FindAllGoldTexts();
        
        // PlayerGold를 GameManager에서 동기화
        PlayerGold = GameManager.Instance.GetGold();
        UpdateGoldText();
    }

    public void FindAllGoldTexts()
    {
        // 모든 씬에서 GoldText라는 이름의 TextMeshProUGUI 컴포넌트들을 찾음
        goldTexts = FindObjectsOfType<TextMeshProUGUI>().Where(text => text.gameObject.name == "GoldText").ToArray();
    }

    public void UpdateGoldText()
    {
        // 골드 텍스트 배열이 비어있지 않으면 업데이트 실행
        if (goldTexts != null && goldTexts.Length > 0)
        {
            foreach (var goldText in goldTexts)
            {
                if (goldText != null)
                {
                    goldText.text = "Gold : " + PlayerGold + "G";  // 골드 텍스트 업데이트
                }
            }
        }
        else
        {
            Debug.LogError("골드 텍스트가 할당되지 않았습니다.");
            return;
        }
    }

    public void GoldSum(int goldReward)
    {
        PlayerGold += goldReward;  // 골드 증가
        GameManager.Instance.SetGold(PlayerGold);  // GameManager에도 골드 업데이트
        UpdateGoldText();  // 골드 텍스트 업데이트
    }
}
