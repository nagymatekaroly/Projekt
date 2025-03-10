using UnityEngine;

public class Move : MonoBehaviour
{
    

    public Animator animator;
    public Rigidbody2D rb;

    public int maxHealth = 3;
    public float jumpHeight = 5f;
    public bool isGround = true;
    public float movementSpeed = 3f;
    
    private bool facingRight = true;
    private float movement;

    void Update()
    {
        // Horizontal movement
        movement = Input.GetAxis("Horizontal");

        // Facing direction
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
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
        animator.SetFloat("Walk", Mathf.Abs(movement));

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
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpHeight);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }
}
