using UnityEngine;

public class PlaceImagesAroundCamera : MonoBehaviour
{
    public Transform cameraTransform; // Assign the camera transform
    public GameObject[] imageGroups; // Assign the ImageGroup GameObjects
    public float radius = 2.0f; // Radius of the semi-circle
    public float heightOffset = 0.5f; // Height adjustment for the images

    void Start()
    {
        if (cameraTransform == null || imageGroups.Length == 0) return;

        // Calculate angular placement for each image group
        float angleStep = 180f / (imageGroups.Length - 1);
        float currentAngle = -90f; // Start from left (-90 degrees)

        foreach (GameObject imageGroup in imageGroups)
        {
            // Calculate position
            float radian = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(radian) * radius, heightOffset, Mathf.Sin(radian) * radius);
            Vector3 targetPosition = cameraTransform.position + offset;

            // Set position and rotation
            imageGroup.transform.position = targetPosition;
            imageGroup.transform.LookAt(new Vector3(cameraTransform.position.x, imageGroup.transform.position.y, cameraTransform.position.z));

            // Increment angle for the next image
            currentAngle += angleStep;
        }
    }
}
