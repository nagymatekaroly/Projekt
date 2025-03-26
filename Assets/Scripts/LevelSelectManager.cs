using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectManager : MonoBehaviour
{
    public Button tutorialButton;
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;

    private void Start()
    {
        tutorialButton.onClick.AddListener(() => LoadLevel("Tutorial"));
        level1Button.onClick.AddListener(() => LoadLevel("Level 1"));
        level2Button.onClick.AddListener(() => LoadLevel("Level 2"));
        level3Button.onClick.AddListener(() => LoadLevel("Level 3"));
        level4Button.onClick.AddListener(() => LoadLevel("Level 4"));
        level5Button.onClick.AddListener(() => LoadLevel("Level 5"));
    }

    private void LoadLevel(string sceneName)
    {
        Debug.Log("Loading level: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
