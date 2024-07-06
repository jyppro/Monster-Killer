using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    [SerializeField] private Camera mainCamera; // 메인 카메라
    private GameObject currentWeapon; // 현재 무기
    private float delay = 0.7f;
    private bool canGenerate = true; // 생성가능 상태체크
    // private float xOffset = -2.0f; // x 좌표 오프셋 값
    Vector3 controlOffset = new (-1.0f, -0.8f, 0.0f);
    Vector3 spawnPosition;

    void Update()
    {
        if (currentWeapon == null && canGenerate) { GenerateWeapon(); }

        if (currentWeapon != null)
        {
            // 매 프레임마다 currentWeapon의 위치를 카메라 위치로 업데이트
            spawnPosition = mainCamera.transform.position;
            spawnPosition += controlOffset; // 위치 조정
            currentWeapon.transform.position = spawnPosition;
        }

        if (Input.GetMouseButtonDown(0) && currentWeapon != null) // 무기 던지기
        {
            currentWeapon.GetComponent<Rigidbody>().isKinematic = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 worldDir = ray.direction;
            currentWeapon.GetComponent<WeaponController>().Shoot(worldDir.normalized * 2000);

            currentWeapon = null;
            canGenerate = false;
            Invoke("EnableGeneration", delay);
        }
    }

    private void GenerateWeapon() // 무기 생성
    {
        currentWeapon = Instantiate(WeaponPrefab);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;

        // 생성된 무기를 카메라 위치로 이동
        Vector3 spawnPosition = mainCamera.transform.position;
        spawnPosition += controlOffset; // 위치 조정
        currentWeapon.transform.position = spawnPosition;
    }

    private void EnableGeneration() { canGenerate = true; }
}
