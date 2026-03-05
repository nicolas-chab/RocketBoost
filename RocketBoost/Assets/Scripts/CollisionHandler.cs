using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float crashDelay = 1.5f;
    [SerializeField] AudioClip SuccessSFX;
    [SerializeField] AudioClip CrashSFX;
    [SerializeField] ParticleSystem SuccessParticles;
    [SerializeField] ParticleSystem CrashParticles;
    AudioSource audioSource;
    bool isControllable = true;
    bool isCollidable = true;
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        RespondToDebugKeys();
    }
   private void OnCollisionEnter(Collision other) 
   {
        if (!isControllable || !isCollidable) 
            { 
                return; 
            }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                
                break;

            case "Fuel":
                Debug.Log("This thing is fuel");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

  

    private void StartSuccessSequence()
    {
        audioSource.Stop();
        SuccessParticles.Play();
        isControllable = false;
        // make sure the movement script stops all active engine effects
        var move = GetComponent<Movement>();
        if (move != null) move.StopAllEffects();
        move.enabled = false; 
        audioSource.PlayOneShot(SuccessSFX);
        Invoke("LoadNextLevel", crashDelay); 
        
    }
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void StartCrashSequence()
    {
        audioSource.Stop();
        CrashParticles.Play();
        isControllable = false;
        var move = GetComponent<Movement>();
        if (move != null) move.StopAllEffects();
        move.enabled = false;
        audioSource.PlayOneShot(CrashSFX);
        Invoke("ReloadLevel", crashDelay);
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
        
    }
}
