using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class Movement : MonoBehaviour
{
    [SerializeField] InputAction Thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float ThrustStrength = 1000f;
    [SerializeField] float RotateStrength = 1000f;
    [SerializeField] AudioClip MainEngineSFX;
    [SerializeField] ParticleSystem MainEngineParticles;
    [SerializeField] ParticleSystem LeftThrusterParticles;
    [SerializeField] ParticleSystem RightThrusterParticles;
    AudioSource audioSource;
    Rigidbody Rb;
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
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
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }


    }

    private void StopThrusting()
    {
        audioSource.Stop();
        MainEngineParticles.Stop();
    }

    private void StartThrusting()
    {
        Rb.AddRelativeForce(Vector3.up * ThrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngineSFX);
        }
        MainEngineParticles.Play();
        if (!MainEngineParticles.isPlaying)
        {
            MainEngineParticles.Play();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        
        if(rotationInput < 0)
        {
            RotateRight();
        }
        else if(rotationInput > 0)
        {
            RotateLeft();

        }

    }

    private void RotateLeft()
    {
        ApplyRotation(-RotateStrength);
        RightThrusterParticles.Stop();
        LeftThrusterParticles.Play();
    }

    private void RotateRight()
    {
        ApplyRotation(RotateStrength);
        LeftThrusterParticles.Stop();
        RightThrusterParticles.Play();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        Rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        Rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
