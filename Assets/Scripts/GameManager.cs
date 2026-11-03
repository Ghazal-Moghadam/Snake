using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Central game manager for the Snake game.
/// Handles game state, score, lives, speed, grid configuration, and sprite creation.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Grid Settings")]
    public int gridWidth = 20;
    public int gridHeight = 15;
    public float cellSize = 1f;

    [Header("Game Settings")]
    public int startingLives = 3;
    public float initialMoveInterval = 0.3f;
    public float speedIncrement = 0.02f;
    public int pointsPerApple = 10;

    [Header("References")]
    public SnakeController snakeController;
    public FoodSpawner foodSpawner;
    public UIManager uiManager;

    [Header("Colors")]
    public Color backgroundColor = new Color(0.08f, 0.08f, 0.17f);     // #15152B
    public Color boardColor = new Color(0.14f, 0.14f, 0.26f);          // #242442
    public Color gridColor = new Color(0.22f, 0.22f, 0.36f);           // #39395B
    public Color snakeHeadColor = new Color(0.21f, 0.88f, 0.76f);      // #35E0C1
    public Color snakeBodyColor = new Color(0.55f, 0.89f, 0.43f);      // #8BE36D
    public Color appleColor = new Color(1.0f, 0.36f, 0.43f);           // #FF5B6E

    // Game state
    public int currentScore { get; private set; }
    public int bestScore { get; private set; }
    public int currentLives { get; private set; }
    public float currentMoveInterval { get; private set; }
    public bool isPlaying { get; private set; }
    public bool isPaused { get; private set; }
    public bool isGameOver { get; private set; }

    // Sprites created at runtime
    private Texture2D whiteTexture;
    private Sprite headSprite;
    private Sprite bodySprite;
    private Sprite appleSprite;

    // Grid bounds
    public float boardLeft { get; private set; }
    public float boardRight { get; private set; }
    public float boardTop { get; private set; }
    public float boardBottom { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        CreateSprites();
        SetupBoardBounds();
        LoadBestScore();
        InitializeGame();
    }

    void CreateSprites()
    {
        // Generate a small white texture for sprites
        whiteTexture = new Texture2D(16, 16);
        Color[] pixels = new Color[16 * 16];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;
        whiteTexture.SetPixels(pixels);
        whiteTexture.Apply();

        headSprite = Sprite.Create(whiteTexture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        bodySprite = Sprite.Create(whiteTexture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        appleSprite = Sprite.Create(whiteTexture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
    }

    void SetupBoardBounds()
    {
        float halfWidth = gridWidth * cellSize / 2f;
        float halfHeight = gridHeight * cellSize / 2f;
        boardLeft = -halfWidth;
        boardRight = halfWidth;
        boardTop = halfHeight;
        boardBottom = -halfHeight;
    }

    void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("SnakeBestScore", 0);
    }

    void SaveBestScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("SnakeBestScore", bestScore);
            PlayerPrefs.Save();
        }
    }

    public void InitializeGame()
    {
        currentScore = 0;
        currentLives = startingLives;
        currentMoveInterval = initialMoveInterval;
        isPlaying = true;
        isPaused = false;
        isGameOver = false;

        if (uiManager != null)
            uiManager.UpdateHUD(currentScore, currentLives, bestScore, "Playing");

        if (snakeController != null)
            snakeController.Initialize();

        if (foodSpawner != null)
            foodSpawner.SpawnApple();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        // Increase speed slightly
        currentMoveInterval = Mathf.Max(0.08f, currentMoveInterval - speedIncrement);

        if (uiManager != null)
            uiManager.UpdateHUD(currentScore, currentLives, bestScore, "Playing");
    }

    public void LoseLife()
    {
        currentLives--;

        if (uiManager != null)
            uiManager.UpdateHUD(currentScore, currentLives, bestScore, "Life Lost");

        if (currentLives <= 0)
        {
            GameOver();
        }
        else
        {
            // Reset snake but keep score
            if (snakeController != null)
                snakeController.ResetSnake();

            if (foodSpawner != null)
                foodSpawner.SpawnApple();
        }
    }

    void GameOver()
    {
        isPlaying = false;
        isGameOver = true;
        SaveBestScore();

        if (uiManager != null)
            uiManager.ShowGameOver(currentScore, bestScore);
    }

    public void RestartGame()
    {
        isGameOver = false;
        InitializeGame();

        if (uiManager != null)
            uiManager.HideGameOver();
    }

    public void TogglePause()
    {
        if (!isPlaying || isGameOver) return;

        isPaused = !isPaused;

        if (uiManager != null)
            uiManager.TogglePausePanel(isPaused);
    }

    public Sprite GetHeadSprite() => headSprite;
    public Sprite GetBodySprite() => bodySprite;
    public Sprite GetAppleSprite() => appleSprite;
    public Color GetSnakeHeadColor() => snakeHeadColor;
    public Color GetSnakeBodyColor() => snakeBodyColor;
    public Color GetAppleColor() => appleColor;
    public Color GetBoardColor() => boardColor;
    public Color GetGridColor() => gridColor;

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / cellSize),
            Mathf.RoundToInt(worldPos.y / cellSize)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0f);
    }

    public bool IsInBounds(Vector2Int gridPos)
    {
        return gridPos.x >= -gridWidth / 2 && gridPos.x < gridWidth / 2 &&
               gridPos.y >= -gridHeight / 2 && gridPos.y < gridHeight / 2;
    }
}