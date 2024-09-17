using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce=5f;
    public float gravityModifier=1.5f;
    public bool gameOver = false;
    public ParticleSystem dustParticle;
    public ParticleSystem explosionParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private Animator playerAnim;
    private Rigidbody rb;
    private bool isGrounded;
    private AudioSource playerAudio;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !gameOver)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnim.SetTrigger("Jump_trig");
            dustParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            dustParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over");
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dustParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }

    }
}
