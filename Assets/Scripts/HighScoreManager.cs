using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
    [Header("Saját highscore-ok UI")]
    public Transform myHighscoreParent;
    public GameObject myHighscoreRowPrefab;

    [Header("API URL")]
    public string baseUrl = "https://mudskipdb.onrender.com/api/Highscore";

    [System.Serializable]
    public class MyHighscoreDto
    {
        public string levelName;
        public int highscore;
    }

    [System.Serializable]
    public class MyHighscoreDtoList
    {
        public List<MyHighscoreDto> items;
    }

    public void OnFetchMyHighscoresButtonPressed()
    {
        Debug.Log("📥 FetchMyHighscores gomb megnyomva.");
        if (string.IsNullOrEmpty(baseUrl))
        {
            Debug.LogError("❌ baseUrl nincs beállítva!");
            return;
        }
        if (myHighscoreParent == null || myHighscoreRowPrefab == null)
        {
            Debug.LogError("❌ UI elemek nincsenek beállítva (parent vagy prefab)!");
            return;
        }

        Debug.Log("🔁 FetchMyHighscores coroutine indítása...");
        StartCoroutine(FetchMyHighscores());
    }

    IEnumerator FetchMyHighscores()
    {
        string url = $"{baseUrl}/my-highscores";
        Debug.Log($"🌐 Lekérés indul: {url}");

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("❌ Highscore lekérés hiba: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;
        Debug.Log("✅ Highscore válasz: " + json);

        MyHighscoreDtoList wrapper = JsonUtility.FromJson<MyHighscoreDtoList>("{\"items\":" + json + "}");
        List<MyHighscoreDto> highscores = wrapper.items;
        Debug.Log($"🔢 Sorok száma: {highscores.Count}");

        foreach (Transform child in myHighscoreParent)
            Destroy(child.gameObject);

        foreach (var hs in highscores)
        {
            GameObject row = Instantiate(myHighscoreRowPrefab, myHighscoreParent);
            Text[] texts = row.GetComponentsInChildren<Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = hs.levelName;
                texts[1].text = hs.highscore.ToString();
            }
            else
            {
                Debug.LogWarning("⚠️ Prefabban nincs elég Text komponens.");
            }
        }

        Debug.Log("✅ Highscore kiírás kész.");
    }
}