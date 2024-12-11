using UnityEngine;

public class EnableStereoCameras : MonoBehaviour
{
    public Camera centerEyeCamera;
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;

    void Start()
    {
        // Disable the CenterEyeAnchor camera first
        if (centerEyeCamera != null)
        {
            centerEyeCamera.enabled = false;
            Debug.Log("CenterEyeCamera disabled.");
        }

        // Enable the LeftEyeAnchor camera
        if (leftEyeCamera != null)
        {
            leftEyeCamera.enabled = true;
            Debug.Log("LeftEyeCamera enabled.");
        }

        // Enable the RightEyeAnchor camera
        if (rightEyeCamera != null)
        {
            rightEyeCamera.enabled = true;
            Debug.Log("RightEyeCamera enabled.");
        }
    }
}
