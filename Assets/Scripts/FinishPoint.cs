using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishPoint : MonoBehaviour
{
    public CoinManager coinManager;           // 🎯 Inspectorban húzd be!
    public Text feedbackText;                 // 🎉 UI Text a visszajelzéshez

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (feedbackText != null)
                feedbackText.text = "🎉 Congratulations!";

            Debug.Log("🎯 Finish elérve – Highscore küldés indul...");
            StartCoroutine(SubmitHighScore());
        }
    }

    IEnumerator SubmitHighScore()
    {
        string url = "https://mudskipdb.onrender.com/api/Highscore";

        if (coinManager == null)
        {
            Debug.LogError("❌ CoinManager nincs beállítva a FinishPointban!");
            yield break;
        }

        string levelName = SceneManager.GetActiveScene().name;

        HighscorePostDto postData = new HighscorePostDto
        {
            levelName = levelName,
            highscoreValue = coinManager.coinCount
        };

        string jsonData = JsonUtility.ToJson(postData);
        Debug.Log("📤 Highscore JSON: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            Debug.Log("✅ Highscore elküldve! Válasz: " + response);

            // 🔍 Visszajelzés szöveg – ha új rekord volt, ezt mondja
            if (response.Contains("saved"))
            {
                if (feedbackText != null)
                    feedbackText.text += "\n🏆 New Highscore!";
            }

            // 🔄 2 másodperc után vissza LevelSelectScene-re
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("LevelSelectScene");
        }
        else
        {
            Debug.LogError("❌ Highscore küldés hiba: " + request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }

    [System.Serializable]
    public class HighscorePostDto
    {
        public string levelName;
        public int highscoreValue;
    }
}
