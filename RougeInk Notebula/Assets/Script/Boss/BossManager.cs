using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [Header("Boss Data")]
    [SerializeField] public int bossMaxHealthPoint;
    public int bossCurrentHp;
    [SerializeField] float knockbackForce = 8f;
    [HideInInspector] public bool isDead {  get; private set; }

    [Header("Boss Data")]
    [HideInInspector] public Rigidbody2D _velocity;
    private Animator _animator;
    private void Awake()
    {
        bossCurrentHp = bossMaxHealthPoint;
        _velocity = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage, Collider2D collisionObject)
    {
        bossCurrentHp -= damage;
        if (bossCurrentHp <= 0)
            BossDied(collisionObject);
        else
            StartCoroutine(FlashColor(Color.red));
    }
    public void BossDied(Collider2D collisionObject)
    {
        if (bossCurrentHp <= 0 && gameObject.tag == "Boss")
        {
            isDead = true;
            gameObject.tag = "Died Enemy";
            Vector2 knockbackDirection = (transform.position - collisionObject.transform.position).normalized;
            _velocity.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            _animator.SetTrigger("IsFalling");
            SoundManager.Instance.PlaySound(Sounds.EnemyDiedSound);
            UpgradeMenu.Instance.PopUpShow();
        }
    }
    IEnumerator FlashColor(Color color)
    {
        Color originalColor = GetComponent<SpriteRenderer>().material.color;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}
