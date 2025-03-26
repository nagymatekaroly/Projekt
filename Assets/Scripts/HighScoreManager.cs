using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public string highScoreUrl = "https://mudskipdb.onrender.com/api/Highscore/my-highscores";

    public Transform highscoreContent;        // Ez legyen egy üres GameObject (pl. HighscoreContent)
    public GameObject highscoreRowPrefab;     // A sor prefabod (benne LevelText + ScoreText)

    [System.Serializable]
    public class HighScoreEntry
    {
        public string levelName;
        public int highscore;
    }

    [System.Serializable]
    public class HighScoreList
    {
        public List<HighScoreEntry> highscores;
    }

    // 🔥 Ezt hívja a GOMB
    public void LoadHighScoresButton()
    {
        StartCoroutine(LoadHighScores());
    }

    IEnumerator LoadHighScores()
    {
        // 🔥 MINDIG kezdjünk egy biztonságos takarítással!
        foreach (Transform child in highscoreContent)
        {
            Destroy(child.gameObject);
        }

        UnityWebRequest request = UnityWebRequest.Get(highScoreUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"highscores\":" + request.downloadHandler.text + "}";
            HighScoreList data = JsonUtility.FromJson<HighScoreList>(json);

            foreach (var entry in data.highscores)
            {
                // ✅ Sor spawn biztonságosan
                if (highscoreRowPrefab != null && highscoreContent != null)
                {
                    GameObject row = Instantiate(highscoreRowPrefab, highscoreContent);
                    Text levelText = row.transform.Find("LevelText").GetComponent<Text>();
                    Text scoreText = row.transform.Find("Score text").GetComponent<Text>();

                    if (levelText != null) levelText.text = entry.levelName;
                    if (scoreText != null) scoreText.text = entry.highscore.ToString();
                }
            }
        }
        else
        {
            Debug.LogError("❌ Failed to load highscores: " + request.error);
        }
    }
}
