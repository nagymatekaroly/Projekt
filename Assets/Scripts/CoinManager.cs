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
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.StartsWith("Level") || sceneName == "Tutorial")
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("✅ CoinManager beállítva.");
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
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

        UpdateText(); // mindig frissítjük a szöveget
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
