using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f; // 🔥 Garantált reset
        SceneManager.LoadScene(sceneName);
    }

    public void DebugClick()
    {
        Debug.Log("✅ RÁKATTINTOTTÁL A GOMBRA!");
    }
}

