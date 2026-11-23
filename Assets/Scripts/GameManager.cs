using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver = false;

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI bonusText;
    public TextMeshProUGUI floatingTextPrefab;
    public GameObject restartButton;
    public GameObject gameOverPanel;

    [Header("Game Stats")]
    private int score = 0;
    private int coinCounter = 0;
    private float distance = 0f;
    private bool isCounting = true;
    private int lastMilestone = 0;

    [Header("Combo System")]
    private float lastCoinTime = 0f;
    private int comboCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (restartButton != null) restartButton.SetActive(false);
        if (bonusText != null) bonusText.gameObject.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        UpdateScoreUI();
        UpdateDistanceUI();
    }

    void Update()
    {
        if (!isGameOver && isCounting)
        {
            distance += Time.deltaTime * 5f;
            UpdateDistanceUI();
        }
    }

    public void AddScore()
    {
        if (isGameOver) return;

        score++;
        coinCounter++;

        // Score Text Pop
        if (scoreText != null)
        {
            scoreText.transform.DOKill();
            scoreText.transform.localScale = Vector3.one;
            scoreText.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 10, 1);
        }

        // Floating "+1" Text
        if (floatingTextPrefab != null && scoreText != null)
        {
            TextMeshProUGUI floatingText = Instantiate(
                floatingTextPrefab,
                scoreText.transform.position,
                Quaternion.identity,
                scoreText.transform.parent
            );

            floatingText.text = "+1";
            floatingText.color = Color.yellow;

            RectTransform rt = floatingText.GetComponent<RectTransform>();
            rt.DOAnchorPosY(rt.anchoredPosition.y + 100f, 0.8f).SetEase(Ease.OutQuad);

            floatingText.DOFade(0f, 0.8f).SetDelay(0.3f)
                .OnComplete(() =>
                {
                    if (floatingText != null) Destroy(floatingText.gameObject);
                });
        }

        // Combo System
        if (Time.time - lastCoinTime < 1f)
        {
            comboCount++;
            if (comboCount >= 5)
            {
                score += 3;
                Debug.Log("COMBO! +3 bonus");
                ShowComboText();
                comboCount = 0;
            }
        }
        else
        {
            comboCount = 0;
        }
        lastCoinTime = Time.time;

        UpdateScoreUI();

        // Bonus har 15 coins par
        if (coinCounter >= 15)
        {
            GiveBonus();
        }
    }

    void ShowComboText()
    {
        if (bonusText != null)
        {
            bonusText.DOKill();
            bonusText.text = "COMBO! +3";
            bonusText.gameObject.SetActive(true);
            bonusText.transform.localScale = Vector3.zero;
            bonusText.color = Color.cyan;

            bonusText.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    if (bonusText != null)
                    {
                        bonusText.transform.DOScale(1f, 0.1f);
                        bonusText.DOFade(0f, 0.3f).SetDelay(0.3f)
                            .OnComplete(() =>
                            {
                                if (bonusText != null)
                                {
                                    bonusText.gameObject.SetActive(false);
                                    bonusText.color = new Color(bonusText.color.r, bonusText.color.g, bonusText.color.b, 1f);
                                }
                            });
                    }
                });
        }
    }

    void GiveBonus()
    {
        score += 5;
        coinCounter = 0;
        UpdateScoreUI();

        if (bonusText != null)
        {
            bonusText.DOKill();
            bonusText.text = "BONUS +5!";
            bonusText.gameObject.SetActive(true);
            bonusText.transform.localScale = Vector3.zero;
            bonusText.color = Color.yellow;

            // Rainbow color animation
            Sequence rainbowSeq = DOTween.Sequence();
            rainbowSeq.Append(bonusText.DOColor(Color.magenta, 0.15f));
            rainbowSeq.Append(bonusText.DOColor(Color.cyan, 0.15f));
            rainbowSeq.Append(bonusText.DOColor(Color.yellow, 0.15f));
            rainbowSeq.Append(bonusText.DOColor(Color.green, 0.15f));

            bonusText.transform.DOScale(1.5f, 0.3f).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    if (bonusText != null)
                    {
                        bonusText.transform.DOScale(1f, 0.2f);
                        bonusText.DOFade(0f, 0.5f).SetDelay(0.5f)
                            .OnComplete(() =>
                            {
                                if (bonusText != null)
                                {
                                    bonusText.gameObject.SetActive(false);
                                    bonusText.color = new Color(bonusText.color.r, bonusText.color.g, bonusText.color.b, 1f);
                                }
                            });
                    }
                });
        }

        Debug.Log("BONUS! +5 coins added. Total score: " + score);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void UpdateDistanceUI()
    {
        if (distanceText != null)
        {
            int displayDistance = Mathf.FloorToInt(distance);
            distanceText.text = "Distance: " + displayDistance + "m";

            // Distance ke hisaab se color change
            if (displayDistance > 500) distanceText.color = Color.cyan;
            else if (displayDistance > 200) distanceText.color = Color.green;
            else if (displayDistance > 100) distanceText.color = Color.yellow;

            // Har 100m par milestone flash
            if (displayDistance >= lastMilestone + 100)
            {
                lastMilestone = displayDistance;
                distanceText.transform.DOKill();
                distanceText.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 10, 1);
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        isCounting = false;

        // Game Over Panel Fade In
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            CanvasGroup cg = gameOverPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = gameOverPanel.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
            cg.DOFade(1f, 0.5f).SetEase(Ease.OutQuad);
        }

        // Restart Button Pop-In
        if (restartButton != null)
        {
            restartButton.SetActive(true);
            restartButton.transform.DOKill();
            restartButton.transform.localScale = Vector3.zero;
            restartButton.transform.DOScale(1f, 0.5f)
                .SetEase(Ease.OutBack)
                .SetDelay(0.3f);
        }
    }

    public void RestartGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDestroy()
    {
        // Saari tweens globally clean karein
        DOTween.KillAll();
    }
}