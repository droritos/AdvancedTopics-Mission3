using System.Collections;
using UnityEngine;

public class BossLevelOne : MonoBehaviour
{
    [Header("Editable Data")]
    [SerializeField] float speed;
    [SerializeField] float shootingInRange;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float flankDistance = 2f;
    [SerializeField] float maxLineOfSite = 5f;

    [Header("In Game Objects")]
    [SerializeField] GameObject[] bulletSpawn;
    [SerializeField] GameObject bulletPrefab;
    GameObject _bulletParentHierarchy;
    private Transform _player;

    [Header("Private Data")]
    private bool _isFlanking = false;
    private Vector3 _flankPosition;
    private Rigidbody2D _velocity;
    private float _nextFireTime;
    private Collider2D[] _bulletColliderByPlayer;


    private void Awake()
    {
        _bulletParentHierarchy = GameObject.Find("BulletParent");
        _velocity = GetComponent<Rigidbody2D>();
        FindColliders2D();
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null)
        {
            if (_isFlanking)
                FlankMovement();
            else
                ApproachPlayer();
            AttackPlayer();
        }
    }
    void ApproachPlayer()
    {
        if (Vector2.Distance(transform.position, _player.position) > maxLineOfSite)
        {
            Vector2 moveDirection = (_player.position - transform.position).normalized;
            _velocity.MovePosition(_velocity.position + moveDirection * speed * Time.deltaTime);
        }
    }

    void FlankMovement()
    {
        // Randomly decide if flanking should continue or switch to approaching
        if (Random.value > 0.95) // 5% chance to stop flanking each frame
        {
            _isFlanking = false;
        }
        else if (Vector2.Distance(transform.position, _flankPosition) > 0.1f)
        {
            Vector2 moveDirection = (_flankPosition - transform.position).normalized;
            _velocity.MovePosition(_velocity.position + moveDirection * speed * Time.deltaTime);
        }
        else
        {
            _flankPosition = FindNewFlankPosition();
        }
    }
    void AttackPlayer()
    {
        if (Vector2.Distance(transform.position, _player.position) <= shootingInRange && Time.time > _nextFireTime)
        {
            foreach (var spawnPoint in bulletSpawn)
            {
                Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity, _bulletParentHierarchy.transform);
            }
            _nextFireTime = Time.time + fireRate;

            // Optionally, toggle flanking after an attack
            _isFlanking = !_isFlanking;
            if (_isFlanking)
            {
                _flankPosition = FindNewFlankPosition();
            }
        }
    }

    Vector3 FindNewFlankPosition()
    {
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float radius = flankDistance;
        return new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * radius + _player.position;
    }
    public void FindColliders2D() //I cant remember what it does but sonthing for the bullets
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
        Gizmos.DrawWireSphere(this.transform.position, flankDistance);
    }
}
