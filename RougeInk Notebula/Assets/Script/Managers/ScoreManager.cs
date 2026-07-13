using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText; // For using TMP text in the inspector
    [SerializeField] TextMeshProUGUI textPlayerHP; 
    [SerializeField] TextMeshProUGUI textAlivedEnemies; 
    public int waveCouter;
    private int playerHP;

    private void Start()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnPlayerHPInit += InItPlayerHp;
            GameEventManager.Instance.OnPlayerHPChanged += TheCurrectPlayerHP;
            GameEventManager.Instance.OnWaveChanged += CurrentWaveText;
            GameEventManager.Instance.OnWaveEnemiesChanged += WaveAlivedEnemies;
        }
    }

    private void OnDestroy()
    {
        if (GameEventManager.Instance != null)
        {
            GameEventManager.Instance.OnPlayerHPInit -= InItPlayerHp;
            GameEventManager.Instance.OnPlayerHPChanged -= TheCurrectPlayerHP;
            GameEventManager.Instance.OnWaveChanged -= CurrentWaveText;
            GameEventManager.Instance.OnWaveEnemiesChanged -= WaveAlivedEnemies;
        }
    }
    public void TheCurrectPlayerHP(int value)
    {
        playerHP -= value;
        PlayerHPText(playerHP.ToString());
    }
    public void CurrentWaveText(int waveNumberText)
    {
        waveText.text = waveNumberText.ToString();
    }

    public void WaveAlivedEnemies(int alivedEnemiesText)
    {
        textAlivedEnemies.text = alivedEnemiesText.ToString();
    }
    private void PlayerHPText(string scoreText)
    {
        textPlayerHP.text = scoreText;
    }
    public void InItPlayerHp(int playerHP)
    {
        this.playerHP = playerHP;
        PlayerHPText(playerHP.ToString());
    }
}
