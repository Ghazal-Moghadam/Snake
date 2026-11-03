using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One-time setup script to configure all UI layouts, colors, and script references.
/// Attach to any GameObject and it will run on Start, then self-destruct.
/// </summary>
public class GameSetup : MonoBehaviour
{
    void Start()
    {
        SetupEverything();
        Destroy(this);
    }

    void SetupEverything()
    {
        Color bgColor = new Color(0.08f, 0.08f, 0.17f);
        Color boardColor = new Color(0.14f, 0.14f, 0.26f);
        Color snakeHeadColor = new Color(0.21f, 0.88f, 0.76f);
        Color snakeBodyColor = new Color(0.55f, 0.89f, 0.43f);
        Color appleColor = new Color(1.0f, 0.36f, 0.43f);
        Color accentColor = new Color(1.0f, 0.82f, 0.40f);
        Color whiteColor = Color.white;

        // Configure Canvas
        var canvasObj = GameObject.Find("Canvas");
        var canvas = canvasObj.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // HUD Panel
        var hudPanel = GameObject.Find("Canvas/HUDPanel");
        SetupRect(hudPanel, new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(20, -20), new Vector2(1200, 140));
        var hudImg = hudPanel.GetComponent<Image>();
        hudImg.color = new Color(0.14f, 0.14f, 0.26f, 0.85f);
        hudImg.raycastTarget = true;

        SetupText(GameObject.Find("Canvas/HUDPanel/TitleText"), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(30, 0), new Vector2(300, 50), "SNAKE", 40, whiteColor, TextAnchor.MiddleLeft, true);
        SetupText(GameObject.Find("Canvas/HUDPanel/ScoreText"), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(30, -40), new Vector2(250, 40), "SCORE: 0", 24, accentColor, TextAnchor.MiddleLeft, false);
        SetupText(GameObject.Find("Canvas/HUDPanel/LivesText"), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(300, -40), new Vector2(250, 40), "LIVES: 3", 24, whiteColor, TextAnchor.MiddleLeft, false);
        SetupText(GameObject.Find("Canvas/HUDPanel/BestScoreText"), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(30, -85), new Vector2(250, 40), "BEST: 0", 20, new Color(0.8f, 0.8f, 0.8f), TextAnchor.MiddleLeft, false);
        SetupText(GameObject.Find("Canvas/HUDPanel/StatusText"), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(300, -85), new Vector2(250, 40), "Playing", 20, new Color(0.6f, 0.9f, 1.0f), TextAnchor.MiddleLeft, false);

        // GameOver Panel
        var goPanel = GameObject.Find("Canvas/GameOverPanel");
        SetupRect(goPanel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(600, 500));
        var goImg = goPanel.GetComponent<Image>();
        goImg.color = new Color(0.08f, 0.08f, 0.17f, 0.95f);
        goImg.raycastTarget = true;
        goPanel.SetActive(false);

        SetupText(GameObject.Find("Canvas/GameOverPanel/GameOverTitle"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 120), new Vector2(500, 60), "GAME OVER", 48, appleColor, TextAnchor.MiddleCenter, true);
        SetupText(GameObject.Find("Canvas/GameOverPanel/FinalScoreText"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 30), new Vector2(500, 40), "FINAL SCORE: 0", 28, whiteColor, TextAnchor.MiddleCenter, false);
        SetupText(GameObject.Find("Canvas/GameOverPanel/BestScoreGOText"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -10), new Vector2(500, 40), "BEST SCORE: 0", 24, accentColor, TextAnchor.MiddleCenter, false);

        var restartBtnGo = GameObject.Find("Canvas/GameOverPanel/RestartButton");
        SetupRect(restartBtnGo, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -100), new Vector2(220, 50));
        var restartImg = restartBtnGo.GetComponent<Image>();
        restartImg.color = snakeHeadColor;
        restartImg.raycastTarget = true;
        SetupText(GameObject.Find("Canvas/GameOverPanel/RestartButton/ButtonText"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(220, 50), "RESTART", 24, bgColor, TextAnchor.MiddleCenter, true);

        // Pause Panel
        var pausePanel = GameObject.Find("Canvas/PausePanel");
        SetupRect(pausePanel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(500, 300));
        var pauseImg = pausePanel.GetComponent<Image>();
        pauseImg.color = new Color(0.08f, 0.08f, 0.17f, 0.9f);
        pauseImg.raycastTarget = true;
        pausePanel.SetActive(false);

        SetupText(GameObject.Find("Canvas/PausePanel/PauseTitle"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 80), new Vector2(400, 50), "PAUSED", 40, accentColor, TextAnchor.MiddleCenter, true);

        var resumeBtnGo = GameObject.Find("Canvas/PausePanel/ResumeButton");
        SetupRect(resumeBtnGo, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -20), new Vector2(200, 50));
        var resumeImg = resumeBtnGo.GetComponent<Image>();
        resumeImg.color = snakeBodyColor;
        resumeImg.raycastTarget = true;
        SetupText(GameObject.Find("Canvas/PausePanel/ResumeButton/ButtonText"), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(200, 50), "RESUME", 24, bgColor, TextAnchor.MiddleCenter, true);

