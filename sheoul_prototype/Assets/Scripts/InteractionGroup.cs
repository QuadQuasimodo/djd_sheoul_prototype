using UnityEngine;
using System.Collections.Generic;

public class InteractionGroup : MonoBehaviour
{
   
    public enum ActivationChainTypes {Simultaneous, Ping_Pong, Simetrical }

    [Tooltip("Objects that will interact with each other," +
    "or otherwise are related when it comes to interactions or activations.")]
    public List<Interactable> interactionGroup = new List<Interactable>();

    [Tooltip("Delay between each item being activated in the group")]
    public float activationDelay;

   /* [Tooltip("All objects in group activate at the same time.")]
    public bool SimultaneousActivation = false;

    public bool PingPongActivation = true;

    public bool ChainActivation = false;*/

    [Tooltip("The chain reaction has can only be started by a certain object")]
    public bool specificReactionStarter = false;

    
    [Tooltip("Index of Object that is the starter")]
    public int indexOfStarterObject;

    [Tooltip("The way the group will activate in the chain.")]
    public ActivationChainTypes activationChainType;


    public int ActiveCount { get; set; } = 0;
    private void Awake()
    {
        indexOfStarterObject = Mathf.Clamp(indexOfStarterObject, 0, interactionGroup.Count);
        for(int i = 0; i<interactionGroup.Count; i++)
        {
            interactionGroup[i].MyInterGroup = this;
            interactionGroup[i].GroupIndex = i;

            if ((specificReactionStarter && i != indexOfStarterObject) ||
                interactionGroup[i].requiresOthersFromGroup) 
            {
                interactionGroup[i].locked = true;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < interactionGroup.Count; i++)
        {
            if (interactionGroup[i].locked)
            {
                if (ActiveCount == interactionGroup.Count - 1)
                {
                    interactionGroup[i].locked = false;
                    if (interactionGroup[i].activatesAutomatically)
                        interactionGroup[i].Activate();
                }
            }
        }
    }
}
