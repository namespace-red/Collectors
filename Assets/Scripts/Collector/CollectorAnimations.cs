using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CollectorAnimations : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator
    {
        get => _animator;
        
        private set
        {
            if (value == null)
                throw new NullReferenceException(nameof(_animator));
            
            _animator = value;
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
        => _animator.SetTrigger(State.Idle);
    
    public void PlayRun()
        => _animator.SetTrigger(State.Run);
    
    public void PlayPickUp1()
        => _animator.SetTrigger(State.PickUp1);
    
    public void PlayPickUp2()
        => _animator.SetTrigger(State.PickUp2);
    
    public void PlayPut1()
        => _animator.SetTrigger(State.Put1);
    
    public void PlayPut2()
        => _animator.SetTrigger(State.Put2);
    
    private static class State
    {
        public const string Idle = nameof(Idle);
        public const string Run = nameof(Run);
        public const string PickUp1 = nameof(PickUp1);
        public const string PickUp2 = nameof(PickUp2);
        public const string Put1 = nameof(Put1);
        public const string Put2 = nameof(Put2);
    }
}
