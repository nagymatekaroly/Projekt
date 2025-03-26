using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("UI References")]
    public InputField usernameInput;
    public InputField fullNameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public InputField passwordConfirmInput;
    public Text feedbackText;

    [Header("API URL")]
    public string registerUrl = "https://mudskipdb.onrender.com/api/User/register";

    // Registration Button Click
    public void RegisterButtonClick()
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text) ||
            string.IsNullOrWhiteSpace(fullNameInput.text) ||
            string.IsNullOrWhiteSpace(emailInput.text) ||
            string.IsNullOrWhiteSpace(passwordInput.text) ||
            string.IsNullOrWhiteSpace(passwordConfirmInput.text))
        {
            feedbackText.color = Color.red;
            feedbackText.text = "All fields are required.";
            return;
        }

        if (passwordInput.text != passwordConfirmInput.text)
        {
            feedbackText.color = Color.red;
            feedbackText.text = "Passwords do not match.";
            return;
        }

        Register(usernameInput.text, fullNameInput.text, emailInput.text, passwordInput.text);
    }

    public void Register(string username, string fullName, string emailAddress, string password)
    {
        StartCoroutine(RegisterCoroutine(username, fullName, emailAddress, password));
    }

    IEnumerator RegisterCoroutine(string username, string fullName, string emailAddress, string password)
    {
        feedbackText.color = Color.yellow;
        feedbackText.text = "Processing registration...";

        var jsonData = JsonUtility.ToJson(new RegisterData(username, fullName, emailAddress, password));
        Debug.Log("🌐 Registration started");
        Debug.Log(jsonData);

        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("✅ Registration ended");

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Registration successful!");
            feedbackText.color = Color.green;
            feedbackText.text = "Registration successful! Redirecting in 5...";

            // 🔥 Indítjuk a visszaszámlálót
            StartCoroutine(RedirectToLogin());
        }
        else
        {
            Debug.LogError("❌ Error: " + request.error);
            Debug.LogError(request.downloadHandler.text);

            feedbackText.color = Color.red;

            string serverResponse = request.downloadHandler.text;
            string errorMsg = ExtractErrorMessage(serverResponse);

            if (string.IsNullOrEmpty(errorMsg))
            {
                errorMsg = "An unknown error occurred.";
            }

            feedbackText.text = errorMsg;
        }
    }

    // 🔥 ÚJ: Visszaszámláló és Login Scene-re váltás
    IEnumerator RedirectToLogin()
    {
        int countdown = 5;
        while (countdown > 0)
        {
            feedbackText.text = "Registration successful! Redirecting in " + countdown + "...";
            yield return new WaitForSeconds(1);
            countdown--;
        }

        // Scene váltás a loginra
        SceneManager.LoadScene("LoginScene"); // Pontosan a login scene neveddel egyezzen!
    }

    private string ExtractErrorMessage(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;


        try
        {
            var errorObj = JsonUtility.FromJson<ServerError>(json);
            return errorObj.error;
        }
        catch
        {
            return json;
        }
    }

    [System.Serializable]
    public class ServerError
    {
        public string error;
    }
}

[System.Serializable]
public class RegisterData
{
    public string username;
    public string fullName;
    public string emailAddress;
    public string passwordHash;

    public RegisterData(string username, string fullName, string emailAddress, string passwordHash)
    {
        this.username = username;
        this.fullName = fullName;
        this.emailAddress = emailAddress;
        this.passwordHash = passwordHash;
    }
}
