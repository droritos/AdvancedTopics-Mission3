using UnityEngine;

public class EnemyFowardBullet : BaseBullet
{
    public float speed = 10f;

    protected override void Start()
    {
        base.Start(); // Subscribes to the Pause event
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (_rb == null) _rb = GetComponentInChildren<Rigidbody2D>();
        if (_rb == null) 
        {
            _rb = gameObject.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
        }
        
        _rb.linearVelocity = transform.up * speed;
    }
}