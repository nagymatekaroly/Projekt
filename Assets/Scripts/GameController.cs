using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector3 checkpointPos;
    Rigidbody2D playerRb;
    Vector3 originalScale; // T�roljuk az eredeti m�retet

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale; // Eredeti m�ret ment�se
    }

    private void Start()
    {
        checkpointPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Die();
            Debug.Log("Collided with: " + collision.gameObject.name);
        }
    }

    void Update()
    {
        Debug.Log("TimeScale: " + Time.timeScale);
    }


    public void UpdateCheckpoint(Vector3 pos)
    {
        checkpointPos = pos;
    }
    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }
    IEnumerator Respawn(float duration)
    {
        playerRb.simulated = false;
        playerRb.linearVelocity = Vector3.zero;
        transform.localScale = Vector3.zero; // Null�ra cs�kkentj�k a m�retet
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        transform.localScale = originalScale; // **Az eredeti m�retet �ll�tjuk vissza!**
        playerRb.simulated = true;
    }
}