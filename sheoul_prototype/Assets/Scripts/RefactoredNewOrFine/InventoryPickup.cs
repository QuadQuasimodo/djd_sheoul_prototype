using UnityEngine;

    // for the effects of these items, Activated means it's in
    // the player's Inventory

    public class InventoryPickup : Interacteable
    {

    public string               inventoryName;
    public Sprite               inventoryIcon;

    // Gets picked up and put in ivnentory
    public override void Activate()
    {

        IsActive = true;
        Destroy(gameObject);

    }

}
