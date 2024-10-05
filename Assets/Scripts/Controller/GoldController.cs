using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class GoldController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] goldTexts;  // 모든 골드 텍스트를 담을 배열
    public int PlayerGold { get; private set; } // 골드 값

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
            return;
        }

        // 씬 전환 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        InitializeGold();  // 골드 초기화 및 텍스트 업데이트
    }

    private void OnDestroy()
    {
        // 이벤트 언구독
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateGoldDisplay();  // 씬 로드 시 골드 텍스트 업데이트
    }

    private void InitializeGold()
    {
        // PlayerGold를 GameManager에서 동기화하고 텍스트 업데이트
        PlayerGold = GameManager.Instance.GetGold();
        UpdateGoldDisplay();
    }

    private void UpdateGoldDisplay()
    {
        // 모든 씬에서 GoldText라는 이름의 TextMeshProUGUI 컴포넌트를 찾음
        goldTexts = FindObjectsOfType<TextMeshProUGUI>()
            .Where(text => text.gameObject.name == "GoldText").ToArray();

        // 골드 텍스트 배열이 비어있지 않으면 업데이트 실행
        foreach (var goldText in goldTexts)
        {
            if (goldText != null)
            {
                goldText.text = $"Gold: {PlayerGold}G";  // 골드 텍스트 업데이트
            }
        }
    }

    public void GoldSum(int goldReward)
    {
        PlayerGold += goldReward;  // 골드 증가
        GameManager.Instance.SetGold(PlayerGold);  // GameManager에도 골드 업데이트
        UpdateGoldDisplay();  // 골드 텍스트 업데이트
    }
}
