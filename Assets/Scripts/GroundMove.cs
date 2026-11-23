using UnityEngine;

public class GroundMove : MonoBehaviour
{
    public float groundWidth = 20f;
    public int totalGrounds = 2;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        float speed = 5f;
        if (DifficultyManager.Instance != null)
            speed = DifficultyManager.Instance.currentSpeed;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= -groundWidth)
        {
            Vector3 newPos = transform.position;
            newPos.x += groundWidth * totalGrounds;
            transform.position = newPos;
        }
    }
}