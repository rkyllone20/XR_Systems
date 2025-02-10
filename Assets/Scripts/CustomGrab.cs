using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    private class HandData
    {
        public Transform handTransform;
        public Vector3 prevPosition;
        public Quaternion prevRotation;
    }

    private Dictionary<Transform, HandData> grabbingHands = new Dictionary<Transform, HandData>();
    private Transform grabbedObject;

    public InputActionReference grabActionLeft; // Assign in Inspector (Left Hand Grip)
    public InputActionReference grabActionRight; // Assign in Inspector (Right Hand Grip)

    public Transform leftHand;
    public Transform rightHand;
    public LayerMask grabbableLayer; // Assign a layer for grabbable objects

    public bool doubleRotation = false; // Extra credit feature

    void Start()
    {
        // Enable Input Actions
        grabActionLeft.action.Enable();
        grabActionRight.action.Enable();

        // Listen for grab/release actions
        grabActionLeft.action.performed += ctx => TryGrab(leftHand);
        grabActionRight.action.performed += ctx => TryGrab(rightHand);
        grabActionLeft.action.canceled += ctx => Release(leftHand);
        grabActionRight.action.canceled += ctx => Release(rightHand);
    }

    void Update()
    {
        if (grabbedObject != null && grabbingHands.Count > 0)
        {
            ApplyTransformations();
        }
    }

    private void TryGrab(Transform hand)
    {
        if (grabbedObject == null)
        {
            grabbedObject = GetClosestGrabbable(hand);
        }

        if (grabbedObject != null && !grabbingHands.ContainsKey(hand))
        {
            HandData handData = new HandData
            {
                handTransform = hand,
                prevPosition = hand.position,
                prevRotation = hand.rotation
            };

            grabbingHands[hand] = handData;
        }
    }

    private void Release(Transform hand)
    {
        if (grabbingHands.ContainsKey(hand))
        {
            grabbingHands.Remove(hand);
        }

        if (grabbingHands.Count == 0)
        {
            grabbedObject = null;
        }
    }

    private Transform GetClosestGrabbable(Transform hand)
    {
        Collider[] colliders = Physics.OverlapSphere(hand.position, 0.2f, grabbableLayer);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            float distance = Vector3.Distance(hand.position, col.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = col.transform;
            }
        }

        return closest;
    }

    private void ApplyTransformations()
        {
        Vector3 totalDeltaPosition = Vector3.zero;
        Quaternion averageRotation = Quaternion.identity;
        bool firstHand = true;
        int handCount = grabbingHands.Count;

        foreach (var handEntry in grabbingHands)
        {
            HandData handData = handEntry.Value;
            Transform handTransform = handData.handTransform;

            // Calculate delta position (translation)
            Vector3 deltaPosition = handTransform.position - handData.prevPosition;
            totalDeltaPosition += deltaPosition;

            // Calculate delta rotation
            Quaternion deltaRotation = handTransform.rotation * Quaternion.Inverse(handData.prevRotation);

            // Average rotations properly
            if (firstHand)
            {
                averageRotation = deltaRotation;
                firstHand = false;
            }
            else
            {
                averageRotation = Quaternion.Slerp(averageRotation, deltaRotation, 0.5f);
            }

            // Update previous state
            handData.prevPosition = handTransform.position;
            handData.prevRotation = handTransform.rotation;
        }

        // Apply transformations
        grabbedObject.position += totalDeltaPosition / handCount; // Average translation
        grabbedObject.rotation = averageRotation * grabbedObject.rotation; // Apply smooth averaged rotation

        // Extra Credit: Double rotation effect
        if (doubleRotation)
        {
            float angle;
            Vector3 axis;
            averageRotation.ToAngleAxis(out angle, out axis);
            grabbedObject.rotation = Quaternion.AngleAxis(angle * 2, axis) * grabbedObject.rotation;
        }
    }
}
