using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WaterParticleHandler : MonoBehaviour
{
    [Header("Fill Target")]
    public FillContainer fillContainer;
    public Collider      containerCollider; 

    [Header("Fill Amount Per Particle")]
    public float fillPerParticle = 1f;

    [Header("Teleporters (optional — add one entry per teleporter pair)")]
    public TeleporterConfig[] teleporters;

    [System.Serializable]
    public class TeleporterConfig
    {
        public Collider       entryCollider; 
        public ParticleSystem exitEmitter;   
    }

    ParticleSystem                ps;
    List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        
        ConfigureTriggerModule();
    }
    
    void ConfigureTriggerModule()
    {
        var triggerModule = ps.trigger;
        triggerModule.enabled = true;
        triggerModule.colliderQueryMode = ParticleSystemColliderQueryMode.All;
        
        // clear da existing colliders (RemoveAllColliders removed in Unity 6)
        for (int i = 0; i < triggerModule.colliderCount; i++)
            triggerModule.SetCollider(i, null);
        
        int colliderIndex = 0;
        
        //  container collider
        if (containerCollider != null)
        {
            triggerModule.SetCollider(colliderIndex, containerCollider);
            colliderIndex++;
        }
        
        // teleporter colliders
        if (teleporters != null)
        {
            foreach (var t in teleporters)
            {
                if (t.entryCollider != null)
                {
                    triggerModule.SetCollider(colliderIndex, t.entryCollider);
                    colliderIndex++;
                }
            }
        }
    }

    void OnParticleTrigger()
    {
        ParticleSystem.ColliderData colliderData;
        int count = ps.GetTriggerParticles(
            ParticleSystemTriggerEventType.Enter,
            particleList,
            out colliderData);

        int fillCount = 0;

        for (int i = 0; i < count; i++)
        {
            bool particleProcessed = false;
            
            int colliderCount = colliderData.GetColliderCount(i);
            
            for (int j = 0; j < colliderCount; j++)
            {
                Collider col = colliderData.GetCollider(i, j) as Collider;
                if (col == null) continue;

                if (containerCollider != null && col == containerCollider && !particleProcessed)
                {
                    ParticleSystem.Particle p = particleList[i];
                    p.remainingLifetime = 0f;
                    particleList[i]     = p;
                    fillCount++;
                    particleProcessed = true;
                }

                foreach (var t in teleporters)
                {
                    if (t.entryCollider != null && col == t.entryCollider && !particleProcessed)
                    {
                        ParticleSystem.Particle p = particleList[i];
                        p.remainingLifetime = 0f;
                        particleList[i]     = p;

                        if (t.exitEmitter != null)
                            t.exitEmitter.Emit(1);

                        particleProcessed = true;
                        break;
                    }
                }
                
                if (particleProcessed)
                    break;
            }
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);

        if (fillCount > 0 && fillContainer != null)
            fillContainer.AddWater(fillCount * fillPerParticle);
    }
}