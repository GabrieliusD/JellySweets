using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject[] bosses;
    public float groupSpawnRate;
    public float spawnUp;
    public float spawnDown;
    public int groupSmallest;
    public int groupLargest;
    public float groupRadius;
    float enemiesPerWave;
    public float enemieswave;
    public int numberOfWaves;
    public float PauseBetweenWaves;

    float totalTime = 0;
    ValueManager valueManager;
    // Start is called before the first frame update
    void Start()
    {
        enemiesPerWave = enemieswave;
        numberOfWaves = 0;
        valueManager = ValueManager.instance;
        
    }

    private void OnEnable()  
    {
        enemiesPerWave = enemieswave+valueManager.Night;
        numberOfWaves = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfWaves < 5)
        {
            if (numberOfWaves == 0)
                totalTime += groupSpawnRate;
            if (enemiesPerWave >= 0)
            {
                 if (totalTime >= groupSpawnRate)
                 {

                  groupLocation();

                  totalTime -= groupSpawnRate;
                  }
            totalTime += Time.deltaTime;
            }
            
        }
        else
        {
            //check if any enemies are left in the scene;
            valueManager.checkIfAllEnemiesDefeated();
        }
        valueManager.WaveTextUpdate(numberOfWaves);

    }

    void groupLocation()
    {

        float randomGrouplocation = Random.Range(spawnUp, spawnDown);
        int groupSize = Random.Range(groupSmallest, groupLargest);
        for (int i = 0; i < groupSize; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Length);

            float radiusX = Random.Range(groupRadius, -groupRadius);
            float radiusY = Random.Range(groupRadius, -groupRadius);
            Vector2 pos = new Vector2(transform.position.x+radiusX, randomGrouplocation+radiusY);
            pos = new Vector2(pos.x,Mathf.Clamp(pos.y, spawnDown, spawnUp));
            
            GameObject enemyGroup = Instantiate(enemies[randomEnemy], pos, Quaternion.identity);
            enemiesPerWave--;
            valueManager.enemyInBattle();
            if((numberOfWaves == 4) && (enemiesPerWave == 1))
            {
                int randomBoss = Random.Range(0, bosses.Length);

                GameObject boss = Instantiate(bosses[randomBoss], pos, Quaternion.identity);
                valueManager.enemyInBattle();

            }
            if (enemiesPerWave <=0)
            {
                enemiesPerWave = enemieswave;
                numberOfWaves++;
                return;
            }
        }

    }

}
