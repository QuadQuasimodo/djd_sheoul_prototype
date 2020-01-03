using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory
{
    private CanvasManager canvasManager;
    public PlayerInventory(CanvasManager cm)
    {
        canvasManager = cm;
    }
    
    List<InventoryPickup> inventory;

    public void AddToInventory(InventoryPickup item)
    {
        inventory.Add(item);
        UpdateInventoryIcons();
    }

    public void RemoveFromInventory(InventoryPickup item)
    {
        inventory.Remove(item);
        UpdateInventoryIcons();
    }

    public bool HasInInventory(InventoryPickup item)
    {
        return inventory.Contains(item);
    }

    public void UpdateInventoryIcons()
    {
        canvasManager.ClearInventoryIcons();

        for (int i = 0; i < inventory.Count; ++i)
            canvasManager.SetInventoryIcon(i, inventory[i].inventoryIcon);
    }
    





}
