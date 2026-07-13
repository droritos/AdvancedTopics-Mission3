using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    Animator deadANM;
    public int maxPlayerHealthPoint = 50;
    public int currentPlayerHealthPoint {  get;  set; }
    private bool isAlive = true;

     
    // Start is called before the first frame update
    void Start()
    {
        deadANM = GetComponent<Animator>();
        currentPlayerHealthPoint = maxPlayerHealthPoint;
    }

    // Update is called once per frame
    void Update()
    {
        DeadPlayer();
        ScoreManager.Instance.InItPlayerHp(currentPlayerHealthPoint);
    }

    private void OnTriggerEnter2D(Collider2D collisionObject)
    {
        if (collisionObject.CompareTag("EnemyBullet") || collisionObject.CompareTag("BossBullet"))
        {
            Destroy(collisionObject.gameObject);
            currentPlayerHealthPoint--;
            ScoreManager.Instance.TheCurrectPlayerHP(1);
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
            GetComponent<BoxCollider2D>().enabled = false;
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
