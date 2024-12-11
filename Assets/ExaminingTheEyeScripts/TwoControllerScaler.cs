using UnityEngine;

public class TwoControllerQuadScaler : MonoBehaviour
{
    public Transform leftHand; // Left controller
    public Transform rightHand; // Right controller
    public LayerMask interactableLayer; // LayerMask for LeftEyeLayer and RightEyeLayer
    public LineRenderer leftRayRenderer; // Optional: Left ray renderer
    public LineRenderer rightRayRenderer; // Optional: Right ray renderer
    public float minScale = 0.1f; // Minimum scale
    public float maxScale = 3.0f; // Maximum scale
    public float hapticStrength = 0.2f; // Haptic feedback strength

    private Transform leftQuad; // Reference to the LeftEyeQuad
    private Transform rightQuad; // Reference to the RightEyeQuad
    private float initialDistance; // Initial distance between controllers
    private Vector3 leftQuadInitialScale; // Initial scale of the LeftEyeQuad
    private Vector3 rightQuadInitialScale; // Initial scale of the RightEyeQuad

    void Update()
    {
        UnityEngine.Debug.Log("**************");
        // Perform raycasting for both controllers
        RaycastHit leftHit, rightHit;

        // Left controller ray
        Ray leftRay = new Ray(leftHand.position, leftHand.forward);
        Transform leftTarget = Physics.Raycast(leftRay, out leftHit, Mathf.Infinity, interactableLayer)
            ? leftHit.transform
            : null;

        if (leftRayRenderer != null)
        {
            leftRayRenderer.SetPosition(0, leftHand.position);
            leftRayRenderer.SetPosition(1, leftTarget ? leftHit.point : leftHand.position + leftHand.forward * 10);
        }

        // Right controller ray
        Ray rightRay = new Ray(rightHand.position, rightHand.forward);
        Transform rightTarget = Physics.Raycast(rightRay, out rightHit, Mathf.Infinity, interactableLayer)
            ? rightHit.transform
            : null;

        if (rightRayRenderer != null)
        {
            rightRayRenderer.SetPosition(0, rightHand.position);
            rightRayRenderer.SetPosition(1, rightTarget ? rightHit.point : rightHand.position + rightHand.forward * 10);
        }

        if(OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && // Left grip
            OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)){
UnityEngine.Debug.Log("LeftTarget: " + leftTarget.name);
        UnityEngine.Debug.Log("RightTarget: " + rightTarget.name);
        UnityEngine.Debug.Log(leftTarget);
        UnityEngine.Debug.Log(rightTarget);
        UnityEngine.Debug.Log("Parent: "+ leftTarget.root.name);
            }

        // Ensure both rays hit valid quads and belong to the same ImageGroup
        if (leftTarget != null && rightTarget != null &&
            leftTarget.root == rightTarget.root && // Check if both targets belong to the same root
            leftTarget.root.CompareTag("ImageGroup") && // Validate it's an ImageGroup
            OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && // Left grip
            OVRInput.Get(OVRInput.Button.SecondaryHandTrigger)) // Right grip
        {
            UnityEngine.Debug.Log("LeftTarget: " + leftTarget.name);
            UnityEngine.Debug.Log("RightTarget: " + rightTarget.name);
            UnityEngine.Debug.Log(leftTarget);
            UnityEngine.Debug.Log(rightTarget);
            UnityEngine.Debug.Log("Parent: "+ leftTarget.root.name);

            // Initialize scaling if not already set
            if (leftQuad == null || rightQuad == null)
            {
                leftQuad = leftTarget;
                rightQuad = rightTarget;

                initialDistance = Vector3.Distance(leftHand.position, rightHand.position);
                leftQuadInitialScale = leftQuad.localScale;
                rightQuadInitialScale = rightQuad.localScale;
            }
            else
            {
                // Scale the quads based on controller distance
                float currentDistance = Vector3.Distance(leftHand.position, rightHand.position);
                float scaleFactor = currentDistance / initialDistance;

                Vector3 newLeftQuadScale = leftQuadInitialScale * scaleFactor;
                Vector3 newRightQuadScale = rightQuadInitialScale * scaleFactor;

                // Clamp the scale to avoid extreme sizes
                newLeftQuadScale.x = Mathf.Clamp(newLeftQuadScale.x, minScale, maxScale);
                newLeftQuadScale.y = Mathf.Clamp(newLeftQuadScale.y, minScale, maxScale);
                newLeftQuadScale.z = Mathf.Clamp(newLeftQuadScale.z, minScale, maxScale);

                newRightQuadScale.x = Mathf.Clamp(newRightQuadScale.x, minScale, maxScale);
                newRightQuadScale.y = Mathf.Clamp(newRightQuadScale.y, minScale, maxScale);
                newRightQuadScale.z = Mathf.Clamp(newRightQuadScale.z, minScale, maxScale);

                // Apply the new scales to the quads
                leftQuad.localScale = newLeftQuadScale;
                rightQuad.localScale = newRightQuadScale;

                // Provide haptic feedback
                OVRInput.SetControllerVibration(hapticStrength, hapticStrength, OVRInput.Controller.LTouch);
                OVRInput.SetControllerVibration(hapticStrength, hapticStrength, OVRInput.Controller.RTouch);
            }
        }
        else
        {
            // Reset if controllers are not targeting the same ImageGroup
            leftQuad = null;
            rightQuad = null;

            // Stop haptic feedback
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        }
    }
}
