using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    private const float  MAX_INTERACTION_DISTANCE = 3.0f;


    public CanvasManager canvasManager;
    private Transform                cameraTransform;
    private InteractableObject       currentInteractive;

    private List<InteractableObject> inventory;

    private bool                     hasRequirements;



    public void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        inventory = new List<InteractableObject>();
    }

    public void Update()
    {
        CheckForInteractive();
        CheckForInteraction();
    }

    private void CheckForInteractive()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                            out RaycastHit hitInfo,
                            MAX_INTERACTION_DISTANCE))
        {
            InteractableObject newInteractive =
                hitInfo.collider.GetComponent<InteractableObject>();

            if (newInteractive != null && newInteractive != currentInteractive)
                SetCurrentInteractive(newInteractive);
            else if (newInteractive == null)
                ClearCurrentInteractive();
        }
        else
            ClearCurrentInteractive();
    }

    private void SetCurrentInteractive(InteractableObject newInteractive)
    {
        currentInteractive = newInteractive;

        if (currentInteractive.type ==
            InteractableObject.InteractiveType.PICKABLE)
                canvasManager.ShowInteractionPanel(currentInteractive.pickupText);

        else if (HasInteractionRequirements())
        {
            hasRequirements = true;
            canvasManager.ShowInteractionPanel(
                currentInteractive.interactText);
        }
        else
        {
            hasRequirements = false;
            canvasManager.ShowInteractionPanel(currentInteractive.requirementText);
        }
    }

    private bool HasInteractionRequirements()
    {
        if (currentInteractive.inventoryRequirements == null)
            return true;

        for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
            if (!HasInInventory(currentInteractive.inventoryRequirements[i]))
                return false;

        return true;
    }

    private void ClearCurrentInteractive()
    {
        if (currentInteractive != null)
        {
            if (currentInteractive.interactedWith)
            {
                StartCoroutine(WaitTime(1));
            }
            else
            {
                currentInteractive = null;
                canvasManager.HideInteractionPanel();
            }
        }
    }

    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractive != null)
        {
            if (currentInteractive.type == InteractableObject.InteractiveType.PICKABLE)
                Pick();
         
            else Interact();
        }
    }

    private void Pick()
    {
        AddToInventory(currentInteractive);
        ShowPickeUpMessage();
        currentInteractive.gameObject.SetActive(false);
    }

    private void Interact()
    {
        if (hasRequirements)
        {
            for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
            {
                RemoveFromInventory(currentInteractive.inventoryRequirements[i]);

                if (currentInteractive.type != InteractableObject.InteractiveType.PICKABLE)
                    canvasManager.ShowInteractionPanel(currentInteractive.interactedText);
            }
            currentInteractive.Interact();
        }
    }

    private void AddToInventory(InteractableObject item)
    {
        inventory.Add(item);
        UpdateInventoryIcons();
    }

    private void RemoveFromInventory(InteractableObject item)
    {
        inventory.Remove(item);
        UpdateInventoryIcons();
    }

    private bool HasInInventory(InteractableObject item)
    {
        return inventory.Contains(item);
    }

    private void UpdateInventoryIcons()
    {
        canvasManager.ClearInventoryIcons();

        for (int i = 0; i < inventory.Count; ++i)
            canvasManager.SetInventoryIcon(i, inventory[i].inventoryIcon);
    }

    private void ShowPickeUpMessage()
    {
        canvasManager.HideInteractionPanel();
        canvasManager.ShowInteractionPanel(currentInteractive.pickedupText);
        currentInteractive.interactedWith = true;
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        currentInteractive = null;
        canvasManager.HideInteractionPanel();
    }
}
