using UnityEngine;
using DG.Tweening;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float minY = -2.5f;
    public float maxY = -1f;

    private float timer = 0f;
    private float nextSpawnTime = 1.5f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            timer = 0f;
            SpawnCoin();
            nextSpawnTime = Random.Range(1.2f, 2f);
        }
    }

    void SpawnCoin()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(transform.position.x, randomY, 0);
        GameObject newCoin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

        // Coin Pop-In effect
        newCoin.transform.localScale = Vector3.zero;
        newCoin.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}