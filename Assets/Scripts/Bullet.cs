using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.GetDamage(damage);
            Destroy(gameObject);
        }
    }
}