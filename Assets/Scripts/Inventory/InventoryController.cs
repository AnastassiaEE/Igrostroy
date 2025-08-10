using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public Inventory inventory;

    private InventoryItem selectedItem;
    private RectTransform rectTransform;

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

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);

        int selectedItemId = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemId]);
    }

    private void InteractWithItem()
    {
        Vector2Int tileGridPosition = inventory.GetTileGridPosition(Input.mousePosition);
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        inventory.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItem = null;
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = inventory.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void DragItem()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
