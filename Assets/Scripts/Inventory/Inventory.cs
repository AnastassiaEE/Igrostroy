using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private float tileSizeWidth = 32;
    private float tileSizeHeight = 32;

    private InventoryItem[,] inventoryItemSlot;

    private RectTransform rectTransform;
    private Vector2 positionOnTheGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();

    [SerializeField] private int inventorySizeWidth;
    [SerializeField] private int inventorySizeHeight;

    [SerializeField] GameObject inventoryItemPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(inventorySizeWidth, inventorySizeHeight);

        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 1, 1);
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
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / (tileSizeWidth));
        tileGridPosition.y = (int)(positionOnTheGrid.y / (tileSizeHeight));

        return tileGridPosition;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform itemRt = inventoryItem.GetComponent<RectTransform>();
        itemRt.SetParent(this.rectTransform, false); 
        itemRt.localScale = Vector3.one;

        inventoryItemSlot[posX, posY] = inventoryItem;

        Vector2 position;
        position.x = posX * tileSizeWidth + tileSizeWidth / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight / 2);

        itemRt.anchoredPosition = position; 
    }

}
