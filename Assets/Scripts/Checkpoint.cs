using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameController gameController;
    public Transform respawnPoint;

    private SpriteRenderer spriteRenderer;
    private Collider2D coll;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("Player")?.GetComponent<GameController>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        if (gameController == null)
        {
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameController != null)
            {
                gameController.UpdateCheckpoint(respawnPoint.position);
            }      
        }
    }
}
