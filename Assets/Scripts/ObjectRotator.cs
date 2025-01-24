using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the object around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}