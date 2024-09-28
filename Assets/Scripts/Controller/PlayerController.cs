using UnityEngine;
using UnityEngine.UI;  // UI 시스템을 사용하기 위한 네임스페이스 추가
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 0f;
    public CharacterController controller;
    public Transform cameraTransform;

    private Vector3 velocity;

    public WeaponGenerator weaponGenerator;  // WeaponGenerator에 대한 참조
    public GameObject originalWeaponPrefab;  // 원래 무기 프리팹
    public GameObject weapon2Prefab;         // 무기2 프리팹
    public int weaponSwapDuration = 5;    // 원래 무기로 돌아가기까지의 시간
    public int skill1Cooldown = 5;
    // public int skill2Cooldown = 20;

    private bool isSwappingWeapon = false;

    // UI 버튼에 대한 참조
    public Button skill1Button; // 스왑 버튼

    // UI 버튼에 대한 참조
    public Button skill2Button;

    // KeyPressSound 오브젝트에 대한 참조
    public KeyPressSound keyPressSound;

    void Start()
    {
        keyPressSound = GameObject.Find("KeyPressSound").GetComponent<KeyPressSound>();
        skill1Button = GameObject.Find("Skill1Weapon").GetComponent<Button>();
        // skill2Button = gameObject.Find("Skill2_Explosion").GetComponent<Button>();

        // 버튼이 존재하는지 확인하고 이벤트 추가
        if (skill1Button != null)
        {
            skill1Button.onClick.AddListener(OnSwapButtonClicked);
        }
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
    }

    private void OnSwapButtonClicked()
    {
        if (!isSwappingWeapon)
        {
            PlayKeyPressSound(0); // 키프레스 사운드 재생 (첫 번째 클립)
            StartCoroutine(SwapWeapon());
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

        // 설정된 시간 동안 대기
        yield return new WaitForSeconds(weaponSwapDuration);

        // 원래 무기로 되돌리고 즉시 무기 업데이트
        weaponGenerator.WeaponPrefab = originalWeaponPrefab;
        weaponGenerator.GenerateWeapon(); // 즉시 원래 무기 생성


        // 쿨다운 시작
        StartCoroutine(Cooldown());

        isSwappingWeapon = false;
    }

    private IEnumerator Cooldown()
    {
        // 쿨다운 시간 동안 대기
        yield return new WaitForSeconds(skill1Cooldown);

        // 버튼 다시 활성화
        if (skill1Button != null)
        {
            skill1Button.interactable = true;
        }
    }
}
