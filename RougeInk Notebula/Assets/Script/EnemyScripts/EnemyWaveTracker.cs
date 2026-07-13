using UnityEngine;

public class EnemyWaveTracker : MonoBehaviour
{
    void OnDisable()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }
    }
}