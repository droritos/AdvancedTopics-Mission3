using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText; // For using TMP text in the inspector
    [SerializeField] TextMeshProUGUI textPlayerHP; 
    [SerializeField] TextMeshProUGUI textAlivedEnemies; 
    public int waveCouter;
    private int playerHP;

    public static ScoreManager Instance;//The "Singleton Pattern" - Easy way to call the ScoreManger script from every where

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
