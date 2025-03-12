using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;
    Rigidbody2D playerRb;
    Vector3 originalScale; // Tároljuk az eredeti méretet

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Eredeti méret mentése
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
    }

    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        playerRb.simulated = false;
        playerRb.linearVelocity = Vector2.zero;
        transform.localScale = Vector3.zero; // Nullára csökkentjük a méretet
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        transform.localScale = originalScale; // **Az eredeti méretet állítjuk vissza!**
        playerRb.simulated = true;
    }
}