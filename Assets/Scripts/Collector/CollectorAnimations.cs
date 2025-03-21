using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CollectorAnimations : MonoBehaviour
{
    private Animator _animator;

    public event Action PickUpComplete;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayPickUp()
        => _animator.SetTrigger(State.PickUp);


    public void OnPickUpComplete()
        => PickUpComplete?.Invoke();

    private static class State
    {
        public const string PickUp = nameof(PickUp);
    }
}
