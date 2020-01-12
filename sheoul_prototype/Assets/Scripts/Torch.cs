using UnityEngine;

    public class Torch : Interactable
    {

    [SerializeField] private GameObject Fire;

        private void Awake() { if(startsActive) Activate(); }

        public override void Activate()
        {
            if(IsActive) return;
            Fire.SetActive(true);
            IsActive = true;
        }
    }
