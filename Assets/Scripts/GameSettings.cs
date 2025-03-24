using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock FPS to 60
        Application.targetFrameRate = 30;

        // Set resolution to 1920x1080, windowed mode
        Screen.SetResolution(1920, 1080, false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
