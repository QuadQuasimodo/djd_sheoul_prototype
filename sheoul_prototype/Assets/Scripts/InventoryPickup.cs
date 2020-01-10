using UnityEngine;

    // for the effects of these items, Activated means it's in
    // the player's Inventory

    public class InventoryPickup : Interactable
    {

    public string               inventoryName;
    public Sprite               inventoryIcon;

    // Gets picked up and put in ivnentory
    protected override void Activate() { IsActive = true; }
}