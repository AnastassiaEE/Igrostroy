using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * Inventory.cellWidth;
        size.y = targetItem.itemData.height * Inventory.cellHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPosition(Inventory targetInventory, InventoryItem targetItem)
    {
        highlighter.SetParent(targetInventory.GetComponent<RectTransform>());

        Vector2 position = targetInventory.GetItemLocalPosition(
            targetItem,
            targetItem.gridPosition.x,
            targetItem.gridPosition.y
            );

        highlighter.localPosition = position;
    }
}
