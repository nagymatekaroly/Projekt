using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void LoadHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public string logoutUrl = "https://mudskipdb.onrender.com/api/User/logout"; // ha van ilyen endpointod

    public void Logout()
    {
        Debug.Log("✅ Logout pressed - contacting API...");
        StartCoroutine(LogoutCoroutine());
    }

    IEnumerator LogoutCoroutine()
    {
        UnityWebRequest request = new UnityWebRequest(logoutUrl, "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // Ha session token kell, itt add hozzá
        // request.SetRequestHeader("Authorization", "Bearer " + YOUR_TOKEN);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Successfully logged out from API.");
        }
        else
        {
            Debug.LogWarning("⚠️ Logout API failed, reason: " + request.error);
        }

        // 🔄 Vissza Loginre akkor is, ha fail (API hibát nem lát a user)
        SceneManager.LoadScene("LoginScene");
    }


    public void ExitGame()
    {
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
}

