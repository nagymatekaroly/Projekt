using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class PublicHighscoreData
{
    public string levelName;
    public int highscoreValue;
    public string username;
}

public class HighscoreUIManager : MonoBehaviour
{
    [SerializeField] private Text highscoreText;

    private string apiUrl = "https://mudskipdb.onrender.com/api/Highscore/by-level";

    public void LoadPlayerScore(string levelName)
    {
        StartCoroutine(GetScoreForLevel(levelName));
    }

    IEnumerator GetScoreForLevel(string levelName)
    {
        string url = apiUrl + "?levelName=" + UnityWebRequest.EscapeURL(levelName);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Hiba történt: " + request.error);
                highscoreText.text = "Nem elérhetõ";
            }
            else
            {
                string json = request.downloadHandler.text;

                // Ha üres a JSON, nem teljesítette a pályát
                if (string.IsNullOrWhiteSpace(json) || json == "{}")
                {
                    highscoreText.text = "Ezt a pályát még nem teljesítetted.";
                }
                else
                {
                    try
                    {
                        var data = JsonUtility.FromJson<PublicHighscoreData>(json);
                        highscoreText.text = $"{data.levelName} - {data.highscoreValue} pont";
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("JSON feldolgozási hiba: " + e.Message);
                        highscoreText.text = "Hibás adat";
                    }
                }
            }
        }
    }
}
