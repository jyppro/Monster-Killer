using UnityEngine;

public interface IMonsterDamageable
{
    void TakeDamage_M(int damage);
    void ShowDamageText(int damage, Vector3 position);
}
