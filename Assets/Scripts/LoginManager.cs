using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text feedbackText;

    public string loginUrl = "https://mudskipdb.onrender.com/api/User/login";

    public void LoginButtonClick()
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text) || string.IsNullOrWhiteSpace(passwordInput.text))
        {
            feedbackText.color = Color.red;
            feedbackText.text = "Username and password are required.";
            return;
        }

        StartCoroutine(LoginCoroutine(usernameInput.text, passwordInput.text));
    }

    IEnumerator LoginCoroutine(string username, string password)
    {
        feedbackText.color = Color.yellow;
        feedbackText.text = "Processing login...";

        var jsonData = JsonUtility.ToJson(new LoginData(username, password));
        Debug.Log("Login data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("✅ Login ended");

        // ✅ KIZÁRÓLAG akkor váltunk, ha tényleg 200-as a válasz
        if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
        {
            Debug.Log("✅ Login successful! Session started.");
            feedbackText.color = Color.green;
            feedbackText.text = "Login successful! Loading game menu...";

            // ✅ Mostantól CSAK innen megy a Scene váltás
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            Debug.LogError("❌ Login failed. Response code: " + request.responseCode);
            Debug.LogError("❌ API Response: " + request.downloadHandler.text);

            feedbackText.color = Color.red;

            string errorMsg = request.downloadHandler.text;
            if (string.IsNullOrEmpty(errorMsg))
                errorMsg = "Invalid username or password.";

            feedbackText.text = errorMsg;
            // ❌ Nem dobunk át sehova
        }

        yield break;
    }
}

[System.Serializable]
public class LoginData
{
    public string username;
    public string password;

    public LoginData(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}
