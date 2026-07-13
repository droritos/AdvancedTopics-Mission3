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

    public void TriggerPlayerHPInit(int hp) => OnPlayerHPInit?.Invoke(hp);
    public void TriggerPlayerHPChanged(int damage) => OnPlayerHPChanged?.Invoke(damage);
    public void TriggerWaveChanged(int wave) => OnWaveChanged?.Invoke(wave);
    public void TriggerWaveEnemiesChanged(int count) => OnWaveEnemiesChanged?.Invoke(count);
    public void TriggerShowUpgradeMenu() => OnShowUpgradeMenu?.Invoke();
    public void TriggerImpactOccurred() => OnImpactOccurred?.Invoke();
}
