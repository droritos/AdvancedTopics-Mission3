using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Editable Data")]
    [SerializeField] int enemyHealthPoint;
    [SerializeField] float speed;
    [SerializeField] float shootingInRange;
    [SerializeField] float fireRate = 1f;
    [SerializeField] float nextFireTime;
    [SerializeField] float flankDistance = 2f;
    [SerializeField] float maxLineOfSite = 5f;
    [SerializeField] float knockbackForce = 5f;

    [Header("Game Objects")]
    [SerializeField] GameObject bulletSpawn;
    public GameObject bullet;
    GameObject _bulletParentHierarchy;
    private Transform _player;

    [Header("Others")]
    public bool _isDead = false;
    private bool _isFlanking = false;
    private Animator _animator;
    private Vector3 _flankPosition;
    private Rigidbody2D _velocity;

    private void Awake()
    {
        _bulletParentHierarchy = GameObject.Find("BulletParent");
        _velocity = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
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

        float distanceFromPlayer = Vector2.Distance(_player.transform.position, transform.position);

        if (distanceFromPlayer < maxLineOfSite && distanceFromPlayer > shootingInRange)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, _player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingInRange)
        {
            if (!_isFlanking)
            {
                // Calculate flank position: a point past the player in their current direction
                Vector3 directionToPlayer = (_player.position - transform.position).normalized;
                _flankPosition = _player.position + directionToPlayer * flankDistance;
                _isFlanking = true;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _flankPosition, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, _flankPosition) < 0.1f)
                {
                    // Reached flank position, consider stopping flanking or find a new flank position
                    _isFlanking = false;
                }
            }
        }
        if (!_isDead && distanceFromPlayer <= shootingInRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity, _bulletParentHierarchy.transform); // Need to change position to the BulletSpawn object
            SoundManager.Instance.PlaySound(Sounds.ShootingSound);
            _isFlanking = false; // Make him flank every shoot;
            nextFireTime = Time.time + fireRate;
        }
    }   

    private void TeleportNearPlayer(bool toFarFromPlayer)
    {
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad; // Random angle in radians
        float radius = Random.Range(1f, 3f); // Random radius between 1 and 3 units

        Vector2 offset = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * radius;
        transform.position = _player.position + (Vector3)offset;

        // Reset flanking state if needed
        _isFlanking = false;
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
            StartCoroutine(FlashColor(Color.red));
        }
    }

    public void EnemyDied(Collider2D bulletCollision)
    {
        if (enemyHealthPoint <= 0)
        {
            _isDead = true;
            gameObject.tag = "Died Enemy";
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
        Color originalColor = GetComponent<SpriteRenderer>().material.color;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = originalColor;
    }

    IEnumerator DestroyEnemy() // makes sure enemies dying if somhow they skipped the Trigger collison
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
        Debug.Log("EnemyDied With DestroyEnemy method");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, maxLineOfSite);
        Gizmos.DrawWireSphere(this.transform.position, shootingInRange);
    }
}
