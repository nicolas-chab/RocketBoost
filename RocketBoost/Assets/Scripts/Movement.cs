using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] InputAction Thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float ThrustStrength = 1000f;
    [SerializeField] float RotateStrength = 1000f;
    Rigidbody Rb;
    void Start() 
    {
        Rb = GetComponent<Rigidbody>();
    }
    private void OnEnable() 
    {
        Thrust.Enable();
        rotation.Enable();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Thrust.IsPressed())
        {
            Rb.AddRelativeForce(Vector3.up * ThrustStrength * Time.fixedDeltaTime);
        }
        
    }
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if(rotationInput < 0)
        {
            ApplyRotation(RotateStrength);
        }
        else if(rotationInput > 0)
        {
            ApplyRotation(-RotateStrength);
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
    }
}
