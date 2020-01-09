using UnityEngine;
using System.Collections.Generic;

public class InteractionGroup : MonoBehaviour
{
    [Tooltip("Delay between each item being activated in the group")]
    public float activationDelay;

    [Tooltip("All activate at the same time.")]
    public bool SimultaneousActivation = false;

    [Tooltip("Objects that will interact with each other," +
        "or otherwise are related when it comes to interactions or activations.")]
    public List<Interacteable> interactionGroup;

    private void Awake()
    {
        for(int i = 0; i<interactionGroup.Count; i++)
        {

            interactionGroup[i].myInterGroup = this;
            interactionGroup[i].groupIndex = i;

        }


    }
}
