using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Ide húzod be az Inspectorban a LevelSelectPanel-t
    public GameObject levelSelectScene;

    // Ezt rakod a Play Game gombra → megjelenik a pályaválasztó
    public void OpenLeveScene()
    {
        levelSelectScene.SetActive(true);
    }

    // Ezt rakod a "Back" gombra a pályaválasztón → eltűnik a pályaválasztó
    public void CloseLevelSelect()
    {
        levelSelectScene.SetActive(false);
    }
}