        // GameBoard
        var boardBgGo = GameObject.Find("GameBoard/BoardBackground");
        var boardBgSR = boardBgGo.GetComponent<SpriteRenderer>();
        var tex = new Texture2D(16, 16);
        var pix = new Color[256];
        for (int i = 0; i < 256; i++) pix[i] = Color.white;
        tex.SetPixels(pix);
        tex.Apply();
        var boardSprite = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        boardBgSR.sprite = boardSprite;
        boardBgSR.color = boardColor;
        boardBgSR.sortingOrder = 0;
        boardBgGo.transform.localScale = new Vector3(20f, 15f, 1f);

        // Assign Script References
        var gmObj = GameObject.Find("GameManager");
        var gm = gmObj.GetComponent<GameManager>();
        gm.snakeController = GameObject.Find("Snake").GetComponent<SnakeController>();
        gm.foodSpawner = GameObject.Find("FoodSpawner").GetComponent<FoodSpawner>();
        gm.uiManager = GameObject.Find("Canvas/HUDPanel").AddComponent<UIManager>();

        var ui = gm.uiManager;
        ui.scoreText = GameObject.Find("Canvas/HUDPanel/ScoreText").GetComponent<Text>();
        ui.livesText = GameObject.Find("Canvas/HUDPanel/LivesText").GetComponent<Text>();
        ui.bestScoreText = GameObject.Find("Canvas/HUDPanel/BestScoreText").GetComponent<Text>();
        ui.statusText = GameObject.Find("Canvas/HUDPanel/StatusText").GetComponent<Text>();
        ui.titleText = GameObject.Find("Canvas/HUDPanel/TitleText").GetComponent<Text>();
        ui.gameOverPanel = GameObject.Find("Canvas/GameOverPanel");
        ui.pausePanel = GameObject.Find("Canvas/PausePanel");
        ui.hudPanel = GameObject.Find("Canvas/HUDPanel");
        ui.finalScoreText = GameObject.Find("Canvas/GameOverPanel/FinalScoreText").GetComponent<Text>();
        ui.bestScoreGameOverText = GameObject.Find("Canvas/GameOverPanel/BestScoreGOText").GetComponent<Text>();
        ui.restartButton = GameObject.Find("Canvas/GameOverPanel/RestartButton").GetComponent<Button>();
        ui.pauseResumeButton = GameObject.Find("Canvas/PausePanel/ResumeButton").GetComponent<Button>();

        Debug.Log("GameSetup: UI setup complete. All references assigned.");
    }

    void SetupRect(GameObject go, Vector2 anchorMin, Vector2 anchorMax, Vector2 piv, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.pivot = piv;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
    }

    void SetupText(GameObject go, Vector2 anchorMin, Vector2 anchorMax, Vector2 piv, Vector2 anchoredPos, Vector2 sizeDelta, string text, int size, Color color, TextAnchor align, bool bold)
    {
        SetupRect(go, anchorMin, anchorMax, piv, anchoredPos, sizeDelta);
        var t = go.GetComponent<Text>();
        t.text = text;
        t.fontSize = size;
        t.color = color;
        t.alignment = align;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (bold) t.fontStyle = FontStyle.Bold;
        t.supportRichText = true;
        t.horizontalOverflow = HorizontalWrapMode.Overflow;
        t.verticalOverflow = VerticalWrapMode.Overflow;
    }
}