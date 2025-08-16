using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemData itemData;
    public Vector2Int gridPosition;

    internal void Set(InventoryItemData inventoryItemData)
    {
        itemData = inventoryItemData;
        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * Inventory.cellWidth;
        size.y = itemData.height * Inventory.cellHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
