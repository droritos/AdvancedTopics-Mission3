using UnityEngine;

public class ParticalSystemTilling : MonoBehaviour
{
    [SerializeField] GameObject particalPrefab;
    private int _currentTriggerCount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _currentTriggerCount = 0;
        if (collision.CompareTag("Player"))
        {
            _currentTriggerCount++;
            Vector2 spawnPosition = GameObject.Find("Player").transform.position;
            Instantiate(particalPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Player has triggered the partical spawn & New particalPrefab instantiated.");
        }
        if (_currentTriggerCount >= 1)
            Destroy(particalPrefab);
    }

}
