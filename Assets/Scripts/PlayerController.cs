using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera camera;
    private Rigidbody2D rigidbody;
    private bool canJump;
    private Animator animator;
    private AudioClip jumpClip;
    private AudioClip hitClip;
    private AudioSource audioSource;

    public PlayerStatus playerStatus;

    public float jumpPower = 8f;

    void Start()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        jumpClip = (AudioClip) Resources.Load("Sounds/flap");
        hitClip = (AudioClip) Resources.Load("Sounds/Hit");

        Vector2 screenBounds = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

        transform.position = new Vector2(-screenBounds.x + 1, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle" && playerStatus == PlayerStatus.STATUS_NORMAL)
        {
            audioSource.PlayOneShot(hitClip);
            animator.SetTrigger("Die");
            GameController.instance.GameOver();
            Debug.Log("Hit by obstacle");
        }
        else
        {
            animator.SetBool("isJumping", false);
            canJump = true;
            Debug.Log("Player is on the ground");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Debug.Log("Jump");
            audioSource.PlayOneShot(jumpClip);
            animator.SetBool("isJumping", true);
            canJump = false;
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}

public enum PlayerStatus
{
    STATUS_NORMAL,
    STATUS_IMMORTAL,
    STATUS_GHOST
}