using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAgent : MonoBehaviour
{
    // determines the object's movement
    [SerializeField]
    private Vector3 speed = new Vector3(0f, 0f, 0f);
    [SerializeField]
    private Vector3 bodyPosition = new Vector3(1000f, 0f, 0f);
    [SerializeField] 
    private Vector3 centerPosition = new Vector3(0f, 0f, 0f);
    [SerializeField]
    private float centerMass = 10f;
    [SerializeField]
    private float gravityConstant = 0.0001f;
    
    private const float Tolerance = 2.5f; // non-gravity zone radius
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = bodyPosition;
    }

    void FixedUpdate()
    {
        var currentPosition = transform.position; // save the current position to avoid inefficiency
        var distance = (centerPosition - currentPosition).magnitude;
        var accelerationDirection = centerPosition - currentPosition;
        var acceleration = gravityConstant * centerMass * accelerationDirection.normalized /
            accelerationDirection.magnitude / accelerationDirection.magnitude; // calculate the acceleration

        if (distance < Tolerance) // create the zero gravity zone to avoid artifacts
        {
            acceleration = accelerationDirection * 0f;
        }
 
        // calculate the movement
        var delta = speed * Time.deltaTime + acceleration*(Time.deltaTime*Time.deltaTime/2);
        speed += acceleration * Time.deltaTime;

        transform.Translate(delta);
    }
}
