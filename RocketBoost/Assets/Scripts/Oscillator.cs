using Unity.VisualScripting;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
   [SerializeField] Vector3 movementVector;
    Vector3 StartPosition ; 
    float movementFactor; // 0 for not moved, 1 for fully moved
    Vector3 EndPosition;
    [SerializeField] float speed; 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPosition = transform.position;
        EndPosition = StartPosition + movementVector;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(StartPosition, EndPosition, movementFactor);
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);
    }
}
