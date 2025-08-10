using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Inventory))]
public class InventoryInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private InventoryController inventoryController;
    private Inventory inventory;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.inventory = inventory;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.inventory = null;
    }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
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
