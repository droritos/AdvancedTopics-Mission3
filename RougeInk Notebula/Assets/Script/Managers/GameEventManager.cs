using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static System.Func<Transform> OnRequestPlayerTransform;
    public event Action<int> OnPlayerHPInit;
    public event Action<int> OnPlayerHPChanged;
    public event Action<int> OnWaveChanged;
    public event Action<int> OnWaveEnemiesChanged;
    public event Action OnShowUpgradeMenu;
    public event Action OnImpactOccurred;
    public event Action<bool> OnGamePaused;

    public void TriggerPlayerHPInit(int hp) => OnPlayerHPInit?.Invoke(hp);
    public void TriggerPlayerHPChanged(int damage) => OnPlayerHPChanged?.Invoke(damage);
    public void TriggerWaveChanged(int wave) => OnWaveChanged?.Invoke(wave);
    public void TriggerWaveEnemiesChanged(int count) => OnWaveEnemiesChanged?.Invoke(count);
    public void TriggerShowUpgradeMenu() => OnShowUpgradeMenu?.Invoke();
    public void TriggerImpactOccurred() => OnImpactOccurred?.Invoke();
    public void TriggerGamePaused(bool isPaused) => OnGamePaused?.Invoke(isPaused);

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("CHEAT: Killing all enemies!");
            var enemies = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude);
            foreach (var enemy in enemies)
            {
                if (enemy is IDamageable damageable)
                {
                    // Don't kill the player accidentally!
                    if (!enemy.gameObject.CompareTag("Player"))
                    {
                        damageable.TakeDamage(9999, null);
                    }
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("CHEAT: Spawning Upgrade Menu!");
            TriggerShowUpgradeMenu();
        }
    }
}
