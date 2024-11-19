using UnityEngine;
using UnityEngine.UI;

public class GalleryManager : MonoBehaviour
{
    [Header("Quads and Materials")]
    public GameObject leftEyeQuad; // Reference to the LeftEyeQuad GameObject
    public GameObject rightEyeQuad; // Reference to the RightEyeQuad GameObject
    public Material leftEyeMaterial; // Material for the LeftEyeQuad
    public Material rightEyeMaterial; // Material for the RightEyeQuad

    [Header("Gallery Setup")]
    public RawImage[] thumbnails; // Array of RawImage components for the thumbnails
    public Texture[] leftEyeTextures; // Array of textures for the left-eye images
    public Texture[] rightEyeTextures; // Array of textures for the right-eye images

    // Function to set textures for the left and right eye quads
    public void SetImagePair(Texture leftTexture, Texture rightTexture)
    {
        // Assign the left-eye texture to the LeftEyeQuad material
        leftEyeMaterial.mainTexture = leftTexture;
        leftEyeQuad.GetComponent<Renderer>().material = leftEyeMaterial;

        // Assign the right-eye texture to the RightEyeQuad material
        rightEyeMaterial.mainTexture = rightTexture;
        rightEyeQuad.GetComponent<Renderer>().material = rightEyeMaterial;
    }

    // Function to handle thumbnail clicks
    public void OnThumbnailClick(int index)
    {
        // Get the left and right textures for the selected thumbnail
        Texture leftTexture = leftEyeTextures[index];
        Texture rightTexture = rightEyeTextures[index];

        // Update the quads with the selected textures
        SetImagePair(leftTexture, rightTexture);
    }
}
