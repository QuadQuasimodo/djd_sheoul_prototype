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
    [SerializeField] public bool startsActive;

    [HideInInspector]
    public InteractionGroup MyInterGroup { get; set; } = null;

    [HideInInspector]
    public int GroupIndex { get; set; }

    [Tooltip("Is the player able to activate this right now.")]
    // Is this item activatable by the player right now
    [SerializeField] public bool locked = false;

    [Tooltip("Does this need all others from it's interaction group " +
    "to be activated for itself to be activateable")]
    // Does this object need all others from its group to be activated for
    // itself to be activatable
    [SerializeField] public bool requiresOthersFromGroup = false;

    [Tooltip("Activates automatically once every object in group is active")]
    ///////// INSERT MESSAGE HERE /////////
    [SerializeField] public bool activatesAutomatically = false;

    [Tooltip("Other items in the group can activate this one")]
    // Allow other items in the interactiongroup to trigger
    // this item's active state
    [SerializeField] private bool activateableByOtherFromGroup = true;



    public abstract void Activate();

    /// <summary>
    /// Called when player interacts with this
    /// </summary>
    /// <param name="simult"> if all in group are activated at the same
    /// time or if there is a slow chain effect </param>
    public virtual void OnInteract()
    {
        if (MyInterGroup == null)
        {
            Activate();
            return;
        }

        if (locked) return;

        else if (!locked) Activate();

        if (MyInterGroup.activationChainType ==
            InteractionGroup.ActivationChainTypes.Simultaneous)
            SimultaneousActivation();

        else if (MyInterGroup.activationChainType ==
            InteractionGroup.ActivationChainTypes.Ping_Pong)
            StartCoroutine(PingPongActivation());

        else if (MyInterGroup.activationChainType ==
            InteractionGroup.ActivationChainTypes.Simetrical)
            StartCoroutine(SimmetricalActivation());
    }

    void SimultaneousActivation()
    {
        foreach (Interactable i in MyInterGroup.interactionGroup)
        {
            if (i.activateableByOtherFromGroup)
                i.Activate();
        }
    }

    IEnumerator PingPongActivation()
    {
        int i, j;
        for (i = j = GroupIndex; (i < MyInterGroup.interactionGroup.Count) || (j >= 0); i++, j--)
        {
            if (i < MyInterGroup.interactionGroup.Count)
            {
                if (MyInterGroup.interactionGroup[i].activateableByOtherFromGroup)
                {
                    yield return new WaitForSeconds(MyInterGroup.activationDelay);
                    MyInterGroup.interactionGroup[i].Activate();
                }
            }
            if (j >= 0)
            {
                if (MyInterGroup.interactionGroup[j].activateableByOtherFromGroup)
                {
                    yield return new WaitForSeconds(MyInterGroup.activationDelay);
                    MyInterGroup.interactionGroup[j].Activate();
                }
            }
        }
    }

    IEnumerator SimmetricalActivation()
    {
        int i, j;
        for (i = j = GroupIndex; (i < MyInterGroup.interactionGroup.Count) || (j >= 0); i++, j--)
        {
            yield return new WaitForSeconds(MyInterGroup.activationDelay);

            if (i < MyInterGroup.interactionGroup.Count)
            {
                if (MyInterGroup.interactionGroup[i].activateableByOtherFromGroup)
                {
                    MyInterGroup.interactionGroup[i].Activate();
                }
            }
            if (j >= 0)
            {
                if (MyInterGroup.interactionGroup[j].activateableByOtherFromGroup)
                {
                    MyInterGroup.interactionGroup[j].Activate();
                }
            }
        }
    }
}

