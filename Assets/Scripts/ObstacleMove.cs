using UnityEngine;
using DG.Tweening;

public class ObstacleMove : MonoBehaviour
{
    public float destroyX = -12f;
    private Tween blinkTween;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            blinkTween = sr.DOColor(Color.yellow, 0.2f)
                .SetLoops(4, LoopType.Yoyo)
                .OnComplete(() => sr.color = originalColor);
        }
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        float speed = 5f;
        if (DifficultyManager.Instance != null)
            speed = DifficultyManager.Instance.currentSpeed;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (blinkTween != null) blinkTween.Kill();
        transform.DOKill();
    }
}