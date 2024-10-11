using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public CharacterController controller;
    public Transform cameraTransform;
    public WeaponGenerator weaponGenerator; // WeaponGenerator 스크립트 참조
    public GameObject originalWeaponPrefab;
    public GameObject weapon2Prefab;
    public GameObject magicAttackPrefab;
    public int weaponSwapDuration = 5;
    public int skill1Cooldown = 5;
    public int skill2Cooldown = 10;
    private bool isSwappingWeapon = false;
    private Button skill1Button;
    private Button skill2Button;
    private KeyPressSound keyPressSound;

    private void Start()
    {
        InitializeComponents();
        BindButtonEvents();
        CenterMouse();
    }

    private void Update()
    {
        HandleMovement();
        HandleWeaponSwap();
        HandleMagicAttack();
    }

    private void InitializeComponents()
    {
        keyPressSound = GameObject.Find("KeyPressSound")?.GetComponent<KeyPressSound>();
        skill1Button = GameObject.Find("Skill1Weapon")?.GetComponent<Button>();
        skill2Button = GameObject.Find("Skill2Explosion")?.GetComponent<Button>();
    }

    private void BindButtonEvents()
    {
        skill1Button?.onClick.AddListener(OnSkill1ButtonClicked);
        skill2Button?.onClick.AddListener(OnSkill2ButtonClicked);
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * speed * Time.deltaTime);
    }

    private void HandleWeaponSwap()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isSwappingWeapon && skill1Button?.interactable == true)
        {
            PlayKeyPressSound(0);
            StartCoroutine(SwapWeapon());
        }
    }

    private void HandleMagicAttack()
    {
        if (Input.GetKeyDown(KeyCode.X) && skill2Button?.interactable == true)
        {
            PlayKeyPressSound(1);
            CastMagicAttack();
        }
    }

    private void OnSkill1ButtonClicked()
    {
        if (!isSwappingWeapon)
            StartCoroutine(SwapWeapon());
    }

    private void OnSkill2ButtonClicked()
    {
        CastMagicAttack();
    }

    private IEnumerator SwapWeapon()
    {
        isSwappingWeapon = true;
        SetButtonInteractable(skill1Button, false);

        // 무기를 즉시 변경
        SwapToWeapon(weapon2Prefab);

        yield return new WaitForSeconds(weaponSwapDuration);

        // 무기를 지속 시간이 끝난 후 즉시 원래 무기로 변경
        SwapToWeapon(originalWeaponPrefab);

        isSwappingWeapon = false;
        StartCoroutine(SkillCooldown(skill1Button, skill1Cooldown));
    }

    private void SwapToWeapon(GameObject weaponPrefab)
    {
        // 무기 프리팹 변경
        weaponGenerator.weaponPrefab = weaponPrefab;

        // 기존 무기를 강제로 파괴하여 무기 교체가 즉시 이루어지도록 함
        if (weaponGenerator.currentWeapon != null)
        {
            Destroy(weaponGenerator.currentWeapon);
        }

        // 데미지 동기화
        SyncWeaponDamage(weaponPrefab);

        // 새로운 무기 생성
        weaponGenerator.GenerateWeapon();
        weaponGenerator.isWeaponGenerated = true;
    }

    private void SyncWeaponDamage(GameObject weaponPrefab)
    {
        var originalWeaponController = originalWeaponPrefab?.GetComponent<WeaponController>();
        var generatedWeaponController = weaponPrefab?.GetComponent<WeaponController>();

        if (originalWeaponController != null && generatedWeaponController != null)
        {
            generatedWeaponController.currentDamage = originalWeaponController.currentDamage;
        }
    }

    private void CastMagicAttack()
    {
        SetButtonInteractable(skill2Button, false);

        if (magicAttackPrefab != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Instantiate(magicAttackPrefab, hit.point, Quaternion.LookRotation(direction));
            }

            StartCoroutine(SkillCooldown(skill2Button, skill2Cooldown));
        }
    }

    private IEnumerator SkillCooldown(Button skillButton, int cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        SetButtonInteractable(skillButton, true);
    }

    private void PlayKeyPressSound(int index)
    {
        keyPressSound?.PlayKeyPressSound(index);
    }

    private void SetButtonInteractable(Button button, bool state)
    {
        if (button != null)
            button.interactable = state;
    }

    private void CenterMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(null, new Vector2(Screen.width / 2, Screen.height / 2), CursorMode.Auto);
    }
}