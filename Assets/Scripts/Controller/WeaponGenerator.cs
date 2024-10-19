using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    public GameObject weaponPrefab; // 무기 프리팹
    public Camera mainCamera; // 메인 카메라
    public Vector3 controlOffset = new Vector3(0.5f, -0.8f, 1.0f); // 무기 위치 오프셋
    public float throwSpeed = 500f; // 던질 때의 속도
    public float delay = 0.7f; // 무기 재생성 딜레이

    public GameObject currentWeapon; // 현재 무기
    public bool canGenerate = true; // 무기 생성 가능 여부
    private Vector3 spawnPosition; // 무기 생성 위치
    public bool isWeaponGenerated = false; // 무기 생성 상태 확인
    public bool isThrowing = false; // 무기 던지기 상태 확인 추가

    private void Start()
    {
        InitializeWeaponDamage();
    }

    private void Update()
    {
        HandleWeaponGeneration();
    }

    // 무기 데미지 초기화
    private void InitializeWeaponDamage()
    {
        var weaponController = weaponPrefab.GetComponent<WeaponController>();
        if (weaponController != null)
        {
            weaponController.currentDamage = GameManager.Instance.GetPower();
        }
        else
        {
            Debug.LogWarning("WeaponController가 WeaponPrefab에 존재하지 않습니다.");
        }
    }

    // 무기 생성과 관련된 처리
    private void HandleWeaponGeneration()
    {
        if (currentWeapon == null && canGenerate && !isThrowing)
        {
            GenerateWeapon();
        }

        if (currentWeapon != null)
        {
            UpdateWeaponPosition();
        }

        if (Input.GetMouseButtonDown(0) && currentWeapon != null && !isThrowing) // 무기 던지기 처리
        {
            ThrowWeapon();
            currentWeapon = null;
            canGenerate = false;
            isThrowing = true; // 던지기 상태 시작
            Invoke(nameof(EnableWeaponGeneration), delay);
        }
    }

    // 무기를 생성하는 함수
    public void GenerateWeapon()
    {
        currentWeapon = Instantiate(weaponPrefab);
        var rb = currentWeapon.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        UpdateWeaponPosition();
        isWeaponGenerated = true;
    }

    // 무기 위치 업데이트
    private void UpdateWeaponPosition()
    {
        spawnPosition = mainCamera.transform.position + mainCamera.transform.rotation * controlOffset;
        currentWeapon.transform.position = spawnPosition;
        currentWeapon.transform.rotation = mainCamera.transform.rotation;
    }

    // 무기 던지기 처리
    private void ThrowWeapon()
    {
        var rb = currentWeapon.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.velocity = mainCamera.transform.forward.normalized * throwSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody가 currentWeapon에 존재하지 않습니다.");
        }
    }

    // 무기 재생성 활성화
    private void EnableWeaponGeneration()
    {
        canGenerate = true;
        isThrowing = false; // 던지기 상태 해제
    }
}