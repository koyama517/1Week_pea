using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnInterval = 2f;
    public float minY = -3f;
    public float maxY = 3f;
    public float spawnX = 10f;

    public float minSpeed = 2f;
    public float maxSpeed = 6f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(minY, maxY);
        float randomSpeed = Random.Range(minSpeed, maxSpeed);

        Vector2 spawnPosition = new Vector2(spawnX, randomY);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyMover mover = enemy.GetComponent<EnemyMover>();
        if (mover != null)
        {
            mover.SetSpeed(randomSpeed);
        }
    }
}
