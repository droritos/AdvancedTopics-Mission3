using UnityEngine;

public class BulletPiercingCollision : MonoBehaviour
{
    public int bulletCollisionDamage;
    BossManager bossHited;
    public void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("Enemy"))
        {
            EnemyBehavior enemyHited = collisionObject.GetComponentInParent<EnemyBehavior>();
            enemyHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
        }
        else if (collisionObject.CompareTag("Boss"))
        {
            bossHited = collisionObject.GetComponentInParent<BossManager>();
            bossHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
        }
        else if (collisionObject.CompareTag("Boss") && collisionObject.CompareTag("Enemy") && collisionObject.CompareTag("Died Enemy")) // Error Checker
        {
            Debug.LogError($"Undifined tag : {collisionObject.gameObject.tag}");
        }
    }
}
