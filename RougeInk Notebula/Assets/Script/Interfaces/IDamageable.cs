using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int damage, Collider2D collisionObject);
}
