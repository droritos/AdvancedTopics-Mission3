using System;
using UnityEngine;

public class EnemyBulletScript : BaseBullet
{
    private GameObject player;
    public float force;

    void OnEnable()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        if (player == null) player = GameEventManager.OnRequestPlayerTransform?.Invoke()?.gameObject;

        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            _rb.linearVelocity = new Vector2 (direction.x, direction.y).normalized * force;

            float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }
}
