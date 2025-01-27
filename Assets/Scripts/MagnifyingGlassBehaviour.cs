using UnityEngine;

public class MagnifyingGlassBehavior : MonoBehaviour
{
    public Transform magnifyingGlass; // The lens or magnifying glass object
    public Camera vrCamera;          // Player's VR Camera
    public Camera magnifyingCamera;  // Camera for render texture

    void LateUpdate()
    {
        // Align magnifying camera's position with the lens
        magnifyingCamera.transform.position = magnifyingGlass.position;

        // Align magnifying camera's rotation with the player's view direction
        magnifyingCamera.transform.rotation = Quaternion.LookRotation(vrCamera.transform.forward, Vector3.up);
    }
}