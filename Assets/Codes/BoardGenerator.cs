
using UnityEngine;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    [Header("Board Settings")]
    public GameObject nodePrefab;
    public int width = 7;
    public int height = 7;
    public float spacing = 1f;

    [Header("Maze Settings")]
    [Range(0f, 1f)]
    public float openingChance = 0.35f;

    private Cell[,] cells;

    private class Cell
    {
        public bool visited = false;

        public bool top = true;
        public bool bottom = true;
        public bool left = true;
        public bool right = true;
    }

    void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        // Clear previous board
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Create cells
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell();
            }
        }

        // Generate basic maze
        GenerateMaze();

        // Add more open passages
        AddExtraConnections();

        EnforceOuterWalls();

        void EnforceOuterWalls()
        {
            for (int x = 0; x < width; x++)
            {
                cells[x, 0].top = true;
                cells[x, height - 1].bottom = true;
            }

            for (int y = 0; y < height; y++)
            {
                cells[0, y].left = true;
                cells[width - 1, y].right = true;
            }
        }

        // Create visual board
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject node = Instantiate(
                    nodePrefab,
                    new Vector3(
                        x * spacing,
                        -y * spacing,
                        0
                    ),
                    Quaternion.identity,
                    transform
                );

                SetNodeSprites(node, x, y);
            }
        }
    }

    // --------------------------------
    // MAZE GENERATION
    // --------------------------------

    void GenerateMaze()
    {
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        Vector2Int current = new Vector2Int(
            Random.Range(0, width),
            Random.Range(0, height)
        );

        cells[current.x, current.y].visited = true;
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Peek();

            List<Vector2Int> neighbors = new List<Vector2Int>();

            // Top
            if (current.y > 0 &&
                !cells[current.x, current.y - 1].visited)
            {
                neighbors.Add(new Vector2Int(
                    current.x, current.y - 1));
            }

            // Bottom
            if (current.y < height - 1 &&
                !cells[current.x, current.y + 1].visited)
            {
                neighbors.Add(new Vector2Int(
                    current.x, current.y + 1));
            }

            // Left
            if (current.x > 0 &&
                !cells[current.x - 1, current.y].visited)
            {
                neighbors.Add(new Vector2Int(
                    current.x - 1, current.y));
            }

            // Right
            if (current.x < width - 1 &&
                !cells[current.x + 1, current.y].visited)
            {
                neighbors.Add(new Vector2Int(
                    current.x + 1, current.y));
            }

            if (neighbors.Count > 0)
            {
                Vector2Int next = neighbors[
                    Random.Range(0, neighbors.Count)
                ];

                RemoveWall(current, next);

                cells[next.x, next.y].visited = true;
                stack.Push(next);
            }
            else
            {
                stack.Pop();
            }
        }
    }

    // --------------------------------
    // OPEN EXTRA PATHS
    // --------------------------------

    void AddExtraConnections()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Open right wall
                if (x < width - 1 &&
                    Random.value < openingChance)
                {
                    RemoveWall(
                        new Vector2Int(x, y),
                        new Vector2Int(x + 1, y)
                    );
                }

                // Open bottom wall
                if (y < height - 1 &&
                    Random.value < openingChance)
                {
                    RemoveWall(
                        new Vector2Int(x, y),
                        new Vector2Int(x, y + 1)
                    );
                }
            }
        }
    }

    // --------------------------------
    // REMOVE WALL BETWEEN TWO CELLS
    // --------------------------------

    void RemoveWall(Vector2Int current, Vector2Int next)
    {
        if (next.x > current.x)
        {
            cells[current.x, current.y].right = false;
            cells[next.x, next.y].left = false;
        }
        else if (next.x < current.x)
        {
            cells[current.x, current.y].left = false;
            cells[next.x, next.y].right = false;
        }
        else if (next.y > current.y)
        {
            cells[current.x, current.y].bottom = false;
            cells[next.x, next.y].top = false;
        }
        else if (next.y < current.y)
        {
            cells[current.x, current.y].top = false;
            cells[next.x, next.y].bottom = false;
        }
    }

    // --------------------------------
    // CREATE NODE SPRITES
    // --------------------------------


    void SetNodeSprites(GameObject node, int x, int y)
    {
        Cell cell = cells[x, y];

        // Hide all sprites first
        foreach (Transform child in node.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Always show floor
        SetTagActive(node, "Center", true);

        // Straight walls
        SetTagActive(node, "Top", cell.top);
        SetTagActive(node, "Bottom", cell.bottom);
        SetTagActive(node, "Left", cell.left);
        SetTagActive(node, "Right", cell.right);

        // Corner walls
        SetTagActive(node, "Top_Left",
            cell.top && cell.left);

        SetTagActive(node, "Top_Right",
            cell.top && cell.right);

        SetTagActive(node, "Bottom_Left",
            cell.bottom && cell.left);

        SetTagActive(node, "Bottom_Right",
            cell.bottom && cell.right);
    }


    void SetTagActive(GameObject node, string tag, bool active)
    {
        foreach (Transform child in node.transform)
        {
            if (child.CompareTag(tag))
            {
                child.gameObject.SetActive(active);
                return;
            }
        }

        Debug.LogError(
            "Could not find child with tag: " + tag
        );
    }
}
