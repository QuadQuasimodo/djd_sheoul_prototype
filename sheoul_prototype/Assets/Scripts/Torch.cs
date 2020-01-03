using UnityEngine;

    public class Torch : Interacteable
    {
        
        GameObject Fire {get; set;}

        private void Awake() {
            if(StartsActive) Activate();
        }

        protected override void Activate()
        {
            if(IsActive) return;
            Fire.SetActive(true);
            IsActive = true;

        }

        

    }
