using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Manages all UI elements for the Snake game.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("HUD References")]
    public Text scoreText;
    public Text livesText;
    public Text bestScoreText;
    public Text statusText;
    public Text titleText;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject hudPanel;

    [Header("Game Over Text")]
    public Text finalScoreText;
    public Text bestScoreGameOverText;

    [Header("Buttons")]
    public Button restartButton;
    public Button pauseResumeButton;

    void Start()
    {
        // Hide panels initially
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (pausePanel != null)
            pausePanel.SetActive(false);

        // Set up button listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);

        if (pauseResumeButton != null)
            pauseResumeButton.onClick.AddListener(OnPauseResumeClicked);

        // Set title
        if (titleText != null)
            titleText.text = "SNAKE";
    }

    public void UpdateHUD(int score, int lives, int bestScore, string status)
    {
        if (scoreText != null)
            scoreText.text = "SCORE: " + score.ToString();
        if (livesText != null)
            livesText.text = "LIVES: " + lives.ToString();
        if (bestScoreText != null)
            bestScoreText.text = "BEST: " + bestScore.ToString();
        if (statusText != null)
            statusText.text = status;
    }

    public void ShowGameOver(int finalScore, int bestScore)
    {
        if (hudPanel != null)
            hudPanel.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "FINAL SCORE: " + finalScore.ToString();
        if (bestScoreGameOverText != null)
            bestScoreGameOverText.text = "BEST SCORE: " + bestScore.ToString();
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (hudPanel != null)
            hudPanel.SetActive(true);
    }

    public void TogglePausePanel(bool isPaused)
    {
        if (pausePanel != null)
            pausePanel.SetActive(isPaused);

        if (pauseResumeButton != null)
        {
            Text buttonText = pauseResumeButton.GetComponentInChildren<Text>();
            if (buttonText != null)
                buttonText.text = isPaused ? "RESUME" : "PAUSE";
        }
    }

    void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    void OnPauseResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    void Update()
    {
        // Space key for pause/resume
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.TogglePause();
        }

        // R key for restart (when game over)
        if (Input.GetKeyDown(KeyCode.R) && GameManager.Instance.isGameOver)
        {
            GameManager.Instance.RestartGame();
        }
    }
}