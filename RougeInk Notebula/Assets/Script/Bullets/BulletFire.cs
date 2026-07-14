using System;
using UnityEngine;

public class BulletFire : MonoBehaviour, IPausable
{
    [Header("Database")]
    [SerializeField] GameDatabase gameDatabase;

    [Header("Bullet Data")]
    private float bulletSpeed;
    private int _totalShots;
    private int bulletDamage;

    public int TotalShots { get => _totalShots; set => _totalShots = value; }
    public int BulletDamage { get => bulletDamage; set => bulletDamage = value; }

    [Header("Bullet Transforms")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] bulltetSpawn;

    GameObject bulletParent; // Parent for the bullet clones to go in the Hierarchy
    private void Awake()
    {
        if (gameDatabase != null)
        {
            bulletSpeed = gameDatabase.playerBulletSpeed;
            _totalShots = gameDatabase.playerTotalShots;
            bulletDamage = gameDatabase.playerBulletDamage;
        }
        bulletParent = GameObject.Find("BulletParent"); // finding the Parent in the Hierarchy
    }
    private bool _isPaused;
    public void SetPaused(bool isPaused) { _isPaused = isPaused; }

    private void Start()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused += SetPaused;
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
            GameEventManager.Instance.OnGamePaused -= SetPaused;
    }

    private void Update()
    {
        if (_isPaused) return;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AmoutInstantiateBullet();
            GetComponent<SquashAndStretch>()?.Squash();
        }
    }

    public void BulletDirection(GameObject bullet ,Vector2 direction)
    {
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.linearVelocity = direction * bulletSpeed * Time.fixedDeltaTime;
    }

    private void InstantiateBullet(int index , Vector2 direction)
    {
        // Calculate the rotation from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle)); 

        GameObject bulletClone = PoolManager.Instance.SpawnFromPool(bulletPrefab.name, bulletPrefab, bulltetSpawn[index].transform.position, rotation, bulletParent.transform);
        BulletCollision bulletCollision = bulletClone.GetComponent<BulletCollision>();
        if (bulletCollision != null)
        {
            bulletCollision.BulletCollisionDamage = this.bulletDamage; // Let bulletCollision know his current damage
        }
        BulletDirection(bulletClone, direction);
    }

    public void ChangeBullet(GameObject newBulletPrefab, bool isDoubleShot)
    {
        int tempDamage = bulletDamage;
        int doubleDamge = bulletDamage + 1;
        if (!isDoubleShot)
        {
            bulletPrefab = newBulletPrefab;
            bulletDamage = tempDamage;
        }
        else if (isDoubleShot)
        {
            bulletPrefab = newBulletPrefab;
            bulletDamage = doubleDamge;
        }
    }

    public void AmoutInstantiateBullet()
    {
        SoundManager.Instance.PlaySound(Sounds.ShootingSound);
        switch (_totalShots)
        {
            case 1:
                InstantiateBullet(0 , Vector2.up);
                break;
            case 2:
                InstantiateBullet(0 , Vector2.up);
                InstantiateBullet(1, Vector2.down);
                break;
            case 3:
                InstantiateBullet(0, Vector2.up);
                InstantiateBullet(1, Vector2.down);
                InstantiateBullet(2, Vector2.right);
                break;
            case 4:
                InstantiateBullet(0, Vector2.up);
                InstantiateBullet(1, Vector2.down);
                InstantiateBullet(2, Vector2.right);
                InstantiateBullet(3, Vector2.left);
                break;
            default:
                _totalShots = 4;
                break;
        }
    }
}
