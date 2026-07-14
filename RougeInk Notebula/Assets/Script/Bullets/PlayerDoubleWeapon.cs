using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoubleWeapon : MonoBehaviour, IPausable
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulltetSpawn;
    [SerializeField] float bulletSpeed = 1500f;
    GameObject bulletParent; // Parent for the bullet clones to go in the Hierarchy

    private void Awake()
    {
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
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            InstantiateBullet();
            GetComponent<SquashAndStretch>()?.Squash();
        }
    }

    private void InstantiateBullet()
    {
        float bulletSpawnX = bulltetSpawn.transform.position.x;
        float bulletSpawnY = bulltetSpawn.transform.position.y;
        Vector2 bulletSpawnVector = new Vector2(bulletSpawnX, bulletSpawnY);
        
        Vector2 rightOffset = new Vector2(0.3f, 0);
        Vector2 leftOffset = new Vector2(-0.3f, 0);

        GameObject bulletRight = Instantiate(bullet, bulletSpawnVector + rightOffset, Quaternion.identity, bulletParent.transform); 
        GameObject bulletLeft = Instantiate(bullet, bulletSpawnVector + leftOffset, Quaternion.identity, bulletParent.transform); 
        
        SoundManager.Instance.PlaySound(Sounds.ShootingSound);
        
        Rigidbody2D rbRight = bulletRight.GetComponent<Rigidbody2D>();
        if (rbRight != null) rbRight.linearVelocity = Vector2.up * bulletSpeed * Time.fixedDeltaTime;
        
        Rigidbody2D rbLeft = bulletLeft.GetComponent<Rigidbody2D>();
        if (rbLeft != null) rbLeft.linearVelocity = Vector2.up * bulletSpeed * Time.fixedDeltaTime;
    }
}
