using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{public float speed;
    public int lives = 3;
    public Image[] lifeIcons;
    public Animator attackAnim;
    public Animator fire;
    public AudioClip shootSound;
    public bool isDead = false;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 movement;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isDead)
        {
            HandleDeath();
            return;
        }

        HandleMovement();
        HandleFlip();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * (speed * Time.fixedDeltaTime);
    }

    private void HandleMovement()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void HandleFlip()
    {
        // Flip based on mouse position relative to the player
        float xDifference = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;

        if ((xDifference < -0.1f && facingRight) || (xDifference > 0.1f && !facingRight))
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private void HandleShooting()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            FireAtEnemy();
            GameManager.instance.IncrementScore();
        }
        else
        {
            MissedShot();
        }
    }

    private void FireAtEnemy()
    {
        fire?.SetTrigger("Fire");
        attackAnim?.SetTrigger("Attack");
        audioSource.PlayOneShot(shootSound);
    }

    public void MissedShot()
    {
        if (--lives <= 0)
        {
            isDead = true;
            attackAnim.SetBool("Dead", true);
            StartCoroutine(WaitForDeathAnimation());
            return;
        }
        UpdateLivesUI();
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = i < lives;
        }
    }

    private void HandleDeath()
    {
        attackAnim.SetBool("Dead", true);
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(attackAnim.GetCurrentAnimatorStateInfo(0).length + 1f);
        GameManager.instance.GameOver();
    }
}
