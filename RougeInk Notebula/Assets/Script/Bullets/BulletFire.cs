using System;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    [Header("Bullet Data")]
    [SerializeField] float bulletSpeed = 1500f;
    [SerializeField] public int _totalShots = 1;
    [SerializeField] public int bulletDamage;

    [Header("Bullet Transforms")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject[] bulltetSpawn;

    GameObject bulletParent; // Parent for the bullet clones to go in the Hierarchy
    private void Awake()
    {
        bulletParent = GameObject.Find("BulletParent"); // finding the Parent in the Hierarchy
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AmoutInstantiateBullet();
        }
    }

    public void BulletDirection(GameObject bullet ,Vector2 direction)
    {
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * bulletSpeed * Time.fixedDeltaTime;
    }

    private void InstantiateBullet(int index , Vector2 direction)
    {
        // Calculate the rotation from the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle)); 

        GameObject bulletClone = Instantiate(bulletPrefab, bulltetSpawn[index].transform.position, rotation, bulletParent.transform); // Dublicate the bullet 
        BulletCollision bulletCollision = bulletClone.GetComponent<BulletCollision>();
        if (bulletCollision != null)
        {
            bulletCollision.bulletCollisionDamage = this.bulletDamage; // Let bulletCollision know his current damage
        }
        BulletDirection(bulletClone, direction);
    }

    public void ChangeBullet(GameObject newBulletPrefab)
    {
        int tempDamage = bulletDamage;
        int doubleDamge = bulletDamage + 1;
        if (!UpgradeMenu.Instance.isDoubleShot)
        {
            bulletPrefab = newBulletPrefab;
            bulletDamage = tempDamage;
        }
        else if (UpgradeMenu.Instance.isDoubleShot)
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
