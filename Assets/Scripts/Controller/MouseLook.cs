using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;

    // 커스텀 커서 텍스처를 위한 public 변수
    public Texture2D customCursorTexture;

    void Start()
    {
        // 게임이 시작될 때 커스텀 커서 설정
        if (customCursorTexture != null)
        {
            Cursor.SetCursor(customCursorTexture, Vector2.zero, CursorMode.Auto);
        }
        
        LockMouse();
        //WebGLInput.captureAllKeyboardInput = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // 마우스 오른쪽 버튼을 누르면 마우스 잠금 해제
        if (Input.GetMouseButtonDown(1))
        {
            UnlockMouse();
        }

        // UI에 클릭이 없을 때만 마우스를 잠글 수 있게 처리
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            LockMouse();
        }

        // ESC키가 눌렸는지 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 커서가 기본으로 돌아왔을 때 커스텀 커서를 다시 설정
            Cursor.SetCursor(customCursorTexture, Vector2.zero, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

     // 마우스 잠금 처리
    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 마우스 잠금 해제 처리
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 현재 마우스가 UI 위에 있는지 감지
    bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
