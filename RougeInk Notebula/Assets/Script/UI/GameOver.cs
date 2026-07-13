using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int gameOverScore;
    void Start()
    {
        gameOverScore = PlayerPrefs.GetInt("WaveDied");
        scoreText.text = gameOverScore.ToString();
    }

    public void StartGameScene()
    {
        SceneManager.LoadScene(1);
    }

}
