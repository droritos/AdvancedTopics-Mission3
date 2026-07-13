using UnityEngine;

public class HomingBulletScript : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _rb;
    private Vector2 _initialTarget;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        GameObject target = FindClosetsTarget();
        if (target != null)
        {
            Transform enemy = target.transform;

            Vector2 direction = (enemy.position - transform.position).normalized;
            _initialTarget = transform.position + (Vector3)direction * 1000;  // Set a far away point in the direction of the enemy
            _rb.linearVelocity = direction * speed;

            float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (_rb.linearVelocity.magnitude < 0.1f)  // Optional: Check if the bullet has practically stopped
            Destroy(gameObject);
    }

    private GameObject FindClosetsTarget()
    {
        GameObject enemyGameObject = GameObject.FindGameObjectWithTag("Enemy");
        GameObject bossGameObject = GameObject.FindGameObjectWithTag("Boss");
        if (enemyGameObject != null)
            return enemyGameObject;
        else if (bossGameObject != null)
            return bossGameObject;
        else
            return null;
    }
}
