using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount;
    public Text coinText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("✅ CoinManager beállítva.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Fontos: eltávolítjuk az eseményt, hogy ne legyen dupla hívás
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Csak akkor nullázunk, ha valóban pályaszintre lépünk
        if (scene.name.StartsWith("Level") || scene.name == "Tutorial")
        {
            coinCount = 0;
            UpdateText();
        }
    }

    void Start()
    {
        if (coinText == null)
        {
            GameObject found = GameObject.Find("CoinCount");
            if (found != null)
            {
                coinText = found.GetComponent<Text>();
                Debug.Log("✅ CoinText automatikusan megtalálva.");
            }
            else
            {
                Debug.LogWarning("⚠️ CoinText nem található!");
            }
        }

        UpdateText();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateText();
    }

    void UpdateText()
    {
        if (coinText != null)
        {
            coinText.text = "High Score: " + coinCount.ToString();
        }
    }
}
