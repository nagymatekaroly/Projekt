using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    public CoinManager coinManager;
    public Text feedbackText;
    public GameObject finishPanel;

    private bool hasFinished = false;

    void Awake()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (!(currentScene.Contains("Level") || currentScene == "Tutorial"))
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        Debug.Log("CoinManager állapot a FinishPointban: " + (coinManager != null ? "OK" : "NULL"));
        if (coinManager != null)
            Debug.Log("Aktuális pont: " + coinManager.coinCount);

        hasFinished = false; // ✅ Reseteljük újraindításkor
        StartCoroutine(FindCoinManagerAfterDelay());
    }

    IEnumerator FindCoinManagerAfterDelay()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.Contains("Level") || currentScene == "Tutorial")
        {
            yield return new WaitForSeconds(0.2f);

            coinManager = FindObjectOfType<CoinManager>();

            if (coinManager == null)
            {
                Debug.LogError("CoinManager továbbra is NULL a FinishPointban!");
            }
            else
            {
                Debug.Log("CoinManager megtalálva a FinishPoint által.");

                if (coinManager.coinText == null)
                {
                    GameObject found = GameObject.Find("CoinCount");
                    if (found != null)
                    {
                        coinManager.coinText = found.GetComponent<Text>();
                        Debug.Log("CoinText automatikusan megtalálva a FinishPoint által.");
                    }
                    else
                    {
                        Debug.LogWarning("CoinText nem található!");
                    }
                }

                coinManager.coinText.text = "High Score: " + coinManager.coinCount.ToString();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasFinished) return; // ✅ Ne fusson le többször

        if (collision.CompareTag("Player"))
        {
            hasFinished = true; // ✅ Csak egyszer aktiválódjon

            if (coinManager == null)
            {
                coinManager = FindObjectOfType<CoinManager>();
                Debug.Log("CoinManager újra lekérve triggerkor.");
            }

            int currentScore = (coinManager != null) ? coinManager.coinCount : -1;

            Debug.Log("CoinManager állapota: " + (coinManager != null ? "OK" : "NULL"));
            Debug.Log("Finish elérve – Aktuális pont: " + currentScore);

            if (finishPanel != null)
            {
                finishPanel.SetActive(true);

                if (feedbackText != null)
                {
                    if (currentScore >= 0)
                    {
                        feedbackText.text = $"Gratulálok!\nPontszámod: {currentScore}";
                    }
                    else
                    {
                        feedbackText.text = $"Nem sikerült betölteni a pontszámot!";
                    }
                }
            }

            StartCoroutine(CheckAndSubmitHighScore());
        }
    }

    void DisablePlayerMovement()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var movement = player.GetComponent<Move>();
            if (movement != null)
            {
                movement.enabled = false;
                Debug.Log("Játékos mozgás letiltva.");
            }

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
                Debug.Log("🧊 Rigidbody2D letiltva (Static).");
            }
        }
    }

    IEnumerator CheckAndSubmitHighScore()
    {
        if (coinManager == null)
        {
            coinManager = FindObjectOfType<CoinManager>();
            if (coinManager == null)
            {
                Debug.LogError("CoinManager nem található még mindig – highscore megszakítva.");
                yield break;
            }
        }

        string levelName = SceneManager.GetActiveScene().name;
        int currentScore = coinManager != null ? coinManager.coinCount : 0;
        int previousScore = -1;

        string getUrl = "https://mudskipdb.onrender.com/api/Highscore/by-level?levelName=" + levelName;
        UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
        yield return getRequest.SendWebRequest();

        if (getRequest.result == UnityWebRequest.Result.Success)
        {
            string json = getRequest.downloadHandler.text;
            if (!string.IsNullOrEmpty(json) && json != "null")
            {
                HighscoreCheckDto dto = JsonUtility.FromJson<HighscoreCheckDto>(json);
                previousScore = dto.highscoreValue;
                Debug.Log("Előző pontszám: " + previousScore);
            }
            else
            {
                Debug.Log("Még nincs highscore ehhez a pályához.");
            }
        }
        else
        {
            Debug.LogWarning("Hiba a korábbi highscore lekérésénél: " + getRequest.error);
        }

        StartCoroutine(SubmitHighScore(currentScore, previousScore));
    }

    IEnumerator SubmitHighScore(int currentScore, int previousScore)
    {
        string url = "https://mudskipdb.onrender.com/api/Highscore";

        if (coinManager == null)
        {
            Debug.LogError("CoinManager nincs beállítva a FinishPointban!");
            yield break;
        }

        string levelName = SceneManager.GetActiveScene().name;

        HighscorePostDto postData = new HighscorePostDto
        {
            levelName = levelName,
            highscoreValue = currentScore
        };

        string jsonData = JsonUtility.ToJson(postData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Highscore elküldve! Válasz: " + request.downloadHandler.text);

            if (finishPanel != null && feedbackText != null)
            {
                feedbackText.text = "Szép munka!\nPontszámod: " + currentScore;
                if (previousScore >= 0 && currentScore > previousScore)
                {
                    feedbackText.text += "\nÚJ REKORD! Te vagy a király ezen a pályán!";
                }
                else if (previousScore == -1)
                {
                    feedbackText.text += "\nEz az első highscore ezen a pályán!";
                }
            }
        }
        else
        {
            Debug.LogError("Highscore küldés hiba: " + request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }

    public void OnNextLevelButtonPressed()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentIndex == 11)
        {
            SceneManager.LoadScene(3);
        }
        else if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            Debug.Log("Nincs több pálya, vissza a menübe.");
            SceneManager.LoadScene("LevelSelectScene");
        }
    }

    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    [System.Serializable]
    public class HighscorePostDto
    {
        public string levelName;
        public int highscoreValue;
    }

    [System.Serializable]
    public class HighscoreCheckDto
    {
        public string username;
        public int highscoreValue;
        public string levelName;
    }
}
