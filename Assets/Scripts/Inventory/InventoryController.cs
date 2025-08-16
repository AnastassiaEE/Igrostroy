using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public Inventory inventory;

    private InventoryItem selectedItem;
    private InventoryItem existingItem;
    private RectTransform selectedItemRectTransform;

    [SerializeField] private List<InventoryItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (inventory == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            InteractWithItem();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab, canvasTransform, false).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        selectedItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        selectedItemRectTransform.SetParent(canvasTransform);

        int selectedItemId = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemId]);
    }

    private void InteractWithItem()
    {

        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * Inventory.cellWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * Inventory.cellHeight / 2;
        }

        Vector2Int cellCoords = inventory.GetCellCoords(position);

        if (selectedItem == null)
        {
            PickUpItem(cellCoords);
        }
        else
        {
            PlaceItem(cellCoords);
        }
    }

    private void PlaceItem(Vector2Int cellCoords)
    {
        bool placed = inventory.PlaceItem(selectedItem, cellCoords.x, cellCoords.y, ref existingItem);
        if (placed)
        {
            selectedItem = null;
            if (existingItem != null)
            {
                selectedItem = existingItem;
                existingItem = null;
                selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }

    private void PickUpItem(Vector2Int cellCoords)
    {
        selectedItem = inventory.PickUpItem(cellCoords.x, cellCoords.y);
        if (selectedItem != null)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void DragItem()
    {
        if (selectedItem != null)
        {
            selectedItemRectTransform.position = Input.mousePosition;
        }
    }
}
