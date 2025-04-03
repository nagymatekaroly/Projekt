using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerLoader : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (IsLevelScene(sceneName))
        {
            // Már van egy GameManager a jelenetben?
            CoinManager existingCoinManager = FindObjectOfType<CoinManager>();

            if (existingCoinManager == null)
            {
                GameObject gm = Instantiate(gameManagerPrefab);
                DontDestroyOnLoad(gm);
                Debug.Log("✅ GameManager (CoinManager-rel) instantiálva és DontDestroyOnLoad beállítva: " + sceneName);
            }
            else
            {
                Debug.Log("✅ CoinManager már létezik ebben a scene-ben: " + sceneName);
            }
        }
    }

    private bool IsLevelScene(string name)
    {
        return name == "Tutorial" ||
               name == "Level 1" ||
               name == "Level 2" ||
               name == "Level 3" ||
               name == "Level 4" ||
               name == "Level 5";
    }
}
