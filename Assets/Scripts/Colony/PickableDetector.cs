using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableDetector : BaseDetector<IPickable>
{
    [SerializeField, Min(0.1f)] private float _secCoolDown;
    
    public event Action<ICollection<IPickable>> DetectedPickables;
    
    private void OnEnable()
    {
        StartCoroutine(RunDetecting());
    }

    public void Run() => enabled = true;
    public void Stop() => enabled = false;

    private IEnumerator RunDetecting()
    {
        var coolDown = new WaitForSeconds(_secCoolDown);
        
        while (enabled)
        {
            var detectedPickables = DetectAll();
            
            if (detectedPickables.Count > 0)
                DetectedPickables?.Invoke(detectedPickables);

            yield return coolDown;
        }
    }
}
