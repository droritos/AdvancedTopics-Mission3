using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [Header("BulletData")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 0f;
    [SerializeField] int numberOfShrapnel = 8;
    Transform bulletParent;
    private void Start()
    {
        bulletParent = GameObject.Find("BulletParent").transform;
    }
    private void OnDestroy()
    {
        float angleStep = 360.0f / numberOfShrapnel;
        float angle = 0f;

        for (int i = 0; i < numberOfShrapnel; i++)
        {
            // Direction calculation
            float radian = angle * Mathf.Deg2Rad;
            Vector3 bulletDirection = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
            GameObject tempBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletParent.transform);
            Rigidbody2D tempBulletRigidbody = tempBullet.GetComponent<Rigidbody2D>();
            tempBulletRigidbody.linearVelocity = bulletDirection * bulletSpeed;

            // Calculate rotation to face the direction of the bullet
            float rotationAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            tempBullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);

            angle += angleStep;
        }
    }
}
