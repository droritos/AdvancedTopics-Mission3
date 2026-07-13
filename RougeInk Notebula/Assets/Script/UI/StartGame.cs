using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    public void StartGameScene()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGameMM()
    {
        Application.Quit();
    }
}
