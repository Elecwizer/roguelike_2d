using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject _PauseMenu;
    
    public void Pause()
    {
        _PauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance.PlayerController._isPaused = true;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
        GameManager.Instance.PlayerController._isPaused = false;
    }

    public void Resume()
    {
        _PauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.Instance.PlayerController._isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        GameManager.Instance.PlayerController._isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
