using System.Collections;
using UnityEngine;

public class Kill : MonoBehaviour
{
    private Animator anim;
    private float animLength;
    private bool isDead = false;

    public Transform player;   // Reference to the player's position
    public float speed;   // Movement speed
    public float stopDistance;
   // private Animator animator; // Reference to the Animator
    private characterMovement playerScript;

    public float fireRate=4f;
    private float nextFireTime=10f;
    public Animator EnemyFire;
    public Animator EnemyLeftFire;
    public float minFireWaitTime = 1f;
    public float maxFireWaitTime = 3f;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Animator not found on the GameObject");
        }
    }
    void Start(){
        GameObject police=GameObject.FindWithTag("Player");
        if(police !=null){
            playerScript=police.GetComponent<characterMovement>();
        }else{
            Debug.Log("player controller not found");
        }

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            transform.Translate(direction * speed * Time.deltaTime, Space.World);


        }
        else
        {
            anim.SetBool("isRunning", false);
            StartCoroutine(ShootingAfterRandomDelay());
            // Stop moving and change animation to idle or stand
         //   Debug.Log("Stop--------------");



        }

        if(playerScript!=null && !playerScript.isDead){
            if (!isDead && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        isDead = true; // Prevent triggering death multiple times

        if (anim != null)
        {
            anim.SetTrigger("Dead");

            // Cache the animation length *after* triggering the animation
            animLength = anim.GetCurrentAnimatorStateInfo(0).length;

            StartCoroutine(WaitForAnimationAndDestroy());
        }
    }

    // Coroutine to wait for the animation to finish
    private IEnumerator WaitForAnimationAndDestroy()
    {
        // Wait for the cached animation length
        yield return new WaitForSeconds(animLength);

        // Destroy the object after the animation has finished playing
        Destroy(gameObject);
    }
    IEnumerator ShootingAfterRandomDelay(){
        float waitTimeBeforeShooting = Random.Range(minFireWaitTime, maxFireWaitTime);
        yield return new WaitForSeconds(waitTimeBeforeShooting);
        if (Time.time >= nextFireTime && !playerScript.isDead && !isDead){
               bool shootFromLeft = Random.Range(0, 2) == 0;

            if (shootFromLeft)
            {
                Debug.Log("Enemy shooting from left side.");
                EnemyLeftFire.SetTrigger("EnemyLeftFire");
            }
            else
            {
                Debug.Log("Enemy shooting from right side.");
                 EnemyFire.SetTrigger("EnemyFire");
            }


                playerScript.isDead=true;

                nextFireTime = Time.time + 5f / fireRate; // Reset the fire cooldown
                Debug.Log("nextfireTime"+nextFireTime);
            }
    }

}
