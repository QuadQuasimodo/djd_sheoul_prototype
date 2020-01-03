using UnityEngine;
using System.Collections;

    /// <summary>
    ///  For torches, doors and things the player activates directly
    /// </summary>
    /// 
    
    public  abstract class Interacteable  
    {
        
        

        [HideInInspector] public bool IsActive {get;  set;} = false;
        //public bool Active {get; set;}
        public bool StartsActive {get; set;}

        public InteractionGroup myInterGroup {get; set;}
        protected int groupIndex {get; set;}

        public bool unlocked {get; set;} = true;

        /// <summary>
        /// Called when player interacts with this
        /// </summary>
        /// <param name="simult"> if all in group are activated at the same
        /// time or if there is a slow chain effect </param>
        public virtual void OnInteract(bool simult = false)
        {
            this.Activate();

            if (!simult && !unlocked)
            {
                for(int i = groupIndex; i < myInterGroup.interactionGroup.Count; i++)
                {
                    for (int x = groupIndex; x > 0; x--)
                    {
                        myInterGroup.interactionGroup[i].Activate();
                        myInterGroup.interactionGroup[x].Activate();
                        WaitTime(myInterGroup.activationDelay);
                    }


                }

            }
            else if(simult && !unlocked) 
            {
                foreach(Interacteable i in myInterGroup.interactionGroup)
                {

                    i.Activate();


                }
            }


        }

        protected abstract void Activate();

        // On player interacting with an instance of this
        // the object will activate all interacteable
        // in their interaction group

        //Interaction group will always contain the object itself
        // 

        IEnumerator WaitTime(float time)
        {
            yield return new WaitForSeconds(time);
        }

    }
