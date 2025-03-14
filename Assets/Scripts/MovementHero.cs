using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{

    public Text health;
    public Animator animator;
    public Rigidbody2D rb;

    public float jumpHeight = 5f;
    public bool isGround = true;
    public float movementSpeed = 3f;
    public CoinManager cm;

    private bool facingRight = true;
    private float movement;

    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        

        // Horizontal movement
        movement = Input.GetAxis("Horizontal");

        // Facing direction
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        // Walk animation
        if (Mathf.Abs(movement) > .1f) {
            animator.SetFloat("Walk", 1f);
        }
        else if ((movement) < .1f) {
            animator.SetFloat("Walk", 0f);
        }

        // Attack animation
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        // Move using Rigidbody
        rb.linearVelocity = new Vector2(movement * movementSpeed, rb.linearVelocity.y);

        // Ground detection
       
        // Fix stuck jump animation
        animator.SetBool("Jump", !isGround);
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin")) 
        {
            Destroy(other.gameObject);
            cm.coinCount++;
        }
    }
}
