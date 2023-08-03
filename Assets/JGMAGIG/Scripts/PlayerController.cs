using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;  
    public float jumpForce = 5f;  
    private bool isJumping = false;  
    private Rigidbody2D rb;
    private Animator animator;
    public bool canDoubleJump = false;  
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f; 
    public bool gameOver = false;  // Add this line
    private bool gameStarted = false;
    private float lastTimeMoving;
    public TMP_Text deathReasonText;  // Assign this in the inspector
    private Vector3 lastPosition;
    private string deathReason;
    public GameObject gameOverPanel;  // Assign this in the inspector
    public TMP_Text scoreText;  // Assign this in the inspector
    public int objectsBlownUp = 0;
    public float distanceTraveled = 0;
    public TMP_Text distanceText;
    public TMP_Text hudScureText;
    public AudioClip gunshot, jumpSound, landSound;
    public AudioSource audioSource;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameStarted = false;
        deathReason = ""; 
        lastPosition = transform.position;
    }

    void CheckGameOver()
    {
        // Check if the player has stopped moving after game start.
        if (gameStarted && Mathf.Abs(rb.velocity.x) < 0.1f && Time.time > lastTimeMoving + 0.2f)
        {
            deathReason = "Eaten By Clown!";
            gameOver = true;
            
            Debug.Log("Game over because player got eaten.");  // Debug print
        }

        // Check if the player has fallen off the platform.
        if (transform.position.y < -10)
        {
            deathReason = "Fell off the platform!";
            gameOver = true;
            
            Debug.Log("Game over because player fell off platform.");  // Debug print
        }// Check if the player has stopped moving vertically.
        if (gameStarted && Mathf.Abs(rb.velocity.y) < 0.01f && Time.time > lastTimeMoving + 0.2f)
        {
            deathReason = "Eaten By Clown!";
            gameOver = true;
    
            Debug.Log("Game over because player got Eaten By Clown!");  // Debug print
        }

    }



    void Update()
    {
        if (!gameOver)
        {
            distanceTraveled = transform.position.x;
            if ((transform.position - lastPosition).magnitude > 0.01f)
            {
                lastTimeMoving = Time.time;
            }
        
            lastPosition = transform.position;
            rb.velocity = new Vector2(speed, rb.velocity.y);

            if ((Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())) && (!isJumping || canDoubleJump))
            {
                audioSource.PlayOneShot(jumpSound);
                gameStarted = true;
                if (isJumping && canDoubleJump)
                {
                    canDoubleJump = false;
                }

                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                isJumping = true;
                animator.SetBool("jump", true);
            }
            else if (Mathf.Abs(rb.velocity.y) < 0.01f)
            {
                animator.SetBool("walk", true);
                animator.SetBool("jump", false);
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                ShootProjectile();
            }

            CheckGameOver();

            IncreaseSpeedOverTime();
            hudScureText.text = "Score: " + objectsBlownUp.ToString();
            distanceText.text = "Distance: " + distanceTraveled.ToString("0.0");

        }       
        else
        {
            rb.velocity = new Vector2(0, 0);
            gameOverPanel.SetActive(true);
            scoreText.text = "Score: " + DistanceTraveled().ToString("0");
            deathReasonText.text = "Cause of Death: " + deathReason;  // Set the text of the death reason
        }
    }
    public float DistanceTraveled()
    {
        return transform.position.x;  // Assumes the player starts at x=0
    }

    public float MaxJumpDistance()
    {
        return (jumpForce / Physics.gravity.magnitude) * speed;
    }

    public float MaxJumpHeight()
    {
        return jumpForce * jumpForce / (2f * Physics.gravity.magnitude * speed);
    }
    public IEnumerator SlowDown(float duration, float slowFactor)
    {
        speed *= slowFactor;
        yield return new WaitForSeconds(duration);
        speed /= slowFactor;
    }

    public Transform barrel;
    public void ShootProjectile()
    {
        audioSource.PlayOneShot(gunshot);
        // Instantiate the projectile Prefab at the player's location and add a forward force to its Rigidbody2D
        GameObject projectile = Instantiate(projectilePrefab, barrel.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.AddForce(new Vector2(projectileSpeed, 0f), ForceMode2D.Impulse);
    }

    // Detect when the player lands after a jump
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // Assumes you have a "Ground" tag on your ground objects
        {
            isJumping = false;
            canDoubleJump = true;
            //audioSource.PlayOneShot(landSound);
        }
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            deathReason = "Hit by an enemy projectile!";
            gameOver = true;
            
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            deathReason = "Collided with an obstacle!";
            gameOver = true;
            
        }
    }    
    private bool IsPointerOverUIObject()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Touch myTouch = Input.GetTouch(0);

            if (myTouch.phase == TouchPhase.Began)
            {
                return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(myTouch.fingerId);
            }
            else
            {
                return false;
            }
        }
        else
        {
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has been hit by an enemy projectile
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            gameOver = true;
             // Assuming you have a gameOver animation
        }       
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            deathReason = "Hit by an enemy projectile!";
            gameOver = true;
            
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            deathReason = "Collided with an obstacle!";
            gameOver = true;
            
        }
    }

    public float speedIncreaseInterval = 10f;
    private float lastSpeedIncreaseDistance = 0f;

    void IncreaseSpeedOverTime()
    {
        // Increase the speed every 50 meters
        if (Mathf.Floor(DistanceTraveled() / speedIncreaseInterval) > Mathf.Floor(lastSpeedIncreaseDistance / speedIncreaseInterval))
        {
            speed += speed * 0.1f;
            speedIncreaseInterval += DistanceTraveled() + speedIncreaseInterval * 0.1f;
            lastSpeedIncreaseDistance = DistanceTraveled();
        }
    }
 
    public void Retry()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}