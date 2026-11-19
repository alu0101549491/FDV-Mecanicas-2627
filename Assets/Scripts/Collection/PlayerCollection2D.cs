using UnityEngine;
using TMPro;

public class PlayerCollection2D : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField, Min(1)] private int pointsForJumpUpgrade = 2;
    [SerializeField, Min(0f)] private float jumpPowerIncrease = 2.5f;

    private PlayerMovement playerMovement;
    private PlayerMovementRaycast playerMovementRaycast;
    private int score;
    private int nextUpgradeScore;

    public int Score => score;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerMovementRaycast = GetComponent<PlayerMovementRaycast>();
        nextUpgradeScore = pointsForJumpUpgrade;
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        if (points <= 0) return;

        score += points;
        while (score >= nextUpgradeScore)
        {
            IncreaseJumpPower();
            nextUpgradeScore += pointsForJumpUpgrade;
        }

        UpdateScoreText();
    }

    private void IncreaseJumpPower()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseJumpPower(jumpPowerIncrease);
        }

        if (playerMovementRaycast != null)
        {
            playerMovementRaycast.IncreaseJumpPower(jumpPowerIncrease);
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Puntuación: {score}";
        }
    }
}
