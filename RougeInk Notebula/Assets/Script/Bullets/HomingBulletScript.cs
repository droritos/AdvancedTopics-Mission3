using UnityEngine;

public class HomingBulletScript : BaseBullet
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    private GameObject _target;

    private void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        
        _target = FindClosetsTarget();
        if (_target == null)
        {
            _rb.linearVelocity = transform.up * speed;
        }
    }

    void FixedUpdate()
    {
        if (_isPaused) return;

        if (_target != null && _target.activeInHierarchy && !_target.CompareTag("Died Enemy"))
        {
            // Steer towards target
            Vector2 direction = (Vector2)_target.transform.position - _rb.position;
            direction.Normalize();
            
            float rotateAmount = Vector3.Cross(direction, transform.up).z;
            _rb.angularVelocity = -rotateAmount * rotationSpeed * 100f;
            _rb.linearVelocity = transform.up * speed;
        }
        else
        {
            // Lost target or no target
            _rb.angularVelocity = 0f;
            _rb.linearVelocity = transform.up * speed;
            _target = FindClosetsTarget(); // Try to find a new target
        }
    }

    private GameObject FindClosetsTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        foreach (GameObject boss in bosses)
        {
            float distanceToBoss = Vector2.Distance(transform.position, boss.transform.position);
            if (distanceToBoss < shortestDistance)
            {
                shortestDistance = distanceToBoss;
                nearestEnemy = boss;
            }
        }
        
        return nearestEnemy;
    }
}
