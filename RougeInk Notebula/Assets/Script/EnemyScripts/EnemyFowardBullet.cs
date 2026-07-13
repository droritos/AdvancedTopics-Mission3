using UnityEngine;

public class EnemyFowardBullet : MonoBehaviour
{
    public float speed = 10f;
    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        rb.linearVelocity = transform.up * speed;
    }
}