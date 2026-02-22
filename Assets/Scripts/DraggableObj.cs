using UnityEngine;

public class DraggableObj : MonoBehaviour
{
    public bool canRotate;
    public float rotationSpeed = 90f;
    
    private static GameObject currentlySelectedObject = null;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                DraggableObj clickedObj = hit.collider.GetComponent<DraggableObj>();
                if (clickedObj != null)
                {
                    currentlySelectedObject = clickedObj.gameObject;
                    Debug.Log("Selected: " + currentlySelectedObject.name);
                }
            }
        }
        
        if (canRotate && Input.GetKey(KeyCode.R) && gameObject == currentlySelectedObject)
        {
            float tiltAngle = Mathf.Sin(Time.time * rotationSpeed) * 30f;
            transform.rotation = Quaternion.Euler(0, 0, tiltAngle);
        }
    }
    
    void OnDrawGizmos()
    {
        if (gameObject == currentlySelectedObject)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}