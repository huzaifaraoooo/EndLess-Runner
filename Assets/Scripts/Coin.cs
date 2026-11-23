using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public float destroyX = -12f;
    private bool isCollected = false;
    private Tween spinTween;
    private Tween bobTween;

    void Start()
    {
        // Coin spin
        spinTween = transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);

        // Coin bobble
        bobTween = transform.DOMoveY(transform.position.y + 0.2f, 0.6f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
        if (isCollected) return;

        float speed = 5f;
        if (DifficultyManager.Instance != null)
            speed = DifficultyManager.Instance.currentSpeed;

        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < destroyX)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        isCollected = true;
        GameManager.Instance.AddScore();

        // Saari purani tweens rok dein
        if (spinTween != null) spinTween.Kill();
        if (bobTween != null) bobTween.Kill();
        transform.DOKill();

        // Collect effect
        transform.DOScale(0f, 0.15f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (gameObject != null) Destroy(gameObject);
            });
    }

    void OnDestroy()
    {
        // Destroy hone par saari tweens clean
        if (spinTween != null) spinTween.Kill();
        if (bobTween != null) bobTween.Kill();
        transform.DOKill();
    }
}