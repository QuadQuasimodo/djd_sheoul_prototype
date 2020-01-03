using UnityEngine;
using System.Collections;

    /// <summary>
    ///  DO NOT USE THIS COMPONENT DIRECTLY.!-- 
    /// USE THE SCRIPTS THAT INHERIT FROM THIS     
    ///  For torches, doors and things the player activates directly
    /// </summary>
    /// 
    
    public  abstract class Interacteable : MonoBehaviour  
    {
        
    public string   interactText;
    public string   interactedText;
    public string   requirementText;

        // if the current interacteable is in its active state
        [HideInInspector] public bool IsActive {get;  set;} = false;
        //public bool Active {get; set;}

        // Starts the scene already activated
        public bool StartsActive {get; set;}

        [HideInInspector]
        public InteractionGroup myInterGroup {get; set;} = null;

        [HideInInspector]
        public int groupIndex {get; set;}

        // Is this item activatable by the player right now
        public bool unlocked {get; set;} = true;

        // Does this object need all others from its group to be activated for
        // itself to be activatable
        public bool RequiresOthersFromGroup {get; set;} = false;

        // Allow other items in the interactiongroup to trigger
        // this item's active state
        public bool ActivateableByOtherFromGroup {get; set;} = true;

        /// <summary>
        /// Called when player interacts with this
        /// </summary>
        /// <param name="simult"> if all in group are activated at the same
        /// time or if there is a slow chain effect </param>
        public virtual void OnInteract(bool simult = false)
        {
            
            int activeCount = 0;
            foreach(Interacteable f in myInterGroup?.interactionGroup)
            {

                if (f.IsActive) activeCount++;

            }

            if(!(activeCount == myInterGroup?.interactionGroup.Count - 1) && RequiresOthersFromGroup) return;
            else if ((activeCount == myInterGroup?.interactionGroup.Count - 1) && RequiresOthersFromGroup) this.Activate();
            else if (!RequiresOthersFromGroup) this.Activate();

            if (!simult)
            {
                for(int i = groupIndex; i < myInterGroup?.interactionGroup.Count; i++)
                {
                    for (int x = groupIndex; x > 0; x--)
                    {
                        
                        WaitTime(myInterGroup.activationDelay);

                        if(myInterGroup.interactionGroup[x].ActivateableByOtherFromGroup)
                            myInterGroup?.interactionGroup[x].Activate();
                        
                    }
                    if(myInterGroup.interactionGroup[i].ActivateableByOtherFromGroup)
                    myInterGroup?.interactionGroup[i].Activate();
                }

            }
            else if(simult) 
            {
                foreach(Interacteable i in myInterGroup?.interactionGroup)
                {

                    if(i.ActivateableByOtherFromGroup)
                        i.Activate();


                }
            }


        }

        protected abstract void Activate();


        IEnumerator WaitTime(float time)
        {
            yield return new WaitForSeconds(time);
        }

    }
