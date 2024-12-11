using UnityEngine;

public class ThumbstickScaler : MonoBehaviour
{
    public Transform leftHand; // Reference to the left hand/controller
    public Transform rightHand; // Reference to the right hand/controller
    public LineRenderer leftRayRenderer; // Optional: LineRenderer for the left ray
    public LineRenderer rightRayRenderer; // Optional: LineRenderer for the right ray
    public LayerMask interactableLayer; // LayerMask to specify which objects can be scaled
    public float scalingSpeed = 0.2f; // Speed of scaling
    public float minScale = 0.1f; // Minimum scale
    public float maxScale = 3.0f; // Maximum scale

    private Transform leftQuad; // Reference to the LeftEyeQuad
    private Transform rightQuad; // Reference to the RightEyeQuad

    void Update()
    {
        RaycastForController(leftHand, leftRayRenderer, OVRInput.Axis2D.PrimaryThumbstick);
        RaycastForController(rightHand, rightRayRenderer, OVRInput.Axis2D.SecondaryThumbstick);
    }

    void RaycastForController(Transform hand, LineRenderer rayRenderer, OVRInput.Axis2D thumbstickAxis)
    {
        // Perform raycasting
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            // Draw the ray for debugging
            if (rayRenderer != null)
            {
                rayRenderer.SetPosition(0, hand.position);
                rayRenderer.SetPosition(1, hit.point);
            }

            // Check if the hit object is a quad and belongs to an ImageGroup
            Transform hitRoot = hit.transform.root; // Get the root of the hit object
            if (hitRoot.CompareTag("ImageGroup"))
            {
                // Find the quads within the ImageGroup
                leftQuad = hitRoot.Find("LeftEyeQuad");
                rightQuad = hitRoot.Find("RightEyeQuad");
            }

            // Scale the quads if they exist
            ScaleQuads(thumbstickAxis);
        }
        else
        {
            // Reset ray and clear target if no hit
            if (rayRenderer != null)
            {
                rayRenderer.SetPosition(0, hand.position);
                rayRenderer.SetPosition(1, hand.position + hand.forward * 10);
            }

            leftQuad = null;
            rightQuad = null;
        }
    }

    void ScaleQuads(OVRInput.Axis2D thumbstickAxis)
    {
        if (leftQuad == null || rightQuad == null) return;

        // Get the thumbstick Y-axis input from the controller
        float thumbstickInput = OVRInput.Get(thumbstickAxis).y;

        // If there's no input, skip scaling
        if (Mathf.Abs(thumbstickInput) < 0.01f) return;

        // Calculate scale change
        float scaleChange = thumbstickInput * scalingSpeed * Time.deltaTime;

        // Scale the LeftEyeQuad
        Vector3 leftScale = leftQuad.localScale + Vector3.one * scaleChange;
        leftScale.x = Mathf.Clamp(leftScale.x, minScale, maxScale);
        leftScale.y = Mathf.Clamp(leftScale.y, minScale, maxScale);
        leftScale.z = Mathf.Clamp(leftScale.z, minScale, maxScale);
        leftQuad.localScale = leftScale;

        // Scale the RightEyeQuad
        Vector3 rightScale = rightQuad.localScale + Vector3.one * scaleChange;
        rightScale.x = Mathf.Clamp(rightScale.x, minScale, maxScale);
        rightScale.y = Mathf.Clamp(rightScale.y, minScale, maxScale);
        rightScale.z = Mathf.Clamp(rightScale.z, minScale, maxScale);
        rightQuad.localScale = rightScale;
    }
}
