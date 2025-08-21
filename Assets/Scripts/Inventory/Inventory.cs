using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static float cellWidth = 32;
    public static float cellHeight = 32;

    private InventoryItem[,] itemGrid;

    private RectTransform rectTransform;
    private Vector2Int cellCoords = new Vector2Int();

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridWidth, gridHeight);
    }

    private void Init(int width, int height)
    {
        itemGrid = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * cellWidth, height * cellHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetCellCoords(Vector2 mousePosition)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPoint);

        cellCoords.x = (int)(localPoint.x / cellWidth);
        cellCoords.y = (int)(-localPoint.y / cellHeight);

        return cellCoords;
    }

    private void ClearItemFromGrid(InventoryItem itemToClear)
    {
        for (int ix = 0; ix < itemToClear.itemData.width; ix++)
        {
            for (int iy = 0; iy < itemToClear.itemData.height; iy++)
            {
                itemGrid[itemToClear.gridPosition.x + ix, itemToClear.gridPosition.y + iy] = null;
            }
        }
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem itemToPickUp = itemGrid[x, y];

        if (itemToPickUp == null) return null;
        ClearItemFromGrid(itemToPickUp);

        return itemToPickUp;
    }

    public bool PlaceItem(InventoryItem itemToPlace, int posX, int posY, ref InventoryItem existingItem)
    {
        if (!IsWithinBounds(posX, posY, itemToPlace.itemData.width, itemToPlace.itemData.height)) return false;

        if (!CanPlaceItem(posX, posY, itemToPlace.itemData.width, itemToPlace.itemData.height, ref existingItem))
        {
            existingItem = null;
            return false;
        }

        if (existingItem != null)
        {
            ClearItemFromGrid(existingItem);
        }

        PlaceItem(itemToPlace, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem itemToPlace, int posX, int posY)
    {
        RectTransform itemRectTransform = itemToPlace.GetComponent<RectTransform>();
        itemRectTransform.SetParent(this.rectTransform, false);

        for (int x = 0; x < itemToPlace.itemData.width; x++)
        {
            for (int y = 0; y < itemToPlace.itemData.height; y++)
            {
                itemGrid[posX + x, posY + y] = itemToPlace;
            }
        }

        itemToPlace.gridPosition.x = posX;
        itemToPlace.gridPosition.y = posY;
        Vector2 itemPosition = GetItemLocalPosition(itemToPlace, posX, posY);

        itemRectTransform.localPosition = itemPosition;
    }

    public Vector2 GetItemLocalPosition(InventoryItem itemToPlace, int posX, int posY)
    {
        Vector2 position;
        position.x = posX * cellWidth + cellWidth * itemToPlace.itemData.width / 2;
        position.y = -(posY * cellHeight + cellHeight * itemToPlace.itemData.height / 2);
        return position;
    }

    private bool CanPlaceItem(int posX, int posY, int width, int height, ref InventoryItem existingItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (itemGrid[posX + x, posY + y] != null)
                {
                    if (existingItem == null)
                    {
                        existingItem = itemGrid[posX + x, posY + y];
                    }
                    else
                    {
                        if (existingItem != itemGrid[posX + x, posY + y]) return false;
                    }
                }
            }
        }

        return true;
    }

    private bool HasAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (itemGrid[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }


    private bool IsWithinGrid(int posX, int posY)
    {
        if (posX < 0 || posY < 0) return false;
        if (posX >= gridWidth || posY >= gridHeight) return false;
        return true;
    }

    public bool IsWithinBounds(int posX, int posY, int width, int height)
    {
        int rightX = posX + width - 1;
        int bottomY = posY + height - 1;

        if (!IsWithinGrid(posX, posY)) return false;
        if (!IsWithinGrid(rightX, bottomY)) return false;

        return true;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return itemGrid[x, y];
    }

    public Vector2Int? FindSpaceForItem(InventoryItem itemToInsert)
    {
        int height = gridHeight - itemToInsert.itemData.height + 1;
        int width = gridWidth - itemToInsert.itemData.width + 1;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (HasAvailableSpace(x, y, itemToInsert.itemData.width, itemToInsert.itemData.height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }
}
