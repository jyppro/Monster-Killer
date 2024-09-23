using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
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

        // ESC 키를 누르면 마우스 잠금 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockMouse();
        }

        // UI에 클릭이 없을 때만 마우스를 잠글 수 있게 처리
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            LockMouse();
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
