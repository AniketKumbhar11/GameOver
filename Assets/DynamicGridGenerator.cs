using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    private int rows = 3; // Number of rows
    private int columns = 3; // Number of columns
    public Vector2 spacing = new Vector2(5f, 5f); // Spacing between blocks
    public GameObject blockPrefab; // Prefab for the grid blocks

    [Header("Container")]
    public RectTransform gridContainer; // Parent container (with GridLayoutGroup)

    void Start()
    {
       // GenerateGrid();
    }

    public void GenerateGrid(List<CardData> _cardData,int _rows,int _columns)
    {
        rows = _rows;
        columns=_columns;
        if (gridContainer == null || blockPrefab == null)
        {
            Debug.LogError("Grid container or block prefab is not assigned!");
            return;
        }

        // Clear existing children
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Calculate block size
        Vector2 containerSize = gridContainer.rect.size;
        float blockWidth = (containerSize.x - (spacing.x * (columns - 1))) / columns;
        float blockHeight = (containerSize.y - (spacing.y * (rows - 1))) / rows;
        Vector2 blockSize = new Vector2(blockWidth, blockHeight);

        // Configure GridLayoutGroup
        GridLayoutGroup gridLayout = gridContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            gridLayout = gridContainer.gameObject.AddComponent<GridLayoutGroup>();
        }

        gridLayout.cellSize = blockSize;
        gridLayout.spacing = spacing;
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        // Create blocks
        for (int i = 0; i < _cardData.Count; i++)
        {
           GameObject block = Instantiate(blockPrefab, gridContainer);
           Card card = block.GetComponent<Card>();
            card.CardID = _cardData[i].CardID;
            card.isOpen = _cardData[i].IsOpen;
            card.endSprite = _cardData[i].CardSprite;
            block.name = $"Block_{i + 1}";
        }
    }
}
