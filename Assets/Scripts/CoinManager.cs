using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coinCount = 0;
    public Text coinText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("⚠️ Dupla CoinManager detektálva. Régi példány törölve.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("✅ CoinManager beállítva.");

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        SetupText();
        UpdateText();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("🔁 Pálya betöltve: " + scene.name);

        // Minden pályabetöltéskor nullázzuk
        coinCount = 0;

        // Új UI-ra rákötés
        SetupText();
        UpdateText();

        Debug.Log("🔄 Coin resetelve, text frissítve.");
    }

    void SetupText()
    {
        if (coinText == null)
        {
            GameObject textObj = GameObject.Find("CoinCount");
            if (textObj != null)
            {
                coinText = textObj.GetComponent<Text>();
                Debug.Log("✅ CoinText megtalálva.");
            }
        }
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

    public void ResetCoins()
    {
        coinCount = 0;
        UpdateText();
        Debug.Log("🧹 CoinManager: Pontszám nullázva.");
    }
}
