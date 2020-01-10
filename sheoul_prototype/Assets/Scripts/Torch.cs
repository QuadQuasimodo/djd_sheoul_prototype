using UnityEngine;

    public class Torch : Interactable
    {

    [SerializeField] private GameObject Fire;

        private void Awake() { if(StartsActive) Activate(); }

        protected override void Activate()
        {
            if(IsActive) return;
            Fire.SetActive(true);
            IsActive = true;
        }
    }
