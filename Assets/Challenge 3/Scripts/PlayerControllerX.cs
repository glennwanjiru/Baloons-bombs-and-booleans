using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver; // To check if the game is over

    public float floatForce; // Force to make the balloon float upwards
    private float gravityModifier = 1.5f; // Gravity multiplier
    private Rigidbody playerRb; // Rigidbody of the player

    public ParticleSystem explosionParticle; // Explosion effect when hit by bomb
    public ParticleSystem fireworksParticle; // Fireworks effect when collecting money

    private AudioSource playerAudio; // Audio source for sound effects
    public AudioClip moneySound; // Sound effect when collecting money
    public AudioClip explodeSound; // Sound effect for explosion
    public AudioClip bounceSound; // Sound effect for bouncing off the ground

    // Variables for height restriction and bounce
    private bool isLowEnough; // To check if the balloon is low enough to float upwards
    public float maxHeight = 15.0f; // Maximum allowed height for the balloon
    public float bounceForce = 10.0f; // Force applied when bouncing off the ground

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier; // Set custom gravity
        playerRb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        playerAudio = GetComponent<AudioSource>(); // Get the AudioSource component

        // Apply an initial upward force to the balloon
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is below the maxHeight limit
        isLowEnough = transform.position.y < maxHeight;

        // On space key press, apply an upward impulse force to push the player up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }

        // If the player is above maxHeight, clamp the player's position and limit the upward velocity
        if (transform.position.y >= maxHeight)
        {
            // Clamp the player's position so they cannot exceed maxHeight
            transform.position = new Vector3(transform.position.x, maxHeight, transform.position.z);

            // Nullify upward velocity to prevent floating beyond maxHeight
            if (playerRb.velocity.y > 0)
            {
                playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
            }
        }
    }

    // Handle collisions with different objects
    private void OnCollisionEnter(Collision other)
    {
        // Check if the player collides with the ground
        if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            // Apply an upward force to make the player bounce off the ground
            playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

            // Play the bounce sound
            playerAudio.PlayOneShot(bounceSound, 1.0f);

            // Debug log to check the bounce behavior
            Debug.Log("Player bounced off the ground");
        }

        // If player collides with a bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play(); // Play explosion effect
            playerAudio.PlayOneShot(explodeSound, 1.0f); // Play explosion sound
            gameOver = true; // Set game over to true
            Debug.Log("Game Over!"); // Log game over message
            Destroy(other.gameObject); // Destroy the bomb
        }

        // If player collides with money, play fireworks and collect
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play(); // Play fireworks effect
            playerAudio.PlayOneShot(moneySound, 1.0f); // Play money sound
            Debug.Log("Money collected!"); // Log money collection
            Destroy(other.gameObject); // Destroy the money object
        }
    }
}
