using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private bool isHit = false;
    [SerializeField] bool _isPiercingEnabled = false;
    private int bulletCollisionDamage;
    public int BulletCollisionDamage { get => bulletCollisionDamage; set => bulletCollisionDamage = value; }

    void OnEnable()
    {
        isHit = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (bulletCollisionDamage <= 0)
        {
            bulletCollisionDamage = GameEventManager.OnRequestPlayerTransform?.Invoke()?.GetComponent<BulletFire>()?.BulletDamage ?? 0;
            Debug.Log($"bulletCollisionDamage not found used Player-Tag data {bulletCollisionDamage}");

        }
        
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            if (!_isPiercingEnabled && !isHit)
            {
                damageable.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
                PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
                isHit = true;
            }
            else if (_isPiercingEnabled)
            {
                damageable.TakeDamage(bulletCollisionDamage, this.gameObject.GetComponent<Collider2D>());
            }
        }
    }
        
    public void IsPiercingBullets(bool isActive)
    {
        _isPiercingEnabled = isActive;
        Debug.Log($"Bullet Piercing is : {_isPiercingEnabled} , by {isActive}");
    }
}
