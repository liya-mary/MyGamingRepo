using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemies;
    public float minSpawnTime, maxSpawnTime;
    public bool spawnOnRightSide;

    private GameObject currentEnemy;
    public float minY=-3f;
    public float maxY=1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   void SpawnEnemies(bool spawnOnRightSide) {
       if(currentEnemy==null){
           float randomY=Random.Range(minY,maxY);
           Vector3 spawnPosition=new Vector3(transform.position.x,randomY,transform.position.z);

           if (spawnOnRightSide) {
                currentEnemy=Instantiate(enemies, spawnPosition , Quaternion.Euler(0, 180, 0));
           } else {
                currentEnemy=Instantiate(enemies, spawnPosition , Quaternion.Euler(0, 0, 0));
           }
       }
    }
    IEnumerator Spawn(){
        float waitTime=2f;
        yield return new WaitForSeconds(waitTime);
        while(true){
            SpawnEnemies(spawnOnRightSide);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
