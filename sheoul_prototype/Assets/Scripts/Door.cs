using UnityEngine;

    public class Door : Interactable
    {
        private Animator _animator;

        private void Awake() 
        {
            if(StartsActive) Activate();
        }

        protected override void  Activate()
        {
            if(IsActive) return;

            _animator?.SetTrigger("Interacted");

            IsActive = true;

        }
    }