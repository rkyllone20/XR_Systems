using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour 
{
    public GameObject PointLightObject; // Assign this in the Inspector if needed
    private Light pointLight; // Reference to the Light component
    public InputActionReference action;
    void Start()
    {
        if (PointLightObject != null)
        {
            pointLight = PointLightObject.GetComponent<Light>();
        }
        else
        {
            pointLight = GetComponent<Light>();
        }
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            if (pointLight != null)
            {
                pointLight.color = new Color(Random.value, Random.value, Random.value); //set random color
            }
        };
    }
}