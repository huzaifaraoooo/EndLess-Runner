using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 12f;
    public float fixedX = -6f;
    public GameObject dustPrefab;
    public int maxJumps = 2;

    private Rigidbody2D rb;
    private int jumpCount = 0;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(fixedX, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (isDead) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (jumpCount < maxJumps) Jump();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (Mathf.Abs(transform.position.x - fixedX) > 0.1f)
            transform.position = new Vector3(fixedX, transform.position.y, transform.position.z);
    }

    void Jump()
    {
        rb.velocity = new Vector2(0, jumpForce);
        jumpCount++;

        transform.DOKill();
        transform.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                if (transform != null)
                    transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
            });
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            SpawnDust();

            transform.DOKill();
            transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.08f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    if (transform != null)
                        transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
                });
        }

        if (collision.gameObject.CompareTag("Obstacle")) Die();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) jumpCount = 0;
    }

    void SpawnDust()
    {
        if (dustPrefab == null) return;
        Vector3 dustPos = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
        GameObject dust = Instantiate(dustPrefab, dustPos, Quaternion.identity);

        dust.transform.DOScale(1.5f, 0.4f).SetEase(Ease.OutQuad);
        SpriteRenderer sr = dust.GetComponent<SpriteRenderer>();
        if (sr != null) sr.DOFade(0f, 0.4f).SetEase(Ease.InQuad);

        Destroy(dust, 0.5f);
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        Camera.main.transform.DOShakePosition(0.5f, 0.3f, 10, 90).SetEase(Ease.OutQuad);
        GameManager.Instance.GameOver();
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}