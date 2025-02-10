using UnityEngine;

public class MagnifyingGlassBehavior : MonoBehaviour
{
    public Transform magnifyingGlass; // The physical magnifying glass
    public Camera vrCamera;          // Player's VR Camera
    public Camera magnifyingCamera;  // Camera for render texture

    void LateUpdate()
    {
        // Keep the magnifying camera's position at the glass
        magnifyingCamera.transform.position = magnifyingGlass.position;

        // Rotate the camera without affecting the glass
        magnifyingCamera.transform.rotation = Quaternion.LookRotation(vrCamera.transform.forward, Vector3.up);
    }
}