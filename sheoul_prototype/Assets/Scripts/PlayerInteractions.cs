using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    private const float  MAX_INTERACTION_DISTANCE = 3.0f;


    public CanvasManager       canvasManager;
    private Transform          cameraTransform;
    private Interactable       currentInteractive;

    private List<Interactable> inventory;


    private bool                     hasInventoryRequirements;
    private bool                     hasActivationRequirements;



    public void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        inventory = new List<Interactable>();
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
            Interactable newInteractive =
                (hitInfo.collider.GetComponent<Interactable>() == null) ?
                hitInfo.collider.GetComponentInParent<Interactable>() :
                hitInfo.collider.GetComponent<Interactable>();

            if (newInteractive != null && newInteractive != currentInteractive)
                SetCurrentInteractive(newInteractive);
            else if (newInteractive == null)
                ClearCurrentInteractive();
        }
        else
            ClearCurrentInteractive();
    }

    private void SetCurrentInteractive(Interactable newInteractive)
    {
        currentInteractive = newInteractive;

        switch (currentInteractive.needs)
        {
            case "Both":
                if (HasInventoryRequirements() && HasActivationRequirements())
                {
                    hasInventoryRequirements = true;
                    hasActivationRequirements = true;

                    if (!HasInteracted(currentInteractive))
                        canvasManager.ShowInteractionPanel(
                        currentInteractive.interactText);
                }
                else canvasManager.ShowInteractionPanel(currentInteractive.requirementText);
                break;
            case "Inventory":
                if (HasInventoryRequirements())
                {
                    hasInventoryRequirements = true;

                    if (!HasInteracted(currentInteractive))
                        canvasManager.ShowInteractionPanel(
                        currentInteractive.interactText);
                }
                else canvasManager.ShowInteractionPanel(currentInteractive.requirementText);
                break;
            case "Activation":
                if (HasActivationRequirements())
                {
                    hasActivationRequirements = true;

                    if (!HasInteracted(currentInteractive))
                        canvasManager.ShowInteractionPanel(
                        currentInteractive.interactText);
                }
                else canvasManager.ShowInteractionPanel(currentInteractive.requirementText);
                break;
            case "None":
                canvasManager.ShowInteractionPanel(currentInteractive.interactText);
                break;
        }
    }

    private bool HasInventoryRequirements()
    {
        if (currentInteractive.inventoryRequirements == null)
            return true;

        for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
            if (!HasInInventory(currentInteractive.inventoryRequirements[i]))
                return false;

        return true;
    }
    
    private bool HasActivationRequirements()
    {
        int activeRequiremnets = 0;

        for (int i = 0; i < currentInteractive.activationRequirements.Length; ++i)
            if (currentInteractive.activationRequirements[i].hasInteracted) activeRequiremnets++;

        if (activeRequiremnets == currentInteractive.activationRequirements.Length) return true;
        else return false;
    }

    private void ClearCurrentInteractive()
    {
        if (currentInteractive != null)
        {
            if (currentInteractive.hasInteracted)
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
            if (currentInteractive.type == Interactable.InteractiveType.PICKABLE)
                Pick();

            else if (currentInteractive.type == Interactable.InteractiveType.TORCH)
                currentInteractive.LightOn();
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
        switch (currentInteractive.needs)
        {
            case "Both":
                if (hasInventoryRequirements && hasActivationRequirements)
                {
                    for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
                    {
                        RemoveFromInventory(currentInteractive.inventoryRequirements[i]);

                        canvasManager.ShowInteractionPanel(currentInteractive.interactedText);
                    }
                    currentInteractive.Interact();
                }
                break;
            case "Inventory":
                if (hasInventoryRequirements)
                {
                    for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
                    {
                        RemoveFromInventory(currentInteractive.inventoryRequirements[i]);

                        canvasManager.ShowInteractionPanel(currentInteractive.interactedText);
                    }
                    currentInteractive.Interact();
                }
                break;
            case "Activation":
                if (hasActivationRequirements) currentInteractive.Interact();
                break;
            case "None":
                currentInteractive.Interact();
                break;
        }
    }

    private void AddToInventory(Interactable item)
    {
        inventory.Add(item);
        UpdateInventoryIcons();
    }

    private void RemoveFromInventory(Interactable item)
    {
        inventory.Remove(item);
        UpdateInventoryIcons();
    }

    private bool HasInteracted(Interactable item)
    {
        return item.hasInteracted;
    }
    
    private bool HasInInventory(Interactable item)
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
        canvasManager.ShowInteractionPanel(currentInteractive.interactedText);
        currentInteractive.hasInteracted = true;
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        currentInteractive = null;
        canvasManager.HideInteractionPanel();
    }
}
