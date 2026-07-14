using UnityEngine;

public class BulletPiercingCollision : MonoBehaviour
{
    public int bulletCollisionDamage;
    [SerializeField] private Collider2D _bulletCollider;

    private void OnValidate()
    {
        if (_bulletCollider == null) _bulletCollider = GetComponent<Collider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("Died Enemy")) return;

        if (collisionObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(bulletCollisionDamage, _bulletCollider);
        }
    }
}
