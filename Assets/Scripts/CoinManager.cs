using UnityEngine;
using UnityEngine.UI;
public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("🔥 TimeScale resetelve: " + Time.timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        coinText.text = "High Score: " + coinCount.ToString();
    }
}
