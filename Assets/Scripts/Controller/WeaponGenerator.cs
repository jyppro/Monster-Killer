using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] public GameObject WeaponPrefab; // 무기 프리팹
    [SerializeField] private Camera mainCamera; // 메인 카메라
    public GameObject currentWeapon; // 현재 무기
    private float delay = 0.7f;
    public bool canGenerate = true; // 생성가능 상태체크
    [SerializeField] Vector3 controlOffset = new Vector3(0.5f, -0.8f, 1.0f);
    [SerializeField] private float throwSpeed = 500f; // 던질 때의 속도
    public Vector3 spawnPosition;
    // public PlayerController PlayerController;
    public bool isWeaponGenerated = false;

    void Start()
    {
        WeaponPrefab.GetComponent<WeaponController>().currentDamage = GameManager.Instance.GetPower();
        // PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (currentWeapon == null && canGenerate) 
        {
            GenerateWeapon();
        }

        if (currentWeapon != null)
        {
            // 매 프레임마다 currentWeapon의 위치를 카메라 위치로 업데이트
            spawnPosition = mainCamera.transform.position + mainCamera.transform.rotation * controlOffset;
            currentWeapon.transform.position = spawnPosition;
            currentWeapon.transform.rotation = mainCamera.transform.rotation;
        }

        if (Input.GetMouseButtonDown(0) && currentWeapon != null) // 무기 던지기
        {
            Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false; // 중력 비활성화

            // 카메라의 정면 방향으로 빠르게 던지기
            Vector3 shootDirection = mainCamera.transform.forward;
            rb.velocity = shootDirection.normalized * throwSpeed; // 던지는 속도 설정

            currentWeapon = null;
            canGenerate = false;
            Invoke("EnableGeneration", delay);
        }
    }

    public void GenerateWeapon() // 무기 생성
    {
        // 기존 무기 제거
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        
        currentWeapon = Instantiate(WeaponPrefab);
        currentWeapon.GetComponent<Rigidbody>().isKinematic = true;

        // 생성된 무기를 카메라 위치로 이동
        spawnPosition = mainCamera.transform.position + mainCamera.transform.rotation * controlOffset;
        currentWeapon.transform.position = spawnPosition;
        currentWeapon.transform.rotation = mainCamera.transform.rotation;
    }

    private void EnableGeneration() 
    {
        canGenerate = true; 
    }
}