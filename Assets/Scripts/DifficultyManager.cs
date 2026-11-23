using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public float currentSpeed = 3f;      // ← 5 se 3
    public float maxSpeed = 7f;           // ← 12 se 7
    public float speedIncreaseRate = 0.3f; // ← 0.5 se 0.3
    public float timeBetweenIncrease = 15f; // ← 10 se 15

    private float timer = 0f;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        timer += Time.deltaTime;

        if (timer >= timeBetweenIncrease)
        {
            timer = 0f;

            if (currentSpeed < maxSpeed)
            {
                currentSpeed += speedIncreaseRate;
                if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
                Debug.Log("Speed increased to: " + currentSpeed);
            }
        }
    }
}