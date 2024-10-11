using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickSound : MonoBehaviour
{
    void Start()
    {
        UnlockMouseMain();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GetComponent<AudioSource>().Play();
        }
    }

    // 마우스 잠금 해제 처리
    public void UnlockMouseMain()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; // 커서 표시
    }
}
