
using UnityEngine;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    [Header("Apple Settings")]
    public GameObject applePrefab;

    private GameObject currentApple;

    public Vector2Int appleGridPosition { get; private set; }

    public void SpawnApple()
    {
        int gridWidth = GameManager.Instance.gridWidth;
        int gridHeight = GameManager.Instance.gridHeight;

        List<Vector2Int> snakePositions =
            GameManager.Instance.snakeController.GetBodyPositions();

        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        // Check every cell on the board
        int minX = -gridWidth / 2;
        int minY = -gridHeight / 2;

        for (int x = minX; x < minX + gridWidth; x++)
        {
            for (int y = minY; y < minY + gridHeight; y++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (!snakePositions.Contains(position))
                {
                    emptyPositions.Add(position);
                }
            }
        }

        // No empty cells available
        if (emptyPositions.Count == 0)
        {
            if (currentApple != null)
            {
                Destroy(currentApple);
                currentApple = null;
            }

            Debug.Log("No empty spaces left on the board!");
            return;
        }

        // Choose a random empty cell
        Vector2Int randomPosition =
            emptyPositions[Random.Range(0, emptyPositions.Count)];

        appleGridPosition = randomPosition;

        // Remove previous apple
        if (currentApple != null)
        {
            Destroy(currentApple);
        }

        // Create new apple
        currentApple = new GameObject("Apple");

        currentApple.transform.position =
            GameManager.Instance.GridToWorld(randomPosition);

        SpriteRenderer sr =
            currentApple.AddComponent<SpriteRenderer>();

        sr.sprite = GameManager.Instance.GetAppleSprite();
        sr.color = GameManager.Instance.GetAppleColor();
        sr.sortingOrder = 2;

        // Apple size = 3
        currentApple.transform.localScale = Vector3.one * 3f;

        Debug.Log("New apple spawned at: " + randomPosition);
        Debug.Log("Empty positions remaining: " + emptyPositions.Count);
    }
}
