using UnityEngine;

    public class Door : Interacteable
    {
        private Animator _animator;

        private void Awake() 
        {
            if(StartsActive) Activate();
        }

        public override void  Activate()
        {
            if(IsActive) return;

            _animator?.SetTrigger("Interacted");

            IsActive = true;

        }
    }
