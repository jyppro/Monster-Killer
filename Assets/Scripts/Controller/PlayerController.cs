using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 0f;
    public CharacterController controller;
    public Transform cameraTransform;
    private Vector3 velocity;
    public WeaponGenerator weaponGenerator;
    public GameObject originalWeaponPrefab;  // 원래 무기 프리팹
    public GameObject weapon2Prefab;         // 무기2 프리팹
    public GameObject magicAttackPrefab;     // 마법 공격 프리팹
    public int weaponSwapDuration = 5;    // 원래 무기로 돌아가기까지의 시간
    public int skill1Cooldown = 5;
    public int skill2Cooldown = 10;
    private bool isSwappingWeapon = false;
    public Button skill1Button; // 스왑 버튼
    public Button skill2Button;
    public KeyPressSound keyPressSound;

    void Start()
    {
        keyPressSound = GameObject.Find("KeyPressSound").GetComponent<KeyPressSound>();
        skill1Button = GameObject.Find("Skill1Weapon").GetComponent<Button>();
        skill2Button = GameObject.Find("Skill2Explosion").GetComponent<Button>();

        // 버튼이 존재하는지 확인하고 이벤트 추가
        if (skill1Button != null)
        {
            skill1Button.onClick.AddListener(OnSkill1ButtonClicked);
        }

        // 버튼이 존재하는지 확인하고 이벤트 추가
        if (skill2Button != null)
        {
            skill2Button.onClick.AddListener(OnSkill2ButtonClicked);
        }

        CenterMouse(); // 마우스를 중앙으로 이동
    }

    void Update()
    {
        // 이동 코드
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        controller.Move(move * speed * Time.deltaTime);

        // 중력 코드
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Z키가 눌렸을 때 무기 스왑 처리 (버튼이 활성화되어 있을 때만)
        if (Input.GetKeyDown(KeyCode.Z) && !isSwappingWeapon && skill1Button.interactable)
        {
            PlayKeyPressSound(0); // 키프레스 사운드 재생 (첫 번째 클립)
            StartCoroutine(SwapWeapon());
        }

        // X키가 눌렸을 때 마법 공격 처리
        if(Input.GetKeyDown(KeyCode.X) && skill2Button.interactable)
        {
            PlayKeyPressSound(1); // 키프레스 사운드 재생 (두 번째 클립)
            CastMagicAttack();
        }
    }

    private void OnSkill1ButtonClicked()
    {
        if (!isSwappingWeapon)
        {
            PlayKeyPressSound(0); // 키프레스 사운드 재생 (첫 번째 클립)
            StartCoroutine(SwapWeapon());
        }
    }

    private void OnSkill2ButtonClicked()
    {
        PlayKeyPressSound(1); // 키프레스 사운드 재생 (두 번째 클립)
        CastMagicAttack();
    }

    private IEnumerator SwapWeapon()
    {
        isSwappingWeapon = true;

        // 버튼 비활성화
        if (skill1Button != null)
        {
            skill1Button.interactable = false;
        }

        // Weapon2로 교체하고 즉시 무기 업데이트
        weaponGenerator.WeaponPrefab = weapon2Prefab;
        weaponGenerator.GenerateWeapon(); // 즉시 무기 생성

        // 원래 무기의 공격력 동기화
        SyncWeaponDamage(weaponGenerator.WeaponPrefab);

        // 설정된 시간 동안 대기
        yield return new WaitForSeconds(weaponSwapDuration);

        // 원래 무기로 되돌리고 즉시 무기 업데이트
        weaponGenerator.WeaponPrefab = originalWeaponPrefab;

        // 쿨다운 시작
        StartCoroutine(Skill1Cooldown());

        isSwappingWeapon = false;
    }

    private void SyncWeaponDamage(GameObject weaponPrefab) // 기본 무기와 Z스킬 무기의 공격력 동기화
    {
        WeaponController originalWeaponController = originalWeaponPrefab.GetComponent<WeaponController>();
        WeaponController generatedWeaponController = weaponPrefab.GetComponent<WeaponController>();

        if (originalWeaponController != null && generatedWeaponController != null && generatedWeaponController.currentDamage != originalWeaponController.currentDamage)
        {
            generatedWeaponController.currentDamage = originalWeaponController.currentDamage;
        }
    }

    private void CastMagicAttack()
    {
        skill2Button.interactable = false;

        if (magicAttackPrefab != null)
        {
            // 마우스 포인터 위치 가져오기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 마우스 포인터가 위치한 지점 찾기
            if (Physics.Raycast(ray, out hit))
            {
                // 마법 공격을 생성하기 전에 방향 설정
                Vector3 spawnPosition = hit.point;

                // 카메라로부터 마우스 위치까지의 방향 계산
                Vector3 direction = (hit.point - transform.position).normalized;

                // 방향을 기반으로 회전값 설정
                Quaternion rotation = Quaternion.LookRotation(direction);

                // 마법 공격을 생성하고 방향 회전을 적용
                GameObject magicAttack = Instantiate(magicAttackPrefab, spawnPosition, rotation);
                Destroy(magicAttack, 5f);
            }

            // 쿨다운 시작
            StartCoroutine(Skill2Cooldown());
        }
    }


    private IEnumerator Skill1Cooldown()
    {
        // 쿨다운 시간 동안 대기
        yield return new WaitForSeconds(skill1Cooldown);

        // 버튼 다시 활성화
        if (skill1Button != null)
        {
            skill1Button.interactable = true;
        }
    }

    private IEnumerator Skill2Cooldown()
    {
        // 쿨다운 시간 동안 대기
        yield return new WaitForSeconds(skill2Cooldown);

        // 버튼 다시 활성화
        if (skill2Button != null)
        {
            skill2Button.interactable = true;
        }
    }

    private void PlayKeyPressSound(int index)
    {
        // KeyPressSound 클래스에서 사운드 재생
        if (keyPressSound != null)
        {
            keyPressSound.PlayKeyPressSound(index);
        }
    }

    // 마우스를 중앙으로 이동하는 함수
    private void CenterMouse()
    {
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.SetCursor(null, new Vector2(Screen.width / 2, Screen.height / 2), CursorMode.Auto); // 마우스 중앙으로 이동
    }
}
