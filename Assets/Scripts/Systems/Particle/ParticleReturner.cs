using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReturner : MonoBehaviour
{
    private string effectName;

    private void Start()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Callback; // Set the stop action to trigger a callback
    }

    public void Initialize(string name)
    {
        effectName = name; // Set the effect name for this particle system
    }

    private void OnParticleSystemStopped()
    {
        // When the particle system stops, return it to the pool
        ParticleSystemController.Instance.ReturnToPool(effectName, gameObject);
    }
}
