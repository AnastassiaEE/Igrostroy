using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    public Inventory selectedInventory;

    private InventoryItem selectedItem;
    private RectTransform rectTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }

        if (selectedInventory == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = selectedInventory.GetTileGridPosition(Input.mousePosition);
            if (selectedItem == null)
            {
                selectedItem = selectedInventory.PickUpItem(tileGridPosition.x, tileGridPosition.y);
                if(selectedItem != null)
                {
                    rectTransform = selectedItem.GetComponent<RectTransform>();
                }
            } else
            {
                selectedInventory.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                selectedItem = null;
            }
        } 
    }
}
