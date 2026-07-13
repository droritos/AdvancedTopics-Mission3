using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private bool isHit = false;
    [SerializeField] bool _isPiercingEnabled = false;
    [HideInInspector] public int bulletCollisionDamage;
    BossManager bossHited;

    public void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (bulletCollisionDamage <= 0)
        {
            bulletCollisionDamage = GameObject.FindWithTag("Player").GetComponent<BulletFire>().bulletDamage;
            Debug.Log($"bulletCollisionDamage not found used Player-Tag data {bulletCollisionDamage}");

        }
        if (!isHit && !_isPiercingEnabled && collisionObject.CompareTag("Enemy"))
        {
            EnemyBehavior enemyHited = collisionObject.GetComponentInParent<EnemyBehavior>();
            enemyHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
            Destroy(gameObject);
            isHit = true;
        }
        else if (!isHit && !_isPiercingEnabled && collisionObject.CompareTag("Boss"))
        {
            bossHited = collisionObject.GetComponentInParent<BossManager>();
            bossHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
            Destroy(gameObject);
            isHit = true;
        }
        else if (_isPiercingEnabled && collisionObject.CompareTag("Enemy"))
        {
            EnemyBehavior enemyHited = collisionObject.GetComponentInParent<EnemyBehavior>();
            enemyHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
        }
        else if (_isPiercingEnabled && collisionObject.CompareTag("Boss"))
        {
            bossHited = collisionObject.GetComponentInParent<BossManager>();
            bossHited.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
        }
        else if (collisionObject.CompareTag("Boss") && collisionObject.CompareTag("Enemy") && collisionObject.CompareTag("Died Enemy")) // Error Checker
        {
            Debug.LogError($"Undifined tag : {collisionObject.gameObject.tag}");
        }
    }
        
    public void IsPiercingBullets(bool isActive)
    {
        _isPiercingEnabled = isActive;
        Debug.Log($"Bullet Piercing is : {_isPiercingEnabled} , by {isActive}");
    }
}
