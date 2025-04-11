using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManagerLoader : MonoBehaviour
{
    public GameObject coinManagerPrefab;

    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (IsLevelScene(sceneName))
        {
            if (FindObjectOfType<CoinManager>() == null)
            {
                Instantiate(coinManagerPrefab);
                Debug.Log("✅ CoinManager betöltve ebben a scene-ben: " + sceneName);
            }
        }
        else
        {
            Debug.Log("ℹ️ Ez nem pálya scene, nem töltjük be a CoinManagert: " + sceneName);
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
