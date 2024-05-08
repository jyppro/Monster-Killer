using UnityEngine;

public class WeaponGenerator : MonoBehaviour
{
    [SerializeField] private GameObject WeaponPrefab; // 무기 프리팹
    private GameObject currentWeapon; // 현재 무기
    private float delay = 0.7f;
    private bool canGenerate = true; // 생성가능 상태체크

    void Update()
    {
        if (currentWeapon == null && canGenerate) { GenerateWeapon(); }

        if (Input.GetMouseButtonDown(0) && currentWeapon != null) // 무기 던지기
        {
            Vector3 spawnPosition = Camera.main.transform.position;
            currentWeapon.transform.position = spawnPosition;

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
    }

    private void EnableGeneration() { canGenerate = true; }
}
