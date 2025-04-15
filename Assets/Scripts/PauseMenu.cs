using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartLevel()
    {
        Debug.Log("🔁 Újrapróbálkozás – CoinManager resetelése");
        if (CoinManager.instance != null)
        {
            CoinManager.instance.ResetCoins();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;

        if (CoinManager.instance != null)
        {
            Destroy(CoinManager.instance.gameObject);
            CoinManager.instance = null;
            Debug.Log("🧹 CoinManager törölve a menübe lépéskor.");
        }

        SceneManager.LoadScene("LevelSelectScene");
    }


    public void QuitGame()
    {
        Debug.Log("Kilépés...");
        Application.Quit();
    }
}
