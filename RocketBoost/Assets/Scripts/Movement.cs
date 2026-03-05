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

    private void OnDisable()
    {
        // when the component is disabled we want to clear everything, not just
        // the main engine (below).  this avoids side thrusters sticking on after
        // a crash or when the scene reloads.
        StopAllEffects();
        Thrust.Disable();
        rotation.Disable();
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

    // stop only the main engine effects; side thrusters remain under the
    // control of the rotation logic so they can work independently.
    public void StopThrusting()
    {
        audioSource.Stop();
        MainEngineParticles.Stop();
    }

    // helper used when the entire movement system is disabled (crash, success,
    // component disabled, etc).  It ensures *all* particle systems and sounds
    // are cleared, including the side thrusters.
    public void StopAllEffects()
    {
        audioSource.Stop();
        MainEngineParticles.Stop();
        LeftThrusterParticles.Stop();
        RightThrusterParticles.Stop();
    }

    private void StartThrusting()
    {
        // apply force every physics frame
        Rb.AddRelativeForce(Vector3.up * ThrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngineSFX);
        }
        // only play once; the playback check below made no sense because we already
        // unconditionally call Play().
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
        else
        {
            RightThrusterParticles.Stop();
            LeftThrusterParticles.Stop();
        }

    }

    private void RotateLeft()
    {
        ApplyRotation(-RotateStrength);
        if (!LeftThrusterParticles.isPlaying)
        {
            RightThrusterParticles.Stop();
            LeftThrusterParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(RotateStrength);
        if (!RightThrusterParticles.isPlaying)
        {
            LeftThrusterParticles.Stop();
            RightThrusterParticles.Play();
        }
        
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        Rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        Rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
