using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float maxDistance = 2f; // Maximum distance the monster will stay behind the player.
    public float jumpForce = 7f;
    public float stuckCatchUpSpeed = 10f;

    private Rigidbody2D rb;
    private PlayerController playerController; // Assuming you have a script named "PlayerController" on your player.
    private float lastPlayerX;
    private Animator animator; // Animator component on the monster

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<PlayerController>();
        lastPlayerX = player.position.x;
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void FixedUpdate()
    {
        // If the player is stuck.
        if(player.position.x == lastPlayerX)
        {
            // Monster catches up with increased speed.
            rb.velocity = new Vector2(stuckCatchUpSpeed, rb.velocity.y);
            animator.SetBool("walk", true); // Set walk animation
        }
        else
        {
            // Monster follows the player.
            if (Mathf.Abs(player.position.x - transform.position.x) > maxDistance)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                animator.SetBool("walk", true); // Set walk animation
            }
            else
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                //animator.SetBool("walk", false); // Set idle animation
            }

            // If player's y position is higher than monster's, make the monster "jump".
            if (player.position.y - transform.position.y > 1f)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                animator.SetBool("jump", true); // Set jump animation
            }
            else
            {
                animator.SetBool("jump", false); // Set idle/walk animation
            }
        }

        // Save the player's current x position.
        lastPlayerX = player.position.x;
    }
}
