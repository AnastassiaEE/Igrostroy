using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static float tileSizeWidth = 32;
    public static float tileSizeHeight = 32;

    private InventoryItem[,] inventoryItemSlot;

    private RectTransform rectTransform;
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();

    [SerializeField] private int inventorySizeWidth;
    [SerializeField] private int inventorySizeHeight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(inventorySizeWidth, inventorySizeHeight);
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        for (int ix = 0; ix < toReturn.itemData.width; ix++)
        {
            for (int iy = 0; iy < toReturn.itemData.height; iy++)
            {
                inventoryItemSlot[toReturn.onGridPositionX + ix, toReturn.onGridPositionY + iy] = null;
            }
        }
        return toReturn;
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPoint);

        tileGridPosition.x = (int)(localPoint.x / tileSizeWidth);
        tileGridPosition.y = (int)(-localPoint.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if (!CheckBoundries(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height)) return false;

        RectTransform itemRt = inventoryItem.GetComponent<RectTransform>();
        itemRt.SetParent(this.rectTransform, false); 
        
        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for (int y = 0; y < inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position;
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.height / 2);

        itemRt.localPosition = position;

        return true;
    }

    private bool CheckPosition(int posX, int posY)
    {
        if (posX < 0 || posY < 0) return false;
        if (posX >= inventorySizeWidth || posY >= inventorySizeHeight) return false;
        return true;
    }

    private bool CheckBoundries(int posX, int posY, int width, int height)
    {
        int rightX = posX + width - 1;
        int bottomY = posY + height - 1;

        if (!CheckPosition(posX, posY)) return false;
        if (!CheckPosition(rightX, bottomY)) return false;

        return true;
    }
}
