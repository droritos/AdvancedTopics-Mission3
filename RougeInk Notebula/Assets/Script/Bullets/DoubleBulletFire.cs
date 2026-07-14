using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBulletFire : MonoBehaviour, IPausable
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
        GameObject bulletRight = Instantiate(bullet, bulletSpawnVector, Quaternion.identity, bulletParent.transform); // Dublicate the bullet 
        GameObject bulletLeft = Instantiate(bullet, bulletSpawnVector, Quaternion.identity, bulletParent.transform); // Dublicate the bullet 
        SoundManager.Instance.PlaySound(Sounds.ShootingSound);
        bulletRight.GetComponent<Rigidbody2D>();
        bulletLeft.GetComponent<Rigidbody2D>();
        GetComponent<Rigidbody>().linearVelocity = Vector2.up * bulletSpeed * Time.fixedDeltaTime;
    }
}
