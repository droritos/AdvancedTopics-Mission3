using System.Collections;
using UnityEngine;

public class BossLevelTwo : MonoBehaviour
{
    [Header("Editable Data")]
    [SerializeField] float speed;
    [SerializeField] float shootingInRange;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float maxLineOfSite = 5f;
    [SerializeField] float zigzagAmplitude = 2f; // Amplitude of the zigzag movement
    [SerializeField] float zigzagFrequency = 1f; // Frequency of the zigzag movement

    [Header("In Game Objects")]
    [SerializeField] GameObject[] bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject specialBulletPrefab;
    GameObject _bulletParentHierarchy;
    private Transform _player;

    [Header("Private Data")]
    private float _nextFireTime;
    private Collider2D[] _bulletColliderByPlayer;
    private float _zigzagTimer;
    private BossManager _bossManager;
    private Transform _parentTransform; // Parent transform to handle rotation and movement

    private void Awake()
    {
        _bulletParentHierarchy = GameObject.Find("BulletParent");
        _bossManager = GetComponent<BossManager>();
        FindColliders2D();

        // Assuming this script is on the child object, get the parent's transform
        _parentTransform = transform.parent;
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _zigzagTimer = 0f;
    }

    void Update()
    {
        if (!_bossManager.isDead)
        {
            HandleMovement();
            HandleShooting();
            SpinningMovement();
        }
    }

    private void HandleMovement()
    {
        float distanceToPlayer = Vector2.Distance(_parentTransform.position, _player.position);

        if (distanceToPlayer <= maxLineOfSite && _player != null)
        {
            // Zigzag movement calculation
            _zigzagTimer += Time.deltaTime * zigzagFrequency;
            float xOffset = Mathf.Sin(_zigzagTimer) * zigzagAmplitude;

            // Move towards the player on the y-axis and zigzag on the x-axis
            Vector3 targetPosition = new Vector3(_parentTransform.position.x + xOffset, _player.position.y, _parentTransform.position.z);

            // Can Use This Instaed : Vector3 direction = (targetPosition - _parentTransform.position).normalized;
            _parentTransform.position = Vector2.MoveTowards(_parentTransform.position, targetPosition, speed * Time.deltaTime);
        }
    }

    void SpinningMovement()
    {
        // Spin on the z-axis
        float rotationSpeed = 150f; // Degrees per second
        _parentTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        float distanceToPlayer = Vector2.Distance(_parentTransform.position, _player.position);
        if (distanceToPlayer <= shootingInRange && Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        bool isLowHealthPoint = false;
        if (_bossManager.bossCurrentHp <= (int)(_bossManager.bossMaxHealthPoint / 1.5))
        {
            isLowHealthPoint = true;
        }
        if (!isLowHealthPoint)
        {
            foreach (var spawnPoint in bulletSpawn) // First phase
            {
                Instantiate(bulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, _bulletParentHierarchy.transform);
            }
        }
        else if (isLowHealthPoint)
        {
            fireRate = 0.1f;
            foreach (var spawnPoint in bulletSpawn) // Second phase
            {
                Instantiate(specialBulletPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation, _bulletParentHierarchy.transform);
            }
        }
    }

    public void FindColliders2D()
    {
        Transform collisionPointParent = transform.Find("CollisionPoints");
        _bulletColliderByPlayer = new Collider2D[collisionPointParent.childCount];
        for (int i = 0; i < collisionPointParent.childCount; i++)
            _bulletColliderByPlayer[i] = collisionPointParent.GetChild(i).GetComponent<Collider2D>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, maxLineOfSite);
        Gizmos.DrawWireSphere(this.transform.position, shootingInRange);
    }

    private void OnDestroy()
    {
        GetComponent<BossManager>().bossMaxHealthPoint *= 2;
    }
}
