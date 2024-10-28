using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    Vector2 movement;
    bool facingRight=true;

    public Animator attackAnim;
    public Animator fire;
    private AudioSource audioSource;
    public AudioClip shootSound;
    public int lives=3;
    public Image[] lifeIcons;
    public bool isDead=false;

    private void Awake(){
        rb=GetComponent<Rigidbody2D>();
        audioSource=GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDifference = mousePos.x - transform.position.x;
        if (!isDead){
            if (xDifference <- 0.1f && facingRight)  // Flip if mouse is on the right side and player is facing left
            {
                Flip();
            //  Debug.Log("calling flip to turn right");
            }else if (xDifference >0.1f && !facingRight) // Flip if mouse is on the left side and player is facing right
            {
                Flip();
            // Debug.Log("Calling flip to turn left");
            }
        }else{
            attackAnim.SetBool("Dead", true);
            StartCoroutine(WaitForDeathAnimation());
        }

        if (Input.GetMouseButtonDown(0)&& !isDead){
            Debug.Log("Police fired");
            audioSource.PlayOneShot(shootSound);
            if (fire != null){
                    fire.SetTrigger("Fire");  //play police fire animation
            }else{
                    Debug.LogError("Fire Animator not assigned");
            }
            if (attackAnim != null){
                    attackAnim.SetTrigger("Attack"); //play police attack animation
            }else{
                    Debug.LogError("Attack Animator not assigned");
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))    // Check if the mouse click was on an enemy
            {
                GameManager.instance.IncrementScore();

            }else{
                MissedShot();
            }
        }
    }

    void Flip(){
         facingRight = !facingRight;
         Vector3 localScale = transform.localScale;
         localScale.x *= -1;
         transform.localScale = localScale;
    }

    public void MissedShot()
    {
        if (lives > 0)
        {
            lives--;  // Decrease the number of lives
            UpdateLivesUI();
        }
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            attackAnim.SetBool("Dead",true);
            isDead=true;
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    void UpdateLivesUI()
    {
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (i < lives)
            {
                lifeIcons[i].enabled = true;  // Show the life icon
            }
            else
            {
                lifeIcons[i].enabled = false;  // Hide the life icon
            }
        }
    }
     IEnumerator WaitForDeathAnimation()
    {
        AnimatorStateInfo animStateInfo = attackAnim.GetCurrentAnimatorStateInfo(0);
        // Get length of the current "Dead" animation

        yield return new WaitForSeconds(animStateInfo.length+1f);  // Wait for the animation to finish (using its length)
        GameManager.instance.GameOver();
       // Destroy(gameObject);
    }
}
