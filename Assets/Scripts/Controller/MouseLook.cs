using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f; // 마우스 감도

    [Header("References")]
    public Transform playerBody; // 플레이어 몸체
    public Texture2D customCursorTexture; // 커스텀 커서 텍스처

    private float xRotation = 0f; // x축 회전 값

    private void Start()
    {
        // 게임 시작 시 커스텀 커서 설정
        SetCustomCursor();
        LockMouse(); // 마우스 잠금
    }

    private void Update()
    {
        HandleMouseLook(); // 마우스 회전 처리
        HandleMouseInput(); // 마우스 입력 처리
    }

    // 마우스 회전 처리
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // x 회전 제한

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    // 마우스 입력 처리
    private void HandleMouseInput()
    {
        // 마우스 오른쪽 버튼 클릭 시 마우스 잠금 해제
        if (Input.GetMouseButtonDown(1))
        {
            UnlockMouse();
        }

        // UI가 아닌 곳 클릭 시 마우스 잠금
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            LockMouse();
        }

        // ESC 키 눌렸을 때 커서 처리
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetCursor();
        }
    }

    // 커서 설정 메서드
    private void SetCustomCursor()
    {
        if (customCursorTexture != null)
        {
            Cursor.SetCursor(customCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    // 커서 리셋 메서드
    private void ResetCursor()
    {
        SetCustomCursor(); // 커스텀 커서 다시 설정
        UnlockMouse(); // 마우스 잠금 해제
    }

    // 마우스 잠금 처리
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // 커서 숨김
    }

    // 마우스 잠금 해제 처리
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 커서 표시
    }

    // 현재 마우스가 UI 위에 있는지 감지
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
