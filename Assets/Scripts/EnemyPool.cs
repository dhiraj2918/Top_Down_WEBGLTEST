using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public int poolSize = 10; // Number of enemies in the pool
    public float spawnRadius = 20f; // Radius around the center to spawn enemies
    public float spawnInterval = 5f; // Time interval between spawns
    public int deathLimit = 2; // Number of deaths before game over
    public TextMeshProUGUI gameOverText; // UI Text element to display game over message

    private Queue<GameObject> enemyPool;
    public int deathCount; // Count of enemy deaths


    void Start()
    {
        deathCount = deathLimit;
        // Initialize the enemy pool
        enemyPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }

        // Start the spawn coroutine
        StartCoroutine(SpawnEnemies());

        // Initialize game over text
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            Vector3 spawnPosition = GetRandomPointOnNavMesh();

            if (spawnPosition != Vector3.zero)
            {
                enemy.transform.position = spawnPosition;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Failed to find a valid spawn point on the NavMesh.");
                enemyPool.Enqueue(enemy); // Return the enemy back to the pool if spawn failed
            }
        }
        else
        {
            Debug.Log("No enemies available in the pool.");
        }
    }

    Vector3 GetRandomPointOnNavMesh()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * spawnRadius;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, spawnRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);

       
        deathCount--;
        if (deathCount <= 0)
        {
            
        }
    }

   
}
