using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractiveType { PICKABLE, INTERACT_ONCE, INTERACT_MULTIPLE, INDIRECT, TORCH };

    public bool                 isActive;
    public bool                 hasInteracted;

    public InteractiveType      type;

    public string               inventoryName;
    public Sprite               inventoryIcon;

    public string               interactText;
    public string               interactedText;
    public string               requirementText;


    public string needs;

    public Interactable[] inventoryRequirements;
    public Interactable[] activationRequirements;

    public Interactable[] activationChain;
    public Interactable[] interactionChain;

    private Animator           animator;

    GameObject fire;

    public void Start()
    {
        animator = GetComponent<Animator>();
        hasInteracted = false;

        needs = default;
        if (inventoryRequirements.Length!=0 && activationRequirements.Length!=0) needs = "Both";
        else if (inventoryRequirements.Length!=0) needs = "Inventory";
        else if (activationRequirements.Length!=0) needs = "Activation";
        else needs = "None";

        if (type == InteractiveType.TORCH)
        {
            fire = GetComponentInChildren<ParticleSystem>()?.gameObject;
            fire.SetActive(false);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Interact()
    {
        if (animator != null)
        {
            animator.SetTrigger("Interacted");
        }

        if (isActive)
        {
            ProcessActivationChain();
            ProcessInteractionChain();

            if (type == InteractiveType.INTERACT_ONCE)
            {
                GetComponent<Collider>().enabled = false;
            }
        }
        hasInteracted = true;
    }

    // For torches
    public void LightOn()
    {
        //Light l = GetComponentInChildren<Light>();
       
        if(!hasInteracted)fire.SetActive(true);
        hasInteracted = true;
    }

    private void ProcessActivationChain()
    {
        if (activationChain != null)
        {
            for (int i = 0; i < activationChain.Length; ++i)
                activationChain[i].Activate();
        }
    }

    private void ProcessInteractionChain()
    {
        if (interactionChain != null)
        {
            for (int i = 0; i < interactionChain.Length; ++i)
                interactionChain[i].Interact();
        }
    }
}
