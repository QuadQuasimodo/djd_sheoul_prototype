using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    private PlayerInventory playerInventory;

    private const float  MAX_INTERACTION_DISTANCE = 3.0f;

    public Intro intro;

    public CanvasManager       canvasManager;
    private Transform          cameraTransform;
    private Interacteable       currentInteractive;

    // private List<Interactable> inventory;
   // private bool                     hasInventoryRequirements;
    // private bool                     hasActivationRequirements;



    public void Start()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        playerInventory = new PlayerInventory(canvasManager);
    }

    public void Update()
    {
        CheckForInteractive();
        CheckForInteraction();

        /*if(intro.introFinished)
        {
            CheckForInteractive();
            CheckForInteraction();
        }*/
    }

    private void CheckForInteractive()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
                            out RaycastHit hitInfo,
                            MAX_INTERACTION_DISTANCE))
        {
            Interacteable newInteractive =
                (hitInfo.collider.GetComponent<Interacteable>() == null) ?
                hitInfo.collider.GetComponentInParent<Interacteable>() :
                hitInfo.collider.GetComponent<Interacteable>();

            if (newInteractive != null && newInteractive != currentInteractive)
                SetCurrentInteractive(newInteractive);
            else if (newInteractive == null)
                ClearCurrentInteractive();
        }
        else
            ClearCurrentInteractive();
    }

    private void SetCurrentInteractive(Interacteable newInteractive)
    {
        currentInteractive = newInteractive;

        if(!currentInteractive.unlocked)
            canvasManager.ShowInteractionPanel(currentInteractive.interactText);
        else if(currentInteractive.unlocked)
            canvasManager.ShowInteractionPanel(currentInteractive.interactText);

    }

    /*private bool HasInventoryRequirements()
    {
        if (currentInteractive.inventoryRequirements == null)
            return true;

        for (int i = 0; i < currentInteractive.inventoryRequirements.Length; ++i)
            if (!HasInInventory(currentInteractive.inventoryRequirements[i]))
                return false;

        return true;
    }*/
    
    /*private bool HasActivationRequirements()
    {
        int activated = 0;

        for (int i = 0; i < currentInteractive.activationRequirements.Length; ++i)
            if (currentInteractive.activationRequirements[i].hasInteracted) activated++;

        if (activated == currentInteractive.activationRequirements.Length) return true;
        else return false;
    }*/

    private void ClearCurrentInteractive()
    {
        if (currentInteractive != null)
        {
                currentInteractive = null;
                canvasManager.HideInteractionPanel();
            
        }
    }

    private void CheckForInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractive != null)
        {
            if(currentInteractive.unlocked)
                currentInteractive.OnInteract();
            if((currentInteractive as InventoryPickup) != null)
            {
                playerInventory.AddToInventory(currentInteractive as InventoryPickup);
                ShowPickeUpMessage();
                currentInteractive.gameObject.SetActive(false);

            } 
            canvasManager.HideInteractionPanel();
            StartCoroutine(WaitTime(1));
        }
    }

    private void ShowPickeUpMessage()
    {
        canvasManager.HideInteractionPanel();
        canvasManager.ShowInteractionPanel(currentInteractive.interactedText);
        //currentInteractive.hasInteracted = true;
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);

        currentInteractive = null;
        canvasManager.HideInteractionPanel();
    }

   
}
