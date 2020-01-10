using UnityEngine;
using System.Collections;

/// <summary>
/// DO NOT USE THIS COMPONENT DIRECTLY.!-- 
/// USE THE SCRIPTS THAT INHERIT FROM THIS,     
///  Like torches, doors and things the player activates directly
/// </summary>
public abstract class Interactable : MonoBehaviour
{

    public string interactText;
    public string interactedText;
    public string requirementText;

    // if the current interacteable is in its active state
    [HideInInspector] public bool IsActive { get; set; } = false;
    //public bool Active {get; set;}

    [Tooltip("Starts the scene already activated.")]
    // Starts the scene already activated
    [SerializeField] public bool StartsActive;

    [HideInInspector]
    public InteractionGroup myInterGroup { get; set; } = null;

    [HideInInspector]
    public int groupIndex { get; set; }

    [Tooltip("Is the player able to activate this right now.")]
    // Is this item activatable by the player right now
    [SerializeField] public bool locked = false;

    [Tooltip("Does this need all others from it's interaction group " +
    "to be activated for itself to be activateable")]
    // Does this object need all others from its group to be activated for
    // itself to be activatable
    [SerializeField] private bool RequiresOthersFromGroup = false;

    [Tooltip("Other items in the group can activate this one")]
    // Allow other items in the interactiongroup to trigger
    // this item's active state
    [SerializeField] private bool ActivateableByOtherFromGroup = true;

    

    /// <summary>
    /// Called when player interacts with this
    /// </summary>
    /// <param name="simult"> if all in group are activated at the same
    /// time or if there is a slow chain effect </param>
    public virtual void OnInteract()
    {

        if (myInterGroup == null)
        {
            Activate();
            return;

        } 

        int activeCount = 0;
        foreach (Interactable f in myInterGroup?.interactionGroup)
        {
            if (f.IsActive) activeCount++;
        }

        if (!(activeCount == myInterGroup?.interactionGroup.Count - 1) && RequiresOthersFromGroup) return;
        else if ((activeCount == myInterGroup?.interactionGroup.Count - 1) && RequiresOthersFromGroup) Activate();
        else if (!RequiresOthersFromGroup) Activate();


        if (myInterGroup.ActivationChainType == InteractionGroup.ActivationChainTypes.Simultaneous)
            SimultaneousActivation();
        else if (myInterGroup.ActivationChainType == InteractionGroup.ActivationChainTypes.Ping_Pong)
            StartCoroutine(PingPongActivation());
        else if (myInterGroup.ActivationChainType == InteractionGroup.ActivationChainTypes.Simetrical)
            StartCoroutine(SimmetricalActivation());

    }

    protected abstract void Activate();



    IEnumerator PingPongActivation() 
    {

        for (int i = groupIndex; i < myInterGroup?.interactionGroup.Count; i++)
        {

            for (int x = groupIndex; x >= 0; x--)
            {

                if (myInterGroup.interactionGroup[x].ActivateableByOtherFromGroup)
                {
                    yield return new WaitForSeconds(myInterGroup.activationDelay);
                    myInterGroup?.interactionGroup[x].Activate();

                }

            }
            if (myInterGroup.interactionGroup[i].ActivateableByOtherFromGroup)
            {
                yield return new WaitForSeconds(myInterGroup.activationDelay);
                myInterGroup?.interactionGroup[i].Activate();

            }

        }

    }

    void SimultaneousActivation()
    {

        foreach (Interactable i in myInterGroup?.interactionGroup)
        {

            if (i.ActivateableByOtherFromGroup)
                i.Activate();

        }

    }

    // not working shit
    IEnumerator SimmetricalActivation()
    {
   
        for (int i = groupIndex; i < myInterGroup?.interactionGroup.Count; i++)
        {
        LoopBegin:
            yield return new WaitForSeconds(myInterGroup.activationDelay);

            if (myInterGroup.interactionGroup[i].ActivateableByOtherFromGroup)
            {

                myInterGroup?.interactionGroup[i].Activate();

            }

            for (int x = groupIndex; x >= 0; x--)
            {
               
                if (myInterGroup.interactionGroup[x].ActivateableByOtherFromGroup)
                {
                    
                    myInterGroup?.interactionGroup[x].Activate();

                    
                }
                goto LoopBegin;

            }


            

        }


    }

   



}

