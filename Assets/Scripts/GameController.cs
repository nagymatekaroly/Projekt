using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 checkpointPos;
    Rigidbody2D playerRb;
    Vector3 originalScale; // Eredeti méret mentése

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    private void Start()
    {
        checkpointPos = transform.position; // Alap checkpoint a kezdőpozíció
        Debug.Log("✅ Kezdő checkpoint pozíció beállítva: " + checkpointPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Debug.Log("❌ Ütközés az akadállyal: " + collision.gameObject.name);
            Die();
        }
    }

    void Update()
    {
       
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
        Debug.Log("✅ Checkpoint frissítve: " + checkpointPos);
    }

    void Die()
    {
        Debug.Log("💀 Meghaltál, respawn indul...");
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        playerRb.simulated = false;
        playerRb.linearVelocity = Vector2.zero;
        transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(duration);

        // Visszaállítjuk a mentett checkpoint pozícióra
        transform.position = checkpointPos;
        transform.localScale = originalScale;
        playerRb.simulated = true;

        Debug.Log($"⚠️ Respawn indult. Checkpoint pozíció: {checkpointPos}");
    }
}
