using UnityEngine;


// Attach to ForceField objects alongside DraggableObj (canRotate = true).
// The push direction rotates with the GameObject's transform.
public class ForceField : MonoBehaviour
{
    [Header("Field Settings")]
    public float radius         = 2.5f;
    public float forceMagnitude = 8f;


    [Header("Direction Mode")]
    [Tooltip("If true, pulls water toward center. If false, pushes in local direction.")]
    public bool attractToCenter = false;


    [Tooltip("Local-space push direction (used when attractToCenter = false).")]
    public Vector2 localPushDir = Vector2.right;


    void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("WaterDroplet")) continue;
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            if (rb == null) continue;


            Vector2 dir;
            if (attractToCenter)
            {
                dir = ((Vector2)transform.position
                     - (Vector2)hit.transform.position).normalized;
            }
            else
            {
                // Rotate localPushDir by this object's rotation
                dir = transform.TransformDirection(localPushDir).normalized;
            }


            rb.AddForce(dir * forceMagnitude, ForceMode2D.Force);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.1f, 0.5f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, radius);
        Gizmos.color = new Color(0.1f, 0.5f, 1f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

