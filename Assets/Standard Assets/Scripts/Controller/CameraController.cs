using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float angle = 0f; // 카메라 이동을 위한 각도
    [SerializeField] private float radius = 20f; // 원의 반지름
    [SerializeField] private float speed = 50f; // 카메라 이동 속도
    private KeyCode leftKey = KeyCode.A; // 왼쪽 방향 키
    private KeyCode rightKey = KeyCode.D; // 오른쪽 방향 키

    private void Update()
    {
        if (Input.GetKey(leftKey)) { angle -= speed * Time.deltaTime; } // 왼쪽 방향 키 입력 시
        else if (Input.GetKey(rightKey)) { angle += speed * Time.deltaTime; } // 오른쪽 방향 키 입력 시

        GameObject activeMonster = GameObject.FindGameObjectWithTag("Monster");

        if(activeMonster != null)
        {
            // 각도를 라디안으로 변환하여 카메라 위치 계산
            float radian = angle * Mathf.Deg2Rad;
            float x = activeMonster.transform.position.x + Mathf.Cos(radian) * radius;
            float z = activeMonster.transform.position.z + Mathf.Sin(radian) * radius;

            transform.position = new Vector3(x, transform.position.y, z); // 카메라 위치 설정
            transform.LookAt(activeMonster.transform); // 카메라가 항상 물체를 향하도록 설정
        }
    }
}
