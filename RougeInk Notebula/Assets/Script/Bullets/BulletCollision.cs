using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    private bool isHit = false;
    [SerializeField] bool _isPiercingEnabled = false;
    [SerializeField] private Collider2D _bulletCollider;
    private int bulletCollisionDamage;
    public int BulletCollisionDamage { get => bulletCollisionDamage; set => bulletCollisionDamage = value; }

    private void OnValidate()
    {
        if (_bulletCollider == null) _bulletCollider = GetComponent<Collider2D>();
    }

    void Awake()
    {
        if (_bulletCollider != null) _bulletCollider.isTrigger = true;
    }

    void OnEnable()
    {
        isHit = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (bulletCollisionDamage <= 0)
        {
            bulletCollisionDamage = GameEventManager.OnRequestPlayerTransform?.Invoke()?.GetComponent<PlayerWeapon>()?.BulletDamage ?? 0;
            Debug.Log($"bulletCollisionDamage not found used Player-Tag data {bulletCollisionDamage}");

        }
        
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            if (!_isPiercingEnabled && !isHit)
            {
                damageable.TakeDamage(bulletCollisionDamage, _bulletCollider);
                PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
                isHit = true;
            }
            else if (_isPiercingEnabled)
            {
                damageable.TakeDamage(bulletCollisionDamage, _bulletCollider);
            }
        }
    }
        
    public void IsPiercingBullets(bool isActive)
    {
        _isPiercingEnabled = isActive;
        Debug.Log($"Bullet Piercing is : {_isPiercingEnabled} , by {isActive}");
    }
}
