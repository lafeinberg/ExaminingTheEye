using UnityEngine;
using UnityEngine.UI;

public class RayPointer : MonoBehaviour
{
    public float rayLength = 7f; // Increase from 10f to 20f (or any value you need)
    public LayerMask interactableLayer; // Layer for interactable objects
    private LineRenderer lineRenderer;
    private GameObject lastHighlightedObject;

    void Start()
    {
        // Initialize the Line Renderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = 2; // Start and end points
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    void Update()
    {
        // Cast a ray from the controller's position forward
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Check if the ray hits an object within the interactable layer
        if (Physics.Raycast(ray, out hit, rayLength, interactableLayer))
        {
            // Update Line Renderer to end at the hit point
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point); // End of the ray

        // Highlight the object
        HighlightObject(hit.collider.gameObject, true);

        // Handle interaction (e.g., trigger press)
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) // Trigger press
        {
            Debug.Log("Selected: " + hit.collider.name);
            ExecuteInteraction(hit.collider.gameObject);
        }
    }
    else
    {
        // Extend the ray forward if no object is hit
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + transform.forward * rayLength);

        // Reset highlight on the last object
        ResetHighlight();
    }
}


    // Highlights an object by changing its material color
    void HighlightObject(GameObject target, bool highlight)
    {
        if (highlight)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow; // Highlight color
                lastHighlightedObject = target;
            }
        }
    }

    // Resets the highlight on the last highlighted object
    void ResetHighlight()
    {
        if (lastHighlightedObject != null)
        {
            Renderer renderer = lastHighlightedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.white; // Default color
            }
            lastHighlightedObject = null;
        }
    }

    // Executes interaction by invoking the Button's onClick event
    void ExecuteInteraction(GameObject target)
    {
        var button = target.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
        }
        else
        {
            Debug.LogWarning("No Button component found on " + target.name);
        }
    }
}
