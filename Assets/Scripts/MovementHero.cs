using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;

    public float jumpHeight = 5f;
    public bool isGround = true;
    public float movementSpeed = 3f;
    public CoinManager cm;

    private bool facingRight = true;
    private float movement;

    // Dash változók
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool canDash = true;
    private bool isDashing = false;

    void Start()
    {
        if (cm == null)
        {
            cm = FindObjectOfType<CoinManager>();
            if (cm == null)
            {
                Debug.LogError("❌ CoinManager nem található MovementHero-ban!");
            }
        }
    }

    void Update()
    {
        // Ha dash közben van, ne engedje a mozgást
        if (isDashing) return;

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
        animator.SetFloat("Walk", Mathf.Abs(movement));

        // Attack animation
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Attack");
        }

        // Dash külön gombra (például L)
        if (Input.GetKeyDown(KeyCode.L) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return; // Ha dash-ben van, ne változtasson a mozgáson

        // Move using Rigidbody
        rb.linearVelocity = new Vector2(movement * movementSpeed, rb.linearVelocity.y);

        // Ground detection
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ütközés történt: " + other.gameObject.name);

        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            cm.AddCoin(100); // vagy cm.coinCount += 100
        }
    }



    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //animator.SetBool("isDashing", true); // Dash animáció bekapcsolása
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0; // Gravitáció kikapcsolása dash alatt

        float dashDirection = facingRight ? 1f : -1f; // Irány meghatározása
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f); // Dash előre

        yield return new WaitForSeconds(dashDuration); // Dash időtartama

        rb.gravityScale = originalGravity; // Gravitáció visszaállítása
        rb.linearVelocity = Vector2.zero; // Mozgás leállítása
        isDashing = false;

        // animator.SetBool("isDashing", false); // Dash animáció kikapcsolása

        yield return new WaitForSeconds(dashCooldown); // Cooldown
        canDash = true;
    }
}
