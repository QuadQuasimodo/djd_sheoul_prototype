using UnityEngine;

public class Door : Interactable
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (startsActive) Activate();
    }

    public override void Activate()
    {
        if (IsActive) return;

        _animator.SetTrigger("Interacted");

        IsActive = true;
    }
}
