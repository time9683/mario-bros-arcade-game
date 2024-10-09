using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float spawnRate = 2f;
    public float amountOfEnemies = 5;
    private float timer = 0f;
    public bool isRight = true;


    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;


        if (timer >= spawnRate && amountOfEnemies > 0)
        {
            // si es right es false volteamos el enemigo
            
            GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            if (!isRight)
            {
                enemyInstance.GetComponent<Enemy>().flipDirection();
            }

            timer = 0f;
            amountOfEnemies--;
        }
        
    }
}
