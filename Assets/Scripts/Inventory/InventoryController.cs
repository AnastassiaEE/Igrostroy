using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public Inventory inventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory == null) return;
        Debug.Log(inventory.GetTileGridPosition(Input.mousePosition));
    }
}
