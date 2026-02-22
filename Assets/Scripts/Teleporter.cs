using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem waterParticleSystem;
    public Transform teleporterOut;

    [Header("Detection")]
    public float radius = 0.5f;

    ParticleSystem.Particle[] particles;

    void Start()
    {
        particles = new ParticleSystem.Particle[waterParticleSystem.main.maxParticles];
    }

    void Update()
    {
        if (waterParticleSystem == null || teleporterOut == null) return;

        int count = waterParticleSystem.GetParticles(particles);
        bool changed = false;

        for (int i = 0; i < count; i++)
        {
            Vector3 worldPos = waterParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local
                ? waterParticleSystem.transform.TransformPoint(particles[i].position)
                : particles[i].position;

            if (Vector3.Distance(worldPos, transform.position) <= radius)
            {
                particles[i].position = waterParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local
                    ? waterParticleSystem.transform.InverseTransformPoint(teleporterOut.position)
                    : teleporterOut.position;

                changed = true;
            }
        }

        if (changed)
            waterParticleSystem.SetParticles(particles, count);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.6f, 0.1f, 1f, 0.4f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(0.6f, 0.1f, 1f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}