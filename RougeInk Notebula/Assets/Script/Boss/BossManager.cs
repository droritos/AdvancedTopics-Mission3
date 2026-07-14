using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour, IDamageable
{
    [Header("Boss Data")]
    [SerializeField] public int bossMaxHealthPoint;
    public int bossCurrentHp;
    [SerializeField] float knockbackForce = 8f;
    [HideInInspector] public bool isDead {  get; private set; }

    [Header("Boss Data")]
    [SerializeField] private Rigidbody2D _velocity;

    [Header("Visual Effects")]
    public GameObject inkSplashPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        bossCurrentHp = bossMaxHealthPoint;
    }

    public void TakeDamage(int damage, Collider2D collisionObject)
    {
        bossCurrentHp -= damage;
        SpawnInkSplash();
        if (bossCurrentHp <= 0)
            BossDied(collisionObject);
        else
            StartCoroutine(FlashColor(Color.red));
    }

    private void SpawnInkSplash()
    {
        if (inkSplashPrefab == null)
        {
#if UNITY_EDITOR
            inkSplashPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefab/InkSplash.prefab");
#endif
        }
        
        if (inkSplashPrefab != null)
        {
            GameObject splash = Instantiate(inkSplashPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = splash.GetComponent<ParticleSystem>();
            if (ps != null && _spriteRenderer != null)
            {
                var main = ps.main;
                main.startColor = _spriteRenderer.color;
            }
            Destroy(splash, 2f);
        }
    }
    public void BossDied(Collider2D collisionObject)
    {
        if (bossCurrentHp <= 0 && !isDead)
        {
            isDead = true;
            gameObject.tag = "Died Enemy";
            Vector2 knockbackDirection = (transform.position - collisionObject.transform.position).normalized;
            _velocity.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            _animator.SetTrigger("IsFalling");
            SoundManager.Instance.PlaySound(Sounds.EnemyDiedSound);
            GameEventManager.Instance?.TriggerShowUpgradeMenu();
        }
    }
    IEnumerator FlashColor(Color color)
    {
        Color originalColor = _spriteRenderer.material.color;
        _spriteRenderer.color = color;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = originalColor;
    }
}
