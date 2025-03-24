using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Ide húzod be az Inspectorban a LevelSelectPanel-t
    public GameObject levelSelectPanel;

    // Ezt rakod a Play Game gombra → megjelenik a pályaválasztó
    public void OpenLevelSelect()
    {
        Debug.Log("✅ OpenLevelSelect() lefutott!");
        levelSelectPanel.SetActive(true);
    }

    // Ezt rakod a "Back" gombra a pályaválasztón → eltűnik a pályaválasztó
    public void CloseLevelSelect()
    {
        levelSelectPanel.SetActive(false);
    }
}