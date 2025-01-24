using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPositionSwitcher : MonoBehaviour
{
    private Vector3 roomPosition = new Vector3(0f, 0f, 0f); // Position inside the room
    private Vector3 externalViewPoint = new Vector3(28.93f, 11.4f, -28.34f); // External viewing point

    private bool isInRoom = true; // Tracks if the player is in the room
    public InputActionReference action;
    void Start()
    {
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            TogglePosition();
        };
    }
    private void TogglePosition()
    {
        if (isInRoom)
        {
            // Move to external viewing point
            transform.position = externalViewPoint;
        }
        else
        {
            // Move back to room position
            transform.position = roomPosition;
        }

        isInRoom = !isInRoom; // Toggle the state
    }
}