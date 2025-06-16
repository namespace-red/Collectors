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
                throw new ArgumentNullException(nameof(_animator));
            
            _animator = value;
        }
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void PlayIdle()
        => _animator.SetTrigger(Params.Idle);
    
    public void PlayRun()
        => _animator.SetTrigger(Params.Run);
    
    public void PlayPickUp1()
        => _animator.SetTrigger(Params.PickUp1);
    
    public void PlayPickUp2()
        => _animator.SetTrigger(Params.PickUp2);
    
    public void PlayPut1()
        => _animator.SetTrigger(Params.Put1);
    
    public void PlayPut2()
        => _animator.SetTrigger(Params.Put2);
    
    private static class Params
    {
        public static readonly int Idle = Animator.StringToHash(nameof(Idle));
        public static readonly int Run = Animator.StringToHash(nameof(Run));
        public static readonly int PickUp1 = Animator.StringToHash(nameof(PickUp1));
        public static readonly int PickUp2 = Animator.StringToHash(nameof(PickUp2));
        public static readonly int Put1 = Animator.StringToHash(nameof(Put1));
        public static readonly int Put2 = Animator.StringToHash(nameof(Put2));
    }
}
