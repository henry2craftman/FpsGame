using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTrigger : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    List<ParticleSystem.Particle> touchedParticles = new List<ParticleSystem.Particle>();

    private void OnParticleTrigger()
    {
        ParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, touchedParticles);

        foreach(var p in touchedParticles)
        {
            print("파티클 감지: " + p.position);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        print(other.name);
    }
}
