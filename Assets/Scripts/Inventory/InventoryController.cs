using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private Inventory hoveredInventory;

    public Inventory HoveredInventory { 
        get => hoveredInventory; 
        set {
            hoveredInventory = value;
            highlight.SetParent(value);
        }
    }

    private InventoryItem selectedItem;
    private InventoryItem existingItem;
    private InventoryItem itemToHighlight;
    private RectTransform selectedItemRectTransform;
    private InventoryHighlight highlight;
    private Vector2Int oldCellCoords;

    [SerializeField] private List<InventoryItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvasTransform;

    

    

    private void Awake()
    {
        highlight = GetComponent<InventoryHighlight>();
    }

    // Update is called once per frame
    private void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            InsertRandomItem();
        }

        if (hoveredInventory == null)
        {
            highlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            InteractWithItem();
        }
    }

    private void InsertRandomItem()
    {
        if (hoveredInventory == null) return;
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? cellCoords = hoveredInventory.FindSpaceForItem(itemToInsert);
        if (cellCoords == null) return;

        hoveredInventory.PlaceItem(itemToInsert, cellCoords.Value.x, cellCoords.Value.y);
    }

    private void HandleHighlight()
    {
        Vector2Int cellCoords = GetCellCoords();

        if (oldCellCoords == cellCoords) return;

        oldCellCoords = cellCoords;

        if (selectedItem == null)
        {
            itemToHighlight = hoveredInventory.GetItem(cellCoords.x, cellCoords.y);

            if (itemToHighlight != null)
            {    
                highlight.Show(true);  
                highlight.SetSize(itemToHighlight);
                highlight.SetPosition(hoveredInventory, itemToHighlight);
            }
            else
            {
                highlight.Show(false);
            }
        } else
        {
            highlight.Show(hoveredInventory.IsWithinBounds(
                cellCoords.x,
                cellCoords.y,
                selectedItem.itemData.width,
                selectedItem.itemData.height
                ));
            highlight.SetSize(selectedItem);
            highlight.SetPosition(hoveredInventory, selectedItem, cellCoords.x, cellCoords.y);
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
        Vector2Int cellCoords = GetCellCoords();

        if (selectedItem == null)
        {
            PickUpItem(cellCoords);
        }
        else
        {
            PlaceItem(cellCoords);
        }
    }

    private Vector2Int GetCellCoords()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * Inventory.cellWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * Inventory.cellHeight / 2;
        }

        Vector2Int cellCoords = hoveredInventory.GetCellCoords(position);
        return cellCoords;
    }

    private void PlaceItem(Vector2Int cellCoords)
    {
        bool placed = hoveredInventory.PlaceItem(selectedItem, cellCoords.x, cellCoords.y, ref existingItem);
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
        selectedItem = hoveredInventory.PickUpItem(cellCoords.x, cellCoords.y);
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
