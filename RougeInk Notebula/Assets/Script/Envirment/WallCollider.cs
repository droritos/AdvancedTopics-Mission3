using UnityEngine;

public class WallCollider : MonoBehaviour
{
    //[SerializeField] Collider2D[] wall;
    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("EnemyBullet") || collisionObject.CompareTag("Died Enemy") || collisionObject.CompareTag("PlayerBullet") || collisionObject.CompareTag("BossBullet"))
        {
            PoolManager.Instance.ReturnToPool(collisionObject.gameObject.name, collisionObject.gameObject);
        }
    }
}
