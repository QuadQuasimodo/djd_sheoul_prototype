using UnityEngine;
using System.Collections.Generic;

public class InteractionGroup : MonoBehaviour
{
    public float activationDelay {get; set;} = 0.5f;
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
