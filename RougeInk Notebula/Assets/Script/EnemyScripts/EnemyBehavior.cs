using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    [Header("Database")]
    [SerializeField] GameDatabase gameDatabase;

    [Header("Editable Data")]
    private int enemyHealthPoint;
    private float speed;
    private float shootingInRange;
    private float fireRate = 1f;
    private float nextFireTime;
    private float maxLineOfSite = 5f;
    private float knockbackForce = 5f;

    [Header("Movement Options")]
    public bool useWobble = true;
    public float wobbleFrequency = 5f;
    public float wobbleAmplitude = 0.5f;

    [Header("Visual Effects")]
    public GameObject inkSplashPrefab;
    public GameObject collisionVfxPrefab;

    [Header("Game Objects")]
    [SerializeField] GameObject bulletSpawn;
    public GameObject bullet;
    GameObject _bulletParentHierarchy;
    private Transform _player;

    [Header("Others")]
    public bool _isDead = false;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _velocity;
    private float _knockbackRecoveryTime;
    private float _orbitDirection = 1f;
    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        if (gameDatabase != null)
        {
            enemyHealthPoint = gameDatabase.enemyHealthPoint;
            speed = gameDatabase.enemySpeed;
            shootingInRange = gameDatabase.enemyShootingInRange;
            fireRate = gameDatabase.enemyFireRate;
            maxLineOfSite = gameDatabase.enemyMaxLineOfSite;
            knockbackForce = gameDatabase.enemyKnockbackForce;
        }

        fireRate = Mathf.Max(0.1f, fireRate);

        if (_velocity == null) _velocity = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();

        _bulletParentHierarchy = GameObject.Find("BulletParent");
    }
    void Start()
    {
        _player = GameEventManager.OnRequestPlayerTransform?.Invoke();
        _orbitDirection = Random.value > 0.5f ? 1f : -1f;
        
        Shader flashShader = Shader.Find("Custom/SpriteFlash");
        if (flashShader != null)
        {
            _spriteRenderer.material = new Material(flashShader);
        }
        else
        {
            Debug.LogError("Custom/SpriteFlash shader not found! Make sure it compiled.");
        }
        
        _mpb = new MaterialPropertyBlock();
    }
    void Update()
    {
        if (!_isDead)
        {
            EnemyMovement();
        }
        if (_velocity == null)
        {
            Debug.LogError("Rigidbody2D not found on " + gameObject.name);
        }
    }

    private void EnemyMovement()
    {
        if (_isDead || _player == null) return;
        if (Time.time < _knockbackRecoveryTime) return;

        // Snappy stop: kill lingering momentum after the stun finishes
        if (_velocity.linearVelocity.sqrMagnitude > 0.1f)
        {
            _velocity.linearVelocity = Vector2.zero;
        }

        float distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);

        if (distanceFromPlayer > shootingInRange)
        {
            // Move towards the player
            Vector3 targetPosition = _player.position;
            if (useWobble)
            {
                Vector3 direction = (_player.position - transform.position).normalized;
                Vector3 perp = new Vector3(-direction.y, direction.x, 0);
                targetPosition += perp * Mathf.Sin(Time.time * wobbleFrequency) * wobbleAmplitude;
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingInRange)
        {
            // Calculate current angle relative to the player
            float angle = Mathf.Atan2(transform.position.y - _player.position.y, transform.position.x - _player.position.x);
            
            // Increment angle to create orbit (angular velocity = speed * 1.5f / radius for aggressive swarming)
            float angularSpeed = (speed * 1.5f) / shootingInRange;
            angle += angularSpeed * _orbitDirection * Time.deltaTime;
            
            // Project the new target position on the circle edge
            Vector3 targetPosition = _player.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * shootingInRange;
            
            // Move smoothly along the orbital path
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Dynamically rotate to face the player while orbiting (negative because sprites face down)
            transform.up = Vector3.Lerp(transform.up, -( _player.position - transform.position).normalized, 5f * Time.deltaTime);
        }
        if (!_isDead && distanceFromPlayer <= shootingInRange && nextFireTime < Time.time)
        {
            PoolManager.Instance.SpawnFromPool(bullet.name, bullet, bulletSpawn.transform.position, Quaternion.identity, _bulletParentHierarchy.transform); // Need to change position to the BulletSpawn object
            SoundManager.Instance.PlaySound(Sounds.ShootingSound);
            nextFireTime = Time.time + fireRate;
        }
    }   

    private void TeleportNearPlayer(bool toFarFromPlayer)
    {
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad; // Random angle in radians
        float radius = Random.Range(1f, 3f); // Random radius between 1 and 3 units

        Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;
        transform.position = _player.position + (Vector3)offset;
    }

    public void TakeDamage(int damage , Collider2D bulletCollision)
    {
        enemyHealthPoint -= damage;
        if (enemyHealthPoint <= 0) 
        {
            EnemyDied(bulletCollision);
        }
        else
        {
            StartCoroutine(FlashColor(Color.white));
        }
    }

    private void SpawnInkSplash()
    {
        if (inkSplashPrefab == null)
        {
#if UNITY_EDITOR
            inkSplashPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/InkSplash.prefab");
#endif
        }
        
        if (inkSplashPrefab != null)
        {
            // Try to use PoolManager if the prefab is registered, otherwise Instantiate directly
            GameObject splash = PoolManager.Instance.SpawnFromPool(inkSplashPrefab.name, inkSplashPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = splash.GetComponent<ParticleSystem>();
            if (ps != null && _spriteRenderer != null)
            {
                var main = ps.main;
                main.startColor = _spriteRenderer.color;
            }

        }
    }

    public void EnemyDied(Collider2D bulletCollision)
    {
        if (enemyHealthPoint <= 0)
        {
            _isDead = true;
            gameObject.tag = "Died Enemy";
            SpawnInkSplash();
            //Debug.Log($"game object name : {bulletCollision.gameObject.name}");
            Vector2 knockbackDirection = (transform.position - bulletCollision.transform.position).normalized;
            _velocity.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // nuke doesnt do any collisionObject to the force so that become null
            _animator.SetTrigger("IsFalling"); //Need diff diying animtion
            SoundManager.Instance.PlaySound(Sounds.EnemyDiedSound); //Need diff diying sound ?
            StartCoroutine(DestroyEnemy());
        }
        else
        {
            Debug.LogError("No Bullet Collision Found");
        }
    }
    IEnumerator FlashColor(Color color)
    {
        if (_mpb == null) yield break;

        // Pull current block, set flash amount to max, push it back
        _spriteRenderer.GetPropertyBlock(_mpb);
        _mpb.SetColor("_FlashColor", color);
        _mpb.SetFloat("_FlashAmount", 1f);
        _spriteRenderer.SetPropertyBlock(_mpb);

        yield return new WaitForSeconds(0.1f);

        // Turn flash off
        _spriteRenderer.GetPropertyBlock(_mpb);
        _mpb.SetFloat("_FlashAmount", 0f);
        _spriteRenderer.SetPropertyBlock(_mpb);
    }

    IEnumerator DestroyEnemy() // makes sure enemies dying if somhow they skipped the Trigger collison
    {
        yield return new WaitForSeconds(5f);
        PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
        Debug.Log("EnemyDied With DestroyEnemy method");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, maxLineOfSite);
        Gizmos.DrawWireSphere(this.transform.position, shootingInRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Died Enemy"))
        {
            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            if (bounceDirection == Vector2.zero) bounceDirection = Random.insideUnitCircle.normalized;
            
            _velocity.linearVelocity = Vector2.zero; // Reset velocity before adding force
            _velocity.AddForce(bounceDirection * knockbackForce, ForceMode2D.Impulse);
            _knockbackRecoveryTime = Time.time + 0.15f; // Quicker stun

            if (collisionVfxPrefab == null)
            {
#if UNITY_EDITOR
                collisionVfxPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/CollisionVFX.prefab");
#endif
            }
            if (collisionVfxPrefab != null)
            {
                PoolManager.Instance.SpawnFromPool(collisionVfxPrefab.name, collisionVfxPrefab, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }
}
