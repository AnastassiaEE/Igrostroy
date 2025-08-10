using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;
}

