public interface IDamageable
{
    void InstantiateHealthText(float damage);

    void GetDamage(float damage);

    void CheckIsDead();

    void MoveHealthBar();
}