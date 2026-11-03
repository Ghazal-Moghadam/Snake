
using UnityEngine;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    [Header("Snake Settings")]
    public int startingLength = 3;
    public float segmentSize = 0.9f;

    public enum Direction { Up, Down, Left, Right }

    private Direction currentDirection = Direction.Right;
    private Direction nextDirection = Direction.Right;

    private List<Vector2Int> bodyPositions = new List<Vector2Int>();
    private List<Transform> bodySegments = new List<Transform>();

    private float moveTimer = 0f;
    private bool isInitialized = false;

    void Update()
    {
        if (!isInitialized || GameManager.Instance == null)
            return;

        if (!GameManager.Instance.isPlaying ||
            GameManager.Instance.isPaused)
            return;

        HandleInput();

        moveTimer += Time.deltaTime;

        if (moveTimer >= GameManager.Instance.currentMoveInterval)
        {
            Move();
            moveTimer = 0f;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (currentDirection != Direction.Down)
                nextDirection = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (currentDirection != Direction.Up)
                nextDirection = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentDirection != Direction.Right)
                nextDirection = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentDirection != Direction.Left)
                nextDirection = Direction.Right;
        }
    }

    public void Initialize()
    {
        ClearBody();

        Vector2Int startPos = new Vector2Int(-5, 0);

        for (int i = 0; i < startingLength; i++)
        {
            Vector2Int pos = new Vector2Int(startPos.x - i, 0);

            bodyPositions.Add(pos);
            CreateSegment(pos, i == 0);
        }

        currentDirection = Direction.Right;
        nextDirection = Direction.Right;

        moveTimer = 0f;
        isInitialized = true;

        Debug.Log("Snake Initialized!");
    }

    void CreateSegment(Vector2Int pos, bool isHead)
    {
        GameObject segment = new GameObject(isHead ? "Head" : "Body");

        segment.transform.SetParent(transform);

        SpriteRenderer sr = segment.AddComponent<SpriteRenderer>();

        sr.sprite = isHead
            ? GameManager.Instance.GetHeadSprite()
            : GameManager.Instance.GetBodySprite();

        sr.color = isHead
            ? GameManager.Instance.GetSnakeHeadColor()
            : GameManager.Instance.GetSnakeBodyColor();

        sr.sortingOrder = isHead ? 2 : 1;

        segment.transform.position =
            GameManager.Instance.GridToWorld(pos);

        segment.transform.localScale = Vector3.one * segmentSize;

        if (isHead)
            bodySegments.Insert(0, segment.transform);
        else
            bodySegments.Add(segment.transform);
    }

    void Move()
    {
        if (bodyPositions.Count == 0)
            return;

        currentDirection = nextDirection;

        Vector2Int newHead = bodyPositions[0];

        switch (currentDirection)
        {
            case Direction.Up: newHead.y++; break;
            case Direction.Down: newHead.y--; break;
            case Direction.Left: newHead.x--; break;
            case Direction.Right: newHead.x++; break;
        }

        bool ateApple =
            GameManager.Instance.foodSpawner != null &&
            newHead == GameManager.Instance.foodSpawner.appleGridPosition;

        bool hitsBody = bodyPositions.Contains(newHead);

        bool hitsTail =
            newHead == bodyPositions[bodyPositions.Count - 1] &&
            !ateApple;

        if (!GameManager.Instance.IsInBounds(newHead) ||
            (hitsBody && !hitsTail))
        {
            Debug.Log("Snake Collision!");
            GameManager.Instance.LoseLife();
            return;
        }

        bodyPositions.Insert(0, newHead);
        CreateSegment(newHead, true);

        if (bodySegments.Count > 1)
        {
            SpriteRenderer oldHead =
                bodySegments[1].GetComponent<SpriteRenderer>();

            oldHead.sprite = GameManager.Instance.GetBodySprite();
            oldHead.color = GameManager.Instance.GetSnakeBodyColor();
            oldHead.sortingOrder = 1;
        }

        if (ateApple)
        {
            Debug.Log("APPLE EATEN!");

            GameManager.Instance.AddScore(
                GameManager.Instance.pointsPerApple
            );

            Debug.Log("Trying to spawn next apple...");

            if (GameManager.Instance.foodSpawner != null)
            {
                GameManager.Instance.foodSpawner.SpawnApple();

                Debug.Log("SpawnApple called successfully!");
            }
            else
            {
                Debug.LogError("FoodSpawner reference is NULL!");
            }
        }
        else
        {
            bodyPositions.RemoveAt(bodyPositions.Count - 1);

            Transform tail = bodySegments[bodySegments.Count - 1];

            bodySegments.RemoveAt(bodySegments.Count - 1);

            if (tail != null)
                Destroy(tail.gameObject);
        }

        Debug.Log("Snake Length: " + bodyPositions.Count);
    }

    void ClearBody()
    {
        foreach (Transform segment in bodySegments)
        {
            if (segment != null)
                Destroy(segment.gameObject);
        }

        bodySegments.Clear();
        bodyPositions.Clear();
    }

    public void ResetSnake()
    {
        Initialize();
    }

    public List<Vector2Int> GetBodyPositions()
    {
        return bodyPositions;
    }
}
