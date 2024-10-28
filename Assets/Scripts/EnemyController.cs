using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private float animLength;
    private bool isEnemyDead = false;

    public Transform player;   // Reference to the player's position
    public float speed;   // Movement speed
    public float stopDistance;
    private characterMovement playerScript;

    public float fireRate=2f;
    private float nextFireTime=10f;
    public Animator EnemyFire;
    public Animator EnemyLeftFire;
    public float minFireWaitTime = 1f;
    public float maxFireWaitTime = 3f;
    private AudioSource audio;
    public AudioClip enemyShootSound;

    private void Awake()
    {
        anim = GetComponent<Animator>();
         audio=GetComponent<AudioSource>();

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
        }else{
            anim.SetBool("isRunning", false);
            StartCoroutine(ShootingAfterRandomDelay());
        }

        if(playerScript!=null && !playerScript.isDead){ // instead i can call enemyDeath fn from player script
            if (!isEnemyDead && Input.GetMouseButtonDown(0)){
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    EnemyDeath();
                }
            }
        }
    }

    private void EnemyDeath(){
        isEnemyDead = true; // Prevent triggering death multiple times

        if (anim != null){
            anim.SetTrigger("Dead");
            animLength = anim.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(WaitForAnimationAndDestroy());
        }
    }

    private IEnumerator WaitForAnimationAndDestroy(){
        yield return new WaitForSeconds(animLength);
        Destroy(gameObject);  //Destroy the enemy
    }

    IEnumerator ShootingAfterRandomDelay(){
        float waitTimeBeforeShooting = Random.Range(minFireWaitTime, maxFireWaitTime);
        yield return new WaitForSeconds(waitTimeBeforeShooting);
        if (Time.time >= nextFireTime && !playerScript.isDead && !isEnemyDead){
               bool shootFromLeft = Random.Range(0, 2) == 0;

            if (shootFromLeft){
                Debug.Log("Enemy shooting from left side.");
                EnemyLeftFire.SetTrigger("EnemyLeftFire");
                 //Invoke("PlayEnemyShootSound", 0.3f);


            }else{
                Debug.Log("Enemy shooting from right side.");
                EnemyFire.SetTrigger("EnemyFire");
                // Invoke("PlayEnemyShootSound", 0.3f);
            }

                playerScript.isDead=true;

                nextFireTime = Time.time + 5f / fireRate;
                Debug.Log("nextfireTime"+nextFireTime);
        }
    }
    private void PlayEnemyShootSound() {
        audio.PlayOneShot(enemyShootSound);
    }

}

