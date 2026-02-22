using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class WaterParticleHandler : MonoBehaviour
{
    [Header("Fill Target")]
    public FillContainer fillContainer;

    [Header("Fill Amount Per Particle")]
    public float fillPerParticle = 1f;

    [Header("Teleporters")]
    public TeleporterConfig[] teleporters;

    [System.Serializable]
    public class TeleporterConfig
    {
        public Transform      entryPoint;    //tp
        public float          entryRadius = 0.5f;
        public ParticleSystem exitEmitter;   
    }

    [Header("Container Trigger (3D Collider on container mouth)")]
    public Collider containerCollider;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    List<ParticleSystem.Particle> triggerList = new List<ParticleSystem.Particle>();

    void Start()
    {
        ps        = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[ps.main.maxParticles];
    }

    void Update()
    {
        if (teleporters == null || teleporters.Length == 0) return;

        int count = ps.GetParticles(particles);
        bool modified = false;

        for (int i = 0; i < count; i++)
        {

            Vector3 worldPos = ps.main.simulationSpace == ParticleSystemSimulationSpace.Local
                ? transform.TransformPoint(particles[i].position)
                : particles[i].position;

            foreach (var t in teleporters)
            {
                if (t.entryPoint == null) continue;
                float dist = Vector3.Distance(worldPos, t.entryPoint.position);

                if (dist <= t.entryRadius)
                {
                    // Kill this particle
                    particles[i].remainingLifetime = 0f;
                    modified = true;

                    // Spawn one at exit
                    if (t.exitEmitter != null)
                        t.exitEmitter.Emit(1);

                    break; 
                }
            }
        }

        if (modified)
            ps.SetParticles(particles, count);
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(
            ParticleSystemTriggerEventType.Enter, triggerList);

        if (numEnter > 0 && fillContainer != null)
            fillContainer.AddWater(numEnter * fillPerParticle);

        for (int i = 0; i < numEnter; i++)
        {
            var p = triggerList[i];
            p.remainingLifetime = 0f;
            triggerList[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, triggerList);
    }
}