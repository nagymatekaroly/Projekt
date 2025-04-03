using UnityEngine;

public class CoinManagerBootstrap : MonoBehaviour
{
    public GameObject coinManagerPrefab;

    void Awake()
    {
        if (CoinManager.instance == null)
        {
            Instantiate(coinManagerPrefab);
        }
    }
}
