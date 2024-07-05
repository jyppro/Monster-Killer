using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private GameObject[] allObjects; // 모든 자식 오브젝트를 저장할 배열
    private Color[] originalColors; // 각 오브젝트의 원래 색상을 저장할 배열

    private void Start()
    {
        // 모든 자식 오브젝트를 가져와 배열에 저장
        allObjects = GetAllChildren(transform);

        // 각 오브젝트의 원래 색상을 저장할 배열 초기화
        originalColors = new Color[allObjects.Length];

        // 초기 상태에서 모든 오브젝트를 투명하게 설정
        for (int i = 0; i < allObjects.Length; i++)
        {
            Renderer renderer = allObjects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                originalColors[i] = renderer.material.color; // 각 오브젝트의 원래 색상 저장
                renderer.material.color = new Color(originalColors[i].r, originalColors[i].g, originalColors[i].b, 0f); // 투명하게 설정
            }
        }

        // GameManager의 OnGameOver 이벤트에 HandleGameOver 메서드를 등록
        GameManager.Instance.OnGameOver += HandleGameOver;
    }

    // 모든 자식 오브젝트를 배열로 반환하는 재귀적 함수
    private GameObject[] GetAllChildren(Transform parent)
    {
        var children = new GameObject[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i).gameObject;
            children[i].SetActive(true); // 비활성화된 경우 활성화 처리
            children = GetAllChildren(children[i].transform);
        }
        return children;
    }

    // 게임 종료 시 호출되는 함수
    private void HandleGameOver()
    {
        // 원래 색상으로 복원
        for (int i = 0; i < allObjects.Length; i++)
        {
            Renderer renderer = allObjects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColors[i];
            }
        }
    }

    private void OnDestroy()
    {
        // 게임 오브젝트가 파괴될 때 이벤트 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOver -= HandleGameOver;
        }
    }
}
