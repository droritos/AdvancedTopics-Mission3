using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Animator deadANM;
    [SerializeField] private Collider2D _collider2D;
    public int maxPlayerHealthPoint = 50;
    public int currentPlayerHealthPoint {  get;  set; }
    private bool isAlive = true;

     
    // Start is called before the first frame update
    void Start()
    {
        if (_collider2D == null) _collider2D = GetComponent<Collider2D>();
        if (deadANM == null) deadANM = GetComponent<Animator>();
        currentPlayerHealthPoint = maxPlayerHealthPoint;
    }

    // Update is called once per frame
    void Update()
    {
        DeadPlayer();
        GameEventManager.Instance?.TriggerPlayerHPInit(currentPlayerHealthPoint);
    }

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("EnemyBullet") || collisionObject.CompareTag("BossBullet"))
        {
            PoolManager.Instance.ReturnToPool(collisionObject.gameObject.name, collisionObject.gameObject);
            currentPlayerHealthPoint--;
            GameEventManager.Instance?.TriggerPlayerHPChanged(1);
            if (GameEventManager.Instance != null) GameEventManager.Instance.TriggerImpactOccurred(); // Juice on hit!
        }
    }
    public void ResetPlayerHealthOnDemand() // Reset the playerHealth when hit the buttton
    {
        currentPlayerHealthPoint = maxPlayerHealthPoint;
        Debug.Log("Health Restored");
    }

    private void DeadPlayer()
    {
        if (currentPlayerHealthPoint <= 0)
        {
            currentPlayerHealthPoint = 0;
            if (_collider2D != null) _collider2D.enabled = false;
            deadANM.SetTrigger("IsDead");
            isAlive = false;
            Debug.Log($"Player Died | isAlive = {this.isAlive}");
            StartCoroutine(WaitTillPlayerIsDead());
        }
    }
    private IEnumerator WaitTillPlayerIsDead()
    {
        if (!isAlive)
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(2);
        }
    }
}
