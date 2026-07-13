using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject resumeMenuUI;
    [SerializeField] private GameObject soundOn;
    [SerializeField] private GameObject soundOff;
    // Update is called once per frame
    private void Awake()
    {
        soundOff.SetActive(true);
        soundOn.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        if (gameIsPaused)
        {
            resumeMenuUI.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }

    public void Pause()
    {
        if (!gameIsPaused)
        {
            resumeMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game! , Come backk!!!");
        Application.Quit();
    }

    public void SoundOn()
    {
        soundOff.SetActive(true);
        soundOn.SetActive(false);
        AudioListener.volume = 1;
        Debug.Log("Sound On");
    }

    public void SoundOff()
    {
        soundOn.SetActive(true);
        soundOff.SetActive(false);
        AudioListener.volume = 0;
        Debug.Log("Sound off");
    }
}
