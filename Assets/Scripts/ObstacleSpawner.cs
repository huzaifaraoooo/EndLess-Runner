using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    public float minSpawnTime = 1.5f;   // Minimum gap (seconds)
    public float maxSpawnTime = 3f;     // Maximum gap (seconds)
    public float minY = -3f;
    public float maxY = -2.5f;

    [Header("Scale Settings")]
    public float minScale = 0.7f;       // Chota obstacle
    public float maxScale = 1.5f;       // Bara obstacle

    private float timer = 0f;
    private float nextSpawnTime = 2f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            timer = 0f;
            SpawnObstacle();

            // Har spawn par random distance (1.5 se 3 seconds)
            nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0) return;

        // Random prefab select karein
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);
        GameObject selectedPrefab = obstaclePrefabs[randomIndex];

        // Random Y position
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);

        // Obstacle spawn karein
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);

        // Random scale (different sizes)
        float randomScale = Random.Range(minScale, maxScale);
        newObstacle.transform.localScale = new Vector3(randomScale, randomScale, 1f);
    }
}