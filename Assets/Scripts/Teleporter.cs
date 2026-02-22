using UnityEngine;



public class Teleporter : MonoBehaviour
{
    [Header("Warp Target")]
    public Transform teleportTarget;    // the exit point
    public bool      preserveVelocity = true;


    [Header("Cooldown (prevents double-warp)")]
    public float cooldown = 0.15f;
    float cooldownTimer;


    void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("WaterDroplet")) return;
        if (cooldownTimer > 0f) return;


        cooldownTimer = cooldown;


        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Vector2 vel    = (rb != null && preserveVelocity)
                         ? rb.linearVelocity : Vector2.zero;


        other.transform.position = teleportTarget.position;


        if (rb != null) rb.linearVelocity = vel;
    }
}